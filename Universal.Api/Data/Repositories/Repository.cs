using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository 
    {
        private readonly UICContext _db;

        public Repository(UICContext db)
        {
            _db = db;
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

        public AuthUser AuthenticateAdmin(string companyName, string password)
        {
            return _db.AuthUsers
                        .Where(a => a.CompanyName == companyName
                                && a.Password == password
                                && a.Status == "ENABLED").SingleOrDefault();
        }

        public async Task<List<Policy>> PolicySelectAsync(FilterPaging filter, string partyId)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Policies.Where(p => p.PartyID == partyId);

            if (filter.CanSearchDate)
            {
                query = query.Where(x => (x.TransDate >= filter.DateFrom) &&
                                         (x.TransDate <= filter.DateTo));
            }

            var policies = await query.OrderByDescending(p => p.TransDate)
                                           .Skip(filter.SkipCount)
                                           .Take(filter.PageSize)
                                           .ToListAsync();
            return policies;
        }

        public Policy PolicySelectThis(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException("Policy No cannot be empty ", nameof(policyNo));

            return _db.Policies.Where(O => O.PolicyNo == policyNo).SingleOrDefault();
        }

        public Policy PolicyCreate(PolicyResult policyDto, IEnumerable<Contracts.V1.PolicyRequest> sectionsDto)
        {
            //var contextTransaction = _db.Database.BeginTransaction();

            if (policyDto == null)
                throw new ArgumentNullException("Policy cannot be empty ", nameof(policyDto));

            if (policyDto.Insured == null)
                throw new ArgumentNullException("Insured cannot be empty ", "Insured");

            if (sectionsDto == null)
                throw new ArgumentNullException("Sections cannot be empty ", nameof(sectionsDto));

            var insured = CustomerSelectThis(policyDto.Insured.CustomerId);

            //var insured = InsuredSelectThis(policyDto.Insured.LastName, policyDto.Insured.DateOfBirth);

            if (insured == null)
            {
                insured = CreateNewInsured(policyDto.Insured, policyDto.AgentId); 
            }

            //get policyNo
            //policyDto.PolicyNo = GeneratePolicyNo(policyDto.ProductId, policyDto.BranchId, policyDto.SourceId);

            var policy = CreateNewPolicy(policyDto, insured);

            _db.Policies.Add(policy);
           // _db.SaveChanges();

            {
                //PolicyDetail policyDetail = new PolicyDetail()
                //{
                //    PolicyNo = policy.PolicyNo,
                //    CoPolicyNo = policy.CoPolicyNo,
                //    EntryDate = DateTime.Now,
                //    StartDate = policy.StartDate,
                //    EndDate = policy.EndDate,
                //    EndorsementNo = "",
                //    BizOption = policy.BizSource,
                //    DNCNNo = "",
                //    CertOrDocNo = "",
                //    InsuredName = insured.FirstName + " " + insured.Surname, // insured.FullName,
                //    LTAStartDate = new DateTime?(),
                //    LTAEndDate = new DateTime?(),
                //    ExRateID = 0, //policy.ExRateID,
                //    ExRate = policy.ExRate,
                //    ExCurrency = policy.ExCurrency,
                //    PremiumRate = policy.PremiumRate,
                //    ProportionRate = policy.ProportionRate,
                //    SumInsured = policy.SumInsured,
                //    GrossPremium = policy.GrossPremium,
                //    SumInsuredFrgn = policy.SumInsuredFrgn,
                //    GrossPremiumFrgn = policy.GrossPremiumFrgn,
                //    ProRataDays = policy.ProRataDays,
                //    ProRataPremium = policy.ProRataPremium,
                //    NetAmount = 0,
                //    Deleted = 0,
                //    Active = 1,
                //    SubmittedBy = "E-CHANNEL",
                //    SubmittedOn = DateTime.Now
                //};

                //_db.PolicyDetails.Add(policyDetail);
                //_db.SaveChanges();

            }


            decimal TotalSumInsured = 0;
            decimal TotalGrossPremium = 0;

            foreach (Contracts.V1.PolicyRequest section in sectionsDto)
            {
                Models.PolicyDetail pd = new Models.PolicyDetail();

                TotalSumInsured += section.SumInsured;
                TotalGrossPremium += section.GrossPremium;

                if (section.GetType().Equals(typeof(PolicyAsAviation)))
                {
                    PolicyAsAviation dto = (PolicyAsAviation)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsBond)))
                {
                    PolicyAsBond dto = (PolicyAsBond)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsEngineering)))
                {
                    PolicyAsEngineering dto = (PolicyAsEngineering)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsFire)))
                {
                    PolicyAsFire dto = (PolicyAsFire)section;
                    //pd = dto.MapPolicyDetail(policy);
                }
                else if (section.GetType().Equals(typeof(PolicyAsGeneralAccident)))
                {
                    PolicyAsGeneralAccident dto = (PolicyAsGeneralAccident)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsMarineCargo)))
                {
                    PolicyAsMarineCargo dto = (PolicyAsMarineCargo)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsMarineHull)))
                {
                    PolicyAsMarineHull dto = (PolicyAsMarineHull)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsMotor)))
                {
                    PolicyAsMotor dto = (PolicyAsMotor)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                else if (section.GetType().Equals(typeof(PolicyAsOilGas)))
                {
                    PolicyAsOilGas dto = (PolicyAsOilGas)section;
                    pd = dto.MapPolicyDetail(policy.PolicyNo);
                }
                _db.PolicyDetails.Add(pd);
            }

            //PolicyDetailUpdate(policyDetail.PolicyNo, TotalSumInsured, TotalGrossPremium);


           
            var debitNote = CreateNewDebitNote(policy, TotalSumInsured, TotalGrossPremium);
            _db.DNCNNotes.Add(debitNote);

            var receipt = CreateNewReceipt(policy, debitNote.refDNCNNo);
            _db.DNCNNotes.Add(receipt);

           // _db.SaveChanges();
           // contextTransaction.Commit();
            return policy;
        }

        private DNCNNote CreateNewDebitNote(Policy P, Decimal TotalSumInsured, Decimal TotalGrossPremium)
        {
            string DNCNNo = Guid.NewGuid().ToString().Split('-')[0];
            return new DNCNNote()
            {
                DNCNNo = DNCNNo,
                refDNCNNo = DNCNNo,
                PolicyNo = P.PolicyNo,
                CoPolicyNo = P.CoPolicyNo,
                BranchID = P.BranchID,
                BizSource = P.BizSource,
                //BizOption = P.BizOption,
                NoteType = "DN",
                BillingDate = DateTime.Now,
                SubRiskID = P.SubRiskID,
                SubRisk = P.SubRisk,
                PartyID = P.PartyID,
                Party = P.Party,
                PartyRate = 0,
                InsuredID = P.InsuredID,
                //InsuredName = P.InsuredName,
                StartDate = P.StartDate,
                EndDate = P.EndDate,
                SumInsured = TotalSumInsured,
                GrossPremium = TotalGrossPremium,
                Commission = 0,
                PropRate = 100.0,
                ProRataDays = 12L,
                ProRataPremium = 0,
                VatRate = 0.0,
                VatAmount = 0,
                NetAmount = 0,
                Narration = "Being policy premium  for Policy No. " + P.PolicyNo,
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
                SubmittedBy = "E-CHANNEL",
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

        private DNCNNote CreateNewReceipt(Policy P, string refDNCNNO)
        {
            string DNCNNo = Guid.NewGuid().ToString().Split('-')[0];
            return new DNCNNote()
            {
                DNCNNo = DNCNNo,
                refDNCNNo = refDNCNNO,
                ReceiptNo = "FORMAT NEEDED",
                PolicyNo = P.PolicyNo,
                CoPolicyNo = P.CoPolicyNo,
                BranchID = P.BranchID,
                BizSource = P.BizSource,
                //BizOption = P.BizOption,
                NoteType = "RCP",
                BillingDate = DateTime.Now,
                SubRiskID = P.SubRiskID,
                SubRisk = P.SubRisk,
                PartyID = P.PartyID,
                Party = P.Party,
                PartyRate = 0,
                InsuredID = P.InsuredID,
                //InsuredName = P.InsuredName,
                StartDate = P.StartDate,
                EndDate = P.EndDate,
                SumInsured = P.SumInsured,
                GrossPremium = P.GrossPremium,
                Commission = 0,
                PropRate = 100.0,
                ProRataDays = 12L,
                ProRataPremium = 0,
                VatRate = 0.0,
                VatAmount = 0,
                NetAmount = 0,
                Narration = "Being policy premium  for Policy No. " + P.PolicyNo,
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
                SubmittedBy = "E-CHANNEL",
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

        private Policy CreateNewPolicy(PolicyResult policyDto, InsuredClient insured)
        {
            Policy policy = new Policy()
            {
                TransDate = policyDto.EntryDate,
                StartDate = policyDto.StartDate,
                EndDate = policyDto.EndDate,
                SubRiskID = policyDto.ProductId,
                PartyID = policyDto.AgentId,
                SubRisk = SubRiskSelectThis(policyDto.ProductId).SubRiskName,
                Party = PartySelectThis(policyDto.AgentId).Party1,
                //Branch = policyDto.BranchId, // BranchSelectThis(policyDto.BranchID).Description,
                //TrackID = 100L,
                //SourceType = policyDto.BizChannel,
                //ExRate = 1.0,
                //ExRateID = 1L,
                ExCurrency = "NAIRA",
                PremiumRate = 0.0,
                ProportionRate = 0.0,
                SumInsured = 0,
                GrossPremium = 0,
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                ProRataDays = 0,
                ProRataPremium = 0,
                //BizSource = policyDto.SourceId,
                Active = 1,
                Deleted = 0,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                PolicyNo = policyDto.PolicyNo,
                CoPolicyNo = policyDto.PolicyNo,
                TransSTATUS = "PENDING",

                InsStateID = policyDto.Insured.StateOfOrigin,
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

            return policy;
        }

        public string GeneratePolicyNo(string productID, string branchID, string bizSource)
        {
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

        public Models.PolicyDetail PolicyDetailSelectThis(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException("Policy No cannot be empty ", nameof(policyNo));

            return _db.PolicyDetails.Where(O => O.PolicyNo == policyNo).OrderBy(O => O.DetailID).ToList().LastOrDefault();
        }
        
    }
}
