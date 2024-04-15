using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GibsLifesMicroWebApi.Contracts.V1;
using GibsLifesMicroWebApi.Models;
using GibsLifesMicroWebApi.Domain;

namespace GibsLifesMicroWebApi.Data.Repositories
{
    public partial class Repository
    {
        public async Task<Policy> PolicySelectThisAsync(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException(nameof(policyNo));

            var p = await _db.Policies.FirstOrDefaultAsync(x => x.PolicyNo == policyNo);

            if (p != null)
                p.DebitNote = await _db.DNCNNotes.FirstOrDefaultAsync(z => z.PolicyNo == policyNo &&
                                                                           z.NoteType == "DN");
            return p;
        }

        public Task<List<Policy>> PolicySelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Policies.Where(x => x.Deleted == 0);

            if (_authContext.User is AppUser u)
                query = query.Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{u.AppId}");

            else if (_authContext.User is AgentUser a)
                query = query.Where(x => x.PartyID == a.PartyId);

            else if (_authContext.User is CustomerUser c)
                query = query.Where(x => x.InsuredID == c.InsuredId);

            if (filter.CanSearchDate)
                query = query.Where(x => (x.TransDate >= filter.DateFrom) &&
                                         (x.TransDate <= filter.DateTo));

            return query.OrderByDescending(x => x.TransDate)
                        //.Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        //public Task<List<PolicyDetail>> PolicyDetailSelectAsync(string policyNo)
        //{
        //    if (string.IsNullOrWhiteSpace(policyNo))
        //        throw new ArgumentNullException(nameof(policyNo));

        //    var query = _db.PolicyDetails.Where(x => x.PolicyNo == policyNo);

        //    return query.OrderByDescending(x => x.StartDate)
        //                //.Skip(filter.SkipCount)
        //                .ToListAsync();
        //}

        public async Task<Policy> PolicyCreateAsync<T>(CreateNew<T> newPolicyDto)
            where T : RiskDetail
        {
            if (newPolicyDto is null)
                throw new ArgumentNullException(nameof(newPolicyDto));

            if (newPolicyDto.PolicySections is null)
                throw new ArgumentNullException(nameof(newPolicyDto.PolicySections));

            // check for insured, party, product
            var insured = await CustomerGetOrAddAsync(newPolicyDto);
            if (insured is null)
                throw new ArgumentOutOfRangeException($"This CustomerId or Insured does not exist");

            var subRisk = await ProductSelectThisAsync(newPolicyDto.ProductID);
            if (subRisk is null)
                throw new ArgumentOutOfRangeException($"This ProductId [{newPolicyDto.ProductID}] does not exist");

            var agents = await AgentSelectThisAsync(newPolicyDto.AgentID);
            if (agents is null)
                throw new ArgumentOutOfRangeException($"This AgentId [{newPolicyDto.AgentID}] does not exist");

            var branch = await BranchSelectThisAsync(_authContext.User.AppId);
            if (branch is null)
                throw new ArgumentOutOfRangeException($"No BranchId for [{_authContext.User.AppId}]");

            // create the policy
            var policy = CreateNewPolicy(newPolicyDto, insured, branch, agents, subRisk);
            _db.Policies.Add(policy);

            policy.PolicyDetails = new List<PolicyDetail>();

            // create the policy details
            foreach (var detailDto in newPolicyDto.PolicySections)
            {
                var policyDetail = CreateNewPolicyDetail(detailDto, policy);
                policy.PolicyDetails.Add(policyDetail);
                //_db.PolicyDetails.Add(policyDetail);
            }

            // create a debit note 
            var debitNote = CreateNewDebitNote(policy);
            _db.DNCNNotes.Add(debitNote);

            policy.DebitNote = debitNote; //add the DN to the policy object
            policy.SubRisk = subRisk;     //add the subRisk to the policy object

            var hasPaid = await PaymentValidate(newPolicyDto.PaymentReferenceID, newPolicyDto.PaymentProviderID);

            if (hasPaid)
            {
                // and also a reciept
                var receipt = CreateNewReceipt(policy, debitNote.refDNCNNo);
                _db.DNCNNotes.Add(receipt);
            }

            //return the policy number
            return policy;
        }

        //public void SaveNaicomStatus(Policy policy, NaicomDetail naicom)
        //{
        //    policy.Z_NAICOM_UID = naicom.UniqueID;

        //    policy.DebitNote.Z_NAICOM_UID = naicom.UniqueID;
        //    policy.DebitNote.Z_NAICOM_STATUS = naicom.Status.ToString();
        //    policy.DebitNote.Z_NAICOM_SENT_ON = naicom.SubmitDate;
        //    policy.DebitNote.Z_NAICOM_ERROR = naicom.ErrorMessage;
        //    policy.DebitNote.Z_NAICOM_JSON = naicom.JsonPayload;
        //}

