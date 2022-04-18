using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
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
            {
                query = query.Where(x => (x.TransDate >= filter.DateFrom) &&
                                         (x.TransDate <= filter.DateTo));
            }

            return query.OrderByDescending(x => x.TransDate)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public Task<Policy> PolicySelectThisAsync(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException(nameof(policyNo));

            return _db.Policies.Where(x => x.PolicyNo == policyNo).SingleOrDefaultAsync();
        }

        public async Task<Policy> PolicyCreateAsync<T>(CreateNew<T> newPolicyDto)
            where T : PolicyRequest
        {
            if (newPolicyDto is null)
                throw new ArgumentNullException(nameof(newPolicyDto));

            if (newPolicyDto.PolicyDetails is null)
                throw new ArgumentNullException(nameof(newPolicyDto.PolicyDetails));

            // check for insured
            var insured = await CustomerSelectThisAsync(newPolicyDto.CustomerId);

            if (insured is null)
                throw new ArgumentOutOfRangeException($"This CustomerId [{newPolicyDto.CustomerId}] does not exist");

            // create the policy
            var policy = await CreateNewPolicyAsync(newPolicyDto, insured);
            _db.Policies.Add(policy);

            // create the policy details
            foreach (var detail in newPolicyDto.PolicyDetails)
            {
                var pd = detail.MapToPolicyDetail();
                pd.PolicyNo = policy.PolicyNo;

                _db.PolicyDetails.Add(pd);
            }

            // create a debit note
            var debitNote = CreateNewDebitNote(policy);
            _db.DNCNNotes.Add(debitNote);

            // and also a reciept
            var receipt = CreateNewReceipt(policy, debitNote.refDNCNNo);
            _db.DNCNNotes.Add(receipt);

            //return the policy number
            return policy;
        }

        private DNCNNote CreateNewDebitNote(Policy policy)
        {
            //string dncnNo = Guid.NewGuid().ToString().Split('-')[0];
            string dncnNo = GetNextAutoNumber("[AUTO]", "DNOTE", BRANCH_ID, policy.SubRiskID);

            return new DNCNNote()
            {
                DNCNNo = dncnNo,
                refDNCNNo = dncnNo,
                PolicyNo = policy.PolicyNo,
                CoPolicyNo = policy.CoPolicyNo,
                BranchID = BRANCH_ID,
                BizSource = "DIRECT",
                //BizOption = P.BizOption,
                NoteType = "DN",
                BillingDate = DateTime.Now,
                SubRiskID = policy.SubRiskID,
                SubRisk = policy.SubRisk,
                PartyID = policy.PartyID,
                Party = policy.Party,
                PartyRate = 0,
                InsuredID = policy.InsuredID,
                //InsuredName = P.InsuredName,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,
                SumInsured = policy.SumInsured,
                GrossPremium = policy.GrossPremium,
                Commission = 0,
                PropRate = 100.0,
                ProRataDays = 12L,
                ProRataPremium = 0,
                VatRate = 0.0,
                VatAmount = 0,
                NetAmount = 0,
                Narration = "Being policy premium  for Policy No. " + policy.PolicyNo,
                ExRate = 1.0,
                ExCurrency = "NAIRA",
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                Approval = 1,
                HasTreaty = 1,
                Remarks = "NORMAL",
                TopMostValue = 0,
                PMLValue = 0,
                PaymentType = "NORMAL",
                Deleted = 0,
                DeletedOn = DateTime.Now,
                Active = 1,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                TotalRiskValue = 0,
                TotalPremium = 0,
                RetProp = 0.0,
                RetValue = 0,
                RetPremium = 0,
                DBDate = DateTime.Now,
                A1 = 0,
                A2 = 0,
                A3 = 0,
                A4 = 0,
                A5 = 0,
                A6 = 0,
                A7 = 0,
                A8 = 0,
                A9 = 0,
                A10 = 0
            };
        }

        private DNCNNote CreateNewReceipt(Policy policy, string refDnCnNo)
        {
            string dncnNo = Guid.NewGuid().ToString().Split('-')[0];
            string receiptNo = GetNextAutoNumber("[AUTO]", "RECEIPT", BRANCH_ID, policy.SubRiskID);

            return new DNCNNote()
            {
                DNCNNo = dncnNo,
                refDNCNNo = refDnCnNo,
                ReceiptNo = receiptNo,
                PolicyNo = policy.PolicyNo,
                CoPolicyNo = policy.CoPolicyNo,
                BranchID = policy.BranchID,
                BizSource = policy.BizSource,
                //BizOption = P.BizOption,
                NoteType = "RCP",
                BillingDate = DateTime.Now,
                SubRiskID = policy.SubRiskID,
                SubRisk = policy.SubRisk,
                PartyID = policy.PartyID,
                Party = policy.Party,
                PartyRate = 0,
                InsuredID = policy.InsuredID,
                //InsuredName = P.InsuredName,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,
                SumInsured = policy.SumInsured,
                GrossPremium = policy.GrossPremium,
                Commission = 0,
                PropRate = 100.0,
                ProRataDays = 12L,
                ProRataPremium = 0,
                VatRate = 0.0,
                VatAmount = 0,
                NetAmount = 0,
                Narration = "Being policy premium  for Policy No. " + policy.PolicyNo,
                ExRate = 1.0,
                ExCurrency = "NAIRA",
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                Approval = 1,
                HasTreaty = 1,
                Remarks = "NORMAL",
                TopMostValue = 0,
                PMLValue = 0,
                PaymentType = "NORMAL",
                Deleted = 0,
                DeletedOn = DateTime.Now,
                Active = 1,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                TotalRiskValue = 0,
                TotalPremium = 0,
                RetProp = 0.0,
                RetValue = 0,
                RetPremium = 0,
                DBDate = DateTime.Now,
                A1 = 0,
                A2 = 0,
                A3 = 0,
                A4 = 0,
                A5 = 0,
                A6 = 0,
                A7 = 0,
                A8 = 0,
                A9 = 0,
                A10 = 0
            };
        }

        private async Task<Policy> CreateNewPolicyAsync<T>(CreateNew<T> newPolicyDto, InsuredClient insured)
             where T : PolicyRequest
        {
            var policyNo = GetNextAutoNumber("[AUTO]", "POLICY", BRANCH_ID, newPolicyDto.ProductId);

            var subRisk = await ProductSelectThisAsync(newPolicyDto.ProductId);
            var party = await PartySelectThisOrNullAsync(newPolicyDto.AgentId);

            if (subRisk is null)
                throw new ArgumentOutOfRangeException($"This ProductId [{newPolicyDto.ProductId}] does not exist");

            if (party is null)
                throw new ArgumentOutOfRangeException($"This AgentId [{newPolicyDto.AgentId}] does not exist");

            return new Policy()
            {
                TransDate = newPolicyDto.TransactionDate,
                StartDate = newPolicyDto.StartDate,
                EndDate = newPolicyDto.EndDate,
                SubRiskID = newPolicyDto.ProductId,
                SubRisk = subRisk.SubRiskName,
                PartyID = party.PartyID,
                Party = party.PartyName,
                BranchID = BRANCH_ID,
                Branch = BRANCH_NAME,
                //TrackID = 100L,
                //SourceType = policyDto.BizChannel,
                //ExRate = 1.0,
                //ExRateID = 1L,
                ExCurrency = "NAIRA",
                PremiumRate = 0.0,
                ProportionRate = 0.0,
                SumInsured = newPolicyDto.TotalSumInsured,
                GrossPremium = newPolicyDto.TotalGrossPremium,
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                ProRataDays = 0,
                ProRataPremium = 0,
                //BizSource = policyDto.SourceId,
                Active = 1,
                Deleted = 0,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                PolicyNo = policyNo,
                CoPolicyNo = policyNo,
                TransSTATUS = "PENDING",
                Remarks = "RETAIL",

                //InsStateID = insured.StateOfOrigin,
                InsuredID = insured.InsuredID,
                InsSurname = insured.Surname,
                InsFirstname = insured.FirstName,
                InsOthernames = insured.OtherNames,
                InsAddress = insured.Address,
                InsMobilePhone = insured.MobilePhone,
                InsLandPhone = insured.LandPhone,
                InsEmail = insured.Email,
                InsOccupation = insured.Occupation,
                InsFaxNo = insured.ApiId,
                //InsFullName = insured.Surname + " " + insured.FirstName + " " + insured.OtherNames,
            };
        }
    }
}
