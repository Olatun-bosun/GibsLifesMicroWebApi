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
        public Task<List<Policy>> PolicySelectAsync(FilterPaging filter, string partyId)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Policies.Where(x => x.PartyID == partyId);

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
                throw new ArgumentNullException($"This customer {newPolicyDto.CustomerId} does not exist");

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
            string dncnNo = Guid.NewGuid().ToString().Split('-')[0];
            return new DNCNNote()
            {
                DNCNNo = dncnNo,
                refDNCNNo = dncnNo,
                PolicyNo = policy.PolicyNo,
                CoPolicyNo = policy.CoPolicyNo,
                BranchID = "19",
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
                SubmittedBy = "WEB_API",
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
            return new DNCNNote()
            {
                DNCNNo = dncnNo,
                refDNCNNo = refDnCnNo,
                ReceiptNo = $"RT/19/{DateTime.Today.Month}/$00000$",
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
                SubmittedBy = "WEB_API",
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
            var policyNo = GeneratePolicyNo(newPolicyDto.ProductId,
                                            "newPolicyDto.BranchId",
                                            "newPolicyDto.SourceId");

            var subRisk = await ProductSelectThisAsync(newPolicyDto.ProductId);
            var party = await PartySelectThisOrNullAsync(newPolicyDto.AgentId);

            return new Policy()
            {
                TransDate = newPolicyDto.TransactionDate,
                StartDate = newPolicyDto.StartDate,
                EndDate = newPolicyDto.EndDate,
                SubRiskID = newPolicyDto.ProductId,
                PartyID = newPolicyDto.AgentId,
                SubRisk = subRisk.SubRiskName,
                Party = party.PartyName,
                BranchID = "19",
                Branch = "RETAIL OFFICE",
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
                SubmittedBy = "WEB_API",
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

        private string GeneratePolicyNo(string productID, string branchID, string bizSource)
        {
            //InsuredID: $BR$$MO$$YR$$00000$

            //PartyID: 41$00000$$MO$

            //PolicyNo: UIC/$BR2$/$SR$/$00000$/$MO$/$YR$










            //var subRisk = SubRiskSelectThis(productID);
            //var policyAutoNumber = PolicyAutoNumberSelectThis(productID, branchID);
            //var nextValue = policyAutoNumber.NextValue;

            //long? nullable1 = (nextValue.Value > 0 ? (nextValue.HasValue ? 1 : 0) : 0) == 0 ? 1 : policyAutoNumber.NextValue;

            ////TODO here

            //string str;

            //if (bizSource == "ACCEPTED")
            //    str = "IN/" + branchID + "/" + policyAutoNumber.RiskID + "/" + DateTime.Today.Year + "/" + nullable1.Value.ToString("00000");

            ////else if (subRisk.MidRiskID == "401")
            ////    str = "OC/" + branchID + "/" + policyAutoNumber.RiskID + "/" + DateTime.Today.Year + "/" + nullable1.Value.ToString("00000");
            //else
            //    str = "P/" + branchID + "/" + policyAutoNumber.RiskID + "/" + DateTime.Today.Year + "/" + nullable1.Value.ToString("00000");

            //long? SerialNo = nullable1.HasValue ? new long?(nullable1.GetValueOrDefault() + 1) : new long?();

            //PolicyAutoNumberUpdateNextValue(policyAutoNumber.RiskID, policyAutoNumber.BranchID, SerialNo);
            //return str;

            return null;
        }

    }
}