        private Policy CreateNewPolicy<T>(CreateNew<T> newPolicyDto, InsuredClient insured, Branch branch, Agents agents, SubRisk subRisk)
             where T : RiskDetail
        {
            if (newPolicyDto.StartDate >= newPolicyDto.EndDate)
                throw new ArgumentOutOfRangeException(nameof(newPolicyDto.StartDate),
                    $"{nameof(newPolicyDto.StartDate)} cannot be later than {nameof(newPolicyDto.EndDate)}");

            var policyNo = GetNextAutoNumber("POLICY", branch.BranchID, newPolicyDto.ProductID);

            var TotalSumInsured = newPolicyDto.PolicySections.Sum(x => x.SectionSumInsured);
            var TotalPremium = newPolicyDto.PolicySections.Sum(x => x.SectionPremium);

            if (TotalSumInsured <= 0)
                throw new InvalidOperationException($"Total [SectionSumInsured] cannot be zero");

            if (TotalPremium <= 0)
                throw new InvalidOperationException($"Total [SectionPremium] cannot be zero");

            return new Policy()
            {
                //save the PolicyNo from caller to MktStaffID //14-feb-23
                MktStaffID = newPolicyDto.PolicyNo,

                PolicyNo = policyNo,
                CoPolicyNo = null,
                TransDate = DateTime.Now,
                StartDate = newPolicyDto.StartDate,
                EndDate = newPolicyDto.EndDate,
                SubRiskID = subRisk.SubRiskID,
                SubRiskName = subRisk.SubRiskName,
                PartyID = agents.AgentID,
                Party = agents.Agent,
                BranchID = branch.BranchID,
                Branch = branch.Description,

                InsStateID = null,
                InsuredID = insured.InsuredID,
                InsSurname = insured.Surname,
                InsFirstname = insured.FirstName,
                InsOthernames = insured.OtherNames,
                InsAddress = insured.Address,
                InsMobilePhone = insured.MobilePhone,
                InsLandPhone = insured.LandPhone,
                InsEmail = insured.Email,
                InsOccupation = insured.Occupation,
                InsFaxNo = "0" /*insured.ApiId*/,

                InsuredClient = insured, //hmmm

                ExRate = 1,
                ExCurrency = "NAIRA",  
                PremiumRate = 0,
                ProportionRate = 100,
                SumInsured = TotalSumInsured,
                GrossPremium = TotalPremium, 
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                ProRataDays = (int)(newPolicyDto.EndDate - newPolicyDto.StartDate).TotalDays + 1,
                ProRataPremium = 0, 
                IsProposal = 0, 
                BizSource = "DIRECT",  
                TransSTATUS = "PENDING",
                Remarks = "RETAIL", 

                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                Active = 1,
                Deleted = 0, 
            };
        }

        private PolicyDetail CreateNewPolicyDetail<T>(T detailDto, Policy policy)
            where T : RiskDetail
        {
            string endorseNo = GetNextAutoNumber("INVOICE", BRANCH_ID); 
            var pd = detailDto.ToPolicyDetail();

            //pd.PolicyNo = policy.PolicyNo;
            pd.Policy = policy;
            pd.ExRate = policy.ExRate;

            pd.EndorsementNo = endorseNo;
            pd.CertOrDocNo = detailDto.CertificateNo; 
            pd.EntryDate = policy.TransDate;
            pd.BizOption = "NEW";
            pd.InsuredName = policy.InsFullname;
            pd.StartDate = policy.StartDate;
            pd.EndDate = policy.EndDate;
            pd.ExRate = policy.ExRate;
            pd.ExCurrency = policy.ExCurrency;
            pd.PremiumRate = policy.PremiumRate;
            pd.ProportionRate = policy.ProportionRate;
            pd.ProRataDays = policy.ProRataDays;

            pd.SumInsured = detailDto.SectionSumInsured;
            pd.TotalRiskValue = detailDto.SectionSumInsured;
            pd.GrossPremium = detailDto.SectionPremium;
            pd.SumInsuredFrgn = 0;
            pd.GrossPremiumFrgn = 0;
            //pd.ProRataPremium = CDbl(PolicyAmount);
            //pd.NetAmount = CDbl(PolicyAmount);
            //pd.Field49 = CDbl(PolicyAmount);
            pd.Field50 = "PENDING";

            pd.SubmittedBy = policy.SubmittedBy;
            pd.SubmittedOn = policy.SubmittedOn;
            pd.Deleted = policy.Deleted;
            pd.Active = policy.Active;

            return pd;
        }

