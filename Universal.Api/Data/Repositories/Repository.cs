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

        public Policy PolicyCreate(PolicyDto policyDto, IEnumerable<PolicySectionDto> sectionsDto)
        {
            var contextTransaction = _db.Database.BeginTransaction();

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
                insured = CustomerCreate(policyDto.Insured); 
            }

            //get policyNo
            string policyNo = GeneratePolicyNo(policyDto.ProductID, policyDto.BranchID, policyDto.BizSource);

            Policy policy = new Policy()
            {
                TransDate = policyDto.EntryDate,
                StartDate = policyDto.StartDate,
                EndDate = policyDto.EndDate,
                SubRiskID = policyDto.ProductID,
                PartyID = policyDto.AgentID,
                BranchID = policyDto.BranchID,
                SubRisk = SubRiskSelectThis(policyDto.ProductID).SubRisk1,
                Party = PartySelectThis(policyDto.AgentID).Party1,
                Branch = policyDto.BranchID, // BranchSelectThis(policyDto.BranchID).Description,
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
                BizSource = policyDto.BizSource,
                Active = 1,
                Deleted = 0,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                PolicyNo = policyNo,
                CoPolicyNo = policyNo,
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
                //InsFullName = insured.Surname + " " + insured.FirstName + " " + insured.OtherNames,
            };

            _db.Policies.Add(policy);
            _db.SaveChanges();

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

            foreach (PolicySectionDto section in sectionsDto)
            {
                PolicyDetail pd = new PolicyDetail();

                TotalSumInsured += section.SectionSumInsured;
                TotalGrossPremium += section.SectionPremium;

                if (section.GetType().Equals(typeof(AviationDto)))
                {
                    AviationDto A = (AviationDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapAviationPolicySection(A);
                }
                else if (section.GetType().Equals(typeof(BondDto)))
                {
                    BondDto B = (BondDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapBondPolicySection(B);
                }
                else if (section.GetType().Equals(typeof(EngineeringDto)))
                {
                    EngineeringDto E = (EngineeringDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapEngineeringPolicySection(E);
                }
                else if (section.GetType().Equals(typeof(FireDto)))
                {
                    FireDto F = (FireDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapFirePolicySection(F);
                }
                else if (section.GetType().Equals(typeof(GeneralAccidentDto)))
                {
                    GeneralAccidentDto G = (GeneralAccidentDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapGeneralAccidentPolicySection(G);
                }
                else if (section.GetType().Equals(typeof(MarineCargoDto)))
                {
                    MarineCargoDto M = (MarineCargoDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapMarineCargoPolicySection(M);
                }
                else if (section.GetType().Equals(typeof(MarineHullDto)))
                {
                    MarineHullDto H = (MarineHullDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapMarineHullPolicySection(H);
                }
                else if (section.GetType().Equals(typeof(MotorDto)))
                {
                    MotorDto M = (MotorDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    //ptd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapMotorPolicySection(M);
                }
                else if (section.GetType().Equals(typeof(OilGasDto)))
                {
                    OilGasDto O = (OilGasDto)section;
                    pd.PolicyNo = policy.PolicyNo;
                    pd.DetailID = policyDetail.DetailID;
                    pd.Tag = "NEW";
                    pd = pd.MapOilGasPolicySection(O);
                }
                _db.PolicyDetails.Add(pd);
            }

            //PolicyDetailUpdate(policyDetail.PolicyNo, TotalSumInsured, TotalGrossPremium);

            var newNote = Extensions.MapDNNoteForPolicy(policy, policyDetail, TotalSumInsured, TotalGrossPremium);

            _db.DNCNNotes.Add(newNote);
            _db.SaveChanges();

            contextTransaction.Commit();
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

        //public PolicyAutoNumber PolicyAutoNumberSelectThis(string productID, string branchID)
        //{
        //    if (string.IsNullOrWhiteSpace(productID))
        //        throw new ArgumentNullException("Product ID cannot be empty ", nameof(productID));

        //    if (string.IsNullOrWhiteSpace(branchID))
        //        throw new ArgumentNullException("Branch ID cannot be empty ", nameof(branchID));

        //    var policyAutoNumber = _db.PolicyAutoNumbers.Where(O => O.RiskID == productID && O.BranchID == branchID).SingleOrDefault();

        //    if (policyAutoNumber != null)
        //        return policyAutoNumber;

        //    throw new KeyNotFoundException("Branch ID does not exist");
        //}

        //public void PolicyAutoNumberUpdateNextValue(string productID, string branchID, long? serialNo)
        //{
        //    if (string.IsNullOrWhiteSpace(productID))
        //        throw new ArgumentNullException("Product ID cannot be empty ", nameof(productID));

        //    if (string.IsNullOrWhiteSpace(branchID))
        //        throw new ArgumentNullException("Branch ID cannot be empty ", nameof(branchID));

        //    var policyAutoNumber = PolicyAutoNumberSelectThis(productID, branchID);

        //    if (policyAutoNumber == null)
        //        throw new KeyNotFoundException("Branch ID does not exist");

        //    policyAutoNumber.NextValue = serialNo;
        //    _db.SaveChanges();
        //}

        public void PolicyDetailUpdate(
          string policyNo,
          decimal totalSumInsured,
          decimal totalGrossPremium)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException("Policy No cannot be empty ", nameof(policyNo));

            if (totalSumInsured <= decimal.Zero)
                throw new ArgumentException("Total SumInsured cannot be less than zero ", nameof(totalSumInsured));

            if (totalGrossPremium <= decimal.Zero)
                throw new ArgumentException("Total GrossPremium cannot be less than zero ", nameof(totalGrossPremium));

            PolicyDetail policyDetail = PolicyDetailSelectThis(policyNo);

            if (policyDetail == null)
                throw new KeyNotFoundException("Policy No does not exist");

            policyDetail.SumInsured = totalSumInsured;
            policyDetail.GrossPremium = totalGrossPremium;

            _db.SaveChanges();
        }

        public PolicyDetail PolicyDetailSelectThis(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException("Policy No cannot be empty ", nameof(policyNo));

            return _db.PolicyDetails.Where(O => O.PolicyNo == policyNo).OrderBy(O => O.DetailID).ToList().LastOrDefault();
        }

        public void PolicyDelete(string policyNo)
        {
            if (string.IsNullOrWhiteSpace(policyNo))
                throw new ArgumentNullException("Policy No cannot be empty", nameof(policyNo));

            var policy = PolicySelectThis(policyNo);

            if (policy == null)
                throw new KeyNotFoundException("Policy No does not exist");

            //mark policy as deleted.
            policy.Active = Convert.ToByte(false);
            policy.Deleted = Convert.ToByte(true);

            _db.Policies.Update(policy);
            _db.SaveChanges();
        }


        
    }
}