        private DNCNNote CreateNewDebitNote(Policy policy)
        {
            decimal partyRate = 0;
            string dncnNo = GetNextAutoNumber("DNOTE", BRANCH_ID, policy.SubRiskID);
            decimal? commission = (policy.GrossPremium * partyRate) / 100;

            return new DNCNNote()
            {
                NoteType = "DN",

                DNCNNo = dncnNo,
                refDNCNNo = dncnNo,
                PolicyNo = policy.PolicyNo,
                CoPolicyNo = policy.CoPolicyNo,
                BranchID = policy.BranchID,
                BizSource = policy.BizSource,
                BizOption = "NEW",
                BillingDate = policy.TransDate,
                SubRiskID = policy.SubRiskID,
                SubRisk = policy.SubRiskName,
                PartyID = policy.PartyID,
                Party = policy.Party,
                InsuredID = policy.InsuredID,
                InsuredName = policy.InsFullname,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,

                Narration = $"Being policy premium  for Policy No. {policy.PolicyNo}",
                ExRate = policy.ExRate,
                ExCurrency = policy.ExCurrency,
                Remarks = "NORMAL",
                PaymentType = "NORMAL",

                SumInsured = policy.SumInsured,
                GrossPremium = policy.GrossPremium,

                PartyRate = partyRate,
                Commission = commission,
                PropRate = policy.ProportionRate,
                ProRataDays = policy.ProRataDays,
                ProRataPremium = policy.ProRataPremium,
                NetAmount = policy.GrossPremium - commission,
                SumInsuredFrgn = policy.SumInsuredFrgn,
                GrossPremiumFrgn = policy.GrossPremiumFrgn,
                TotalRiskValue = policy.SumInsured,
                TotalPremium = policy.GrossPremium,
                HasTreaty = 0,
                Approval = 0,
                RetProp = 0,
                RetValue = 0,
                RetPremium = 0,
                DBDate = policy.TransDate, 

                SubmittedBy = policy.SubmittedBy,
                SubmittedOn = policy.SubmittedOn,
                Deleted = policy.Deleted,
                Active = policy.Active,

                //Z_NAICOM_UID = null,
                //Z_NAICOM_STATUS = "QUEUED",  // CIP -> PENDING, SENT, IGNORED, ARCHIVED
                // Z_NAICOM_SENT_ON
                // Z_NAICOM_ERROR
                // Z_NAICOM_JSON
            };
        }

        private DNCNNote CreateNewReceipt(Policy policy, string refDnCnNo)
        {
            decimal partyRate = 0;
            string dncnNo = Guid.NewGuid().ToString().Split('-')[0];
            string receiptNo = GetNextAutoNumber("RECEIPT", BRANCH_ID, policy.SubRiskID);
            decimal? commission = (policy.GrossPremium * partyRate) / 100;

            return new DNCNNote()
            {
                NoteType = "RCP",

                DNCNNo = dncnNo,
                refDNCNNo = refDnCnNo,
                ReceiptNo = receiptNo,
                PolicyNo = policy.PolicyNo,
                CoPolicyNo = policy.CoPolicyNo,
                BranchID = policy.BranchID,
                BizSource = policy.BizSource,
                BizOption = "NEW",
                BillingDate = policy.TransDate,
                SubRiskID = policy.SubRiskID,
                SubRisk = policy.SubRiskName,
                PartyID = policy.PartyID,
                Party = policy.Party,
                InsuredID = policy.InsuredID,
                InsuredName = policy.InsFullname,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,

                Narration = $"Being reciept for Debit Note No. {refDnCnNo}",
                ExRate = policy.ExRate,
                ExCurrency = policy.ExCurrency,
                Remarks = "NORMAL",
                PaymentType = "NORMAL",

                SumInsured = policy.SumInsured,
                GrossPremium = policy.GrossPremium,

                PartyRate = partyRate,
                Commission = commission,
                PropRate = policy.ProportionRate,
                ProRataDays = policy.ProRataDays,
                ProRataPremium = policy.ProRataPremium,
                NetAmount = policy.GrossPremium - commission,
                SumInsuredFrgn = policy.SumInsuredFrgn,
                GrossPremiumFrgn = policy.GrossPremiumFrgn,
                TotalRiskValue = policy.SumInsured,
                TotalPremium = policy.GrossPremium,
                Approval = 0,
                HasTreaty = 0,
                RetProp = 0,
                RetValue = 0,
                RetPremium = 0,
                DBDate = policy.TransDate,

                SubmittedBy = policy.SubmittedBy,
                SubmittedOn = policy.SubmittedOn,
                Active = policy.Active,
                Deleted = policy.Deleted,
            };
        }

        private async static Task<bool> PaymentValidate( string merchantId ,string transactionId)
        {
            await Task.Delay(50);

            if (string.IsNullOrEmpty( merchantId))
                return false;

            if (string.IsNullOrEmpty(transactionId))
                return false;

            if (merchantId.ToUpper() == "PAYSTACK")
            {
                if (transactionId == "DEMO")
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}
