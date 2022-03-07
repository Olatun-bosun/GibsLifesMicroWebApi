using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public interface IRepository
    {
        Party PartySelectThis(string agentID);
        Claim ClaimSelectThis(string claimNo);
        Task<List<Claim>> ClaimsSelectAsync(FilterPaging filter);
        Task<List<Party>> PartySelectAsync(string searchText, int pageNo, int pageSize);
        void PolicyDelete(string policyNo);
        Task<List<Policy>> PolicySelectAsync(FilterPaging filter, string partyId);
        Policy PolicySelectThis(string policyNo);
        string PartyCreate(AgentDto agentDto);
        SubRisk SubRiskSelectThis(string subRiskID);
        Task<List<SubRisk>> SubRisksSelectAsync(string searchText, int pageNo, int pageSize);
        InsuredClient CustomerSelectThis(string customerID);
        Task<List<InsuredClient>> CustomerSelectAsync(string searchText, int pageNo, int pageSize);
        AuthUser Authenticate(string companyName, string password);
    }

    public class Repository : IRepository
    {
        private readonly UICContext _db;

        public Repository(UICContext db)
        {
            _db = db;
        }

        public AuthUser Authenticate(string companyName, string password)
        {
            return  _db.AuthUsers
                        .Where(a => a.CompanyName == companyName 
                                && a.Password == password 
                                && a.Status == "ENABLED").SingleOrDefault();
        }

        public async Task<List<Claim>> ClaimsSelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.ClaimsReserved.AsQueryable();
            if (filter.CanSearchDate)
            {
                query = query.Where(x => (x.EntryDate >= filter.DateFrom) &&
                                         (x.EntryDate <= filter.DateTo));
            }

            var claims = await query.OrderByDescending(c => c.EntryDate)
                                    .Skip(filter.SkipCount)
                                    .Take(filter.PageSize)
                                    .ToListAsync();
            return claims;
        }

        public Claim ClaimSelectThis(string claimNo)
        {
            if (string.IsNullOrWhiteSpace(claimNo))
                throw new ArgumentNullException("Claim No cannot be empty ", nameof(claimNo));

            return _db.ClaimsReserved.Where(O => O.ClaimNo == claimNo).SingleOrDefault();
        }

        public Claim ClaimCreate(ClaimDto claimDto)
        {
            Policy policy = PolicySelectThis(claimDto.PolicyNo);

            if (policy == null)
                throw new KeyNotFoundException("Policy No you supplied is invalid");

            var claim = new Claim
            {
                //NotificatnNo = GenerateNotificationNo(policy.SubRiskID, policy.BranchID),
                PolicyNo = claimDto.PolicyNo,
                NotifyDate = claimDto.NotifyDate.ToUniversalTime(),
                LossDate = claimDto.LossDate.ToUniversalTime(),
                LossDetails = claimDto.LossDetails,
                //InsuredName = policy.InsFullName,
                BranchID = policy.BranchID,
                //SumInsured = policy.SumInsured,
                InsuredID = policy.InsuredID,
                //RegStatus = "PENDING",
                Approval = 0,
                Active = 1,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1).AddDays(-1.0),
                EntryDate = DateTime.Now,
                Party = policy.Party,
                SubRisk = policy.SubRisk,
                //Premium = policy.GrossPremium
            };
            _db.ClaimsReserved.Add(claim);
            //_db.SaveChanges();
            return claim;
        }




        public async Task<List<Party>> PartySelectAsync(string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.Parties.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                char[] chArray = new char[1] { ' ' };
                foreach (string A in searchText.Split(chArray))
                {
                    query = query.Where(O => O.Party1.Contains(A)).AsQueryable();
                }
            }

            var agents = await query.OrderBy(o => o.Party1).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            return agents;
        }

        public Party PartySelectThis(string agentID)
        {
            if (string.IsNullOrWhiteSpace(agentID))
                throw new ArgumentNullException("Agent ID cannot be empty ", "AgentID");

            var party = _db.Parties.Where(O => O.PartyID == agentID).SingleOrDefault();

            if (party != null)
                return party;

            throw new KeyNotFoundException("Agent ID does not exist");
        }

        public string PartyCreate(AgentDto agentDto)
        {
            if (agentDto.CommRate <= decimal.Zero)
                throw new ArgumentException("Comm Rate cannot be less than or equal to zero ", nameof(agentDto.CommRate));

            if (agentDto.CreditLimit <= decimal.Zero)
                throw new ArgumentException("Credit Limit cannot be less than or equal to zero ", nameof(agentDto.CreditLimit));

            Party agent = new Party()
            {
                PartyID = agentDto.AgentID,
                Party1 = agentDto.AgentName,
                Address = agentDto.Address,
                LandPhone = agentDto.Telephone,
                mobilePhone = agentDto.MobilePhone,
                Email = agentDto.Email,
                ComRate = agentDto.CommRate,
                CreditLimit = agentDto.CreditLimit,
                PartyType = "AG",
                InsContact = agentDto.InsContact,
                FinContact = agentDto.FinContact,
                Remarks = agentDto.Remarks
            };
            _db.Parties.Add(agent);
            _db.SaveChanges();

            return agent.PartyID;
        }

        public InsuredClient CustomerSelectThis(string customerID)
        {
            if (string.IsNullOrWhiteSpace(customerID))
                throw new ArgumentNullException(nameof(customerID), "Customer ID cannot be empty.");

            var insuredClient = _db.InsuredClients.Where(I => I.InsuredID == customerID).SingleOrDefault();

            if (insuredClient != null)
                return insuredClient;

            throw new KeyNotFoundException("Customer ID does not exist.");
        }

        public async Task<List<InsuredClient>> CustomerSelectAsync(string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.InsuredClients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText)) {
                query = query.Where(C => C.Surname.Contains(searchText) || C.FirstName.Contains(searchText));
            }

            var skipCount = (pageNo - 1) * pageSize;
            var customers = await query.OrderBy(o => o.Surname).Skip(skipCount).Take(pageSize).ToListAsync();
            return customers;
        }



        public async Task<List<Policy>> PolicySelectAsync(FilterPaging filter, string partyId)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Policies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(partyId))
            {
                query = query.Where(p => p.PartyID == partyId);
            }

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

        //public Policy PolicyCreate(PolicyDto policyDto, IEnumerable<PolicySectionDto> sectionsDto)
        //{
        //    var contextTransaction = _db.Database.BeginTransaction();

        //    if (policyDto == null)
        //        throw new ArgumentNullException("Policy cannot be empty ", nameof(policyDto));

        //    if (policyDto.Insured == null)
        //        throw new ArgumentNullException("Insured cannot be empty ", "Insured");

        //    if (sectionsDto == null)
        //        throw new ArgumentNullException("Sections cannot be empty ", nameof(sectionsDto));

        //    var insured = InsuredSelectThis(policyDto.Insured.LastName, policyDto.Insured.DateOfBirth);

        //    if (insured == null)
        //    {
        //        var newInsured = new InsuredClient
        //        {
        //            InsuredID = Guid.NewGuid().ToString().Split('-')[0].ToUpper(),
        //            Address = policyDto.Insured.AddressProof,
        //            Email = policyDto.Insured.Email,
        //            FirstName = policyDto.Insured.FirstName,
        //            //FullName = policyDto.Insured.Title + " " + policyDto.Insured.LastName + " " + policyDto.Insured.FirstName + " " + policyDto.Insured.LastName,
        //            //DOB = policyDto.Insured.DateOfBirth,
        //            //InsuredType = policyDto.Insured.InsuredType,
        //            LandPhone = policyDto.Insured.Telephone,
        //            MobilePhone = policyDto.Insured.MobilePhone,
        //            //MeansID = policyDto.Insured.Identification,
        //            //MeansIDNo = policyDto.Insured.IdentificationNo,
        //            Occupation = policyDto.Insured.Industry,
        //            OtherNames = policyDto.Insured.OtherName,
        //            //Profile = policyDto.Insured.RiskProfiling,
        //            SubmittedBy = "E-CHANNEL",
        //            SubmittedOn = DateTime.Now,
        //            Surname = policyDto.Insured.LastName,
        //            Active = 1,
        //            Deleted = 0,
        //        };

        //        _db.InsuredClients.Add(newInsured);
        //        _db.SaveChanges();

        //        insured = newInsured;
        //    }

        //    //get policyNo
        //    string policyNo = GeneratePolicyNo(policyDto.ProductID, policyDto.BranchID, policyDto.BizSource);

        //    Policy policy = new Policy()
        //    {
        //        TransDate = policyDto.EntryDate,
        //        StartDate = policyDto.StartDate,
        //        EndDate = policyDto.EndDate,
        //        SubRiskID = policyDto.ProductID,
        //        PartyID = policyDto.AgentID,
        //        BranchID = policyDto.BranchID,
        //        SubRisk = SubRiskSelectThis(policyDto.ProductID).SubRisk1,
        //        Party = PartySelectThis(policyDto.AgentID).Party1,
        //        Branch = BranchSelectThis(policyDto.BranchID).Description,
        //        //TrackID = 100L,
        //        //SourceType = policyDto.BizChannel,
        //        //A1 = 0,
        //        //A2 = 0,
        //        //A3 = 0,
        //        //A4 = 0,
        //        //A5 = 0,
        //        //A6 = 0,
        //        //A7 = 0,
        //        //A8 = 0,
        //        //A9 = 0,
        //        //A10 = 0,
        //        //A11 = 0,
        //        //A12 = 0,
        //        //A13 = 0,
        //        //A14 = 0,
        //        //A15 = 0,
        //        //ExRate = 1.0,
        //        //ExRateID = 1L,
        //        ExCurrency = "NAIRA",
        //        PremiumRate = 0.0,
        //        ProportionRate = 0.0,
        //        SumInsured = 0,
        //        GrossPremium = 0,
        //        SumInsuredFrgn = 0,
        //        GrossPremiumFrgn = 0,
        //        ProRataDays = 0,
        //        ProRataPremium = 0,
        //        BizSource = policyDto.BizSource,
        //        Active = 1,
        //        Deleted = 0,
        //        SubmittedBy = "E-CHANNEL",
        //        SubmittedOn = DateTime.Now,
        //        PolicyNo = policyNo,
        //        CoPolicyNo = policyNo,
        //        TransSTATUS = "PENDING",

        //        InsStateID = policyDto.Insured.StateOfOrigin,
        //        InsuredID = insured.InsuredID,
        //        InsSurname = insured.Surname,
        //        InsFirstname = insured.FirstName,
        //        InsOthernames = insured.OtherNames,
        //        InsAddress = insured.Address,
        //        InsMobilePhone = insured.MobilePhone,
        //        InsLandPhone = insured.LandPhone,
        //        InsEmail = insured.Email,
        //        InsOccupation = insured.Occupation,
        //        //InsFullName = insured.Surname + " " + insured.FirstName + " " + insured.OtherNames,
        //    };

        //    _db.Policies.Add(policy);
        //    _db.SaveChanges();

        //    PolicyDetail policyDetail = new PolicyDetail()
        //    {
        //        PolicyNo = policy.PolicyNo,
        //        CoPolicyNo = policy.CoPolicyNo,
        //        EntryDate = DateTime.Now,
        //        StartDate = policy.StartDate,
        //        EndDate = policy.EndDate,
        //        EndorsementNo = "",
        //        BizOption = policy.BizSource,
        //        DNCNNo = "",
        //        CertOrDocNo = "",
        //        InsuredName = insured.FullName,
        //        LTAStartDate = new DateTime?(),
        //        LTAEndDate = new DateTime?(),
        //        ExRateID = policy.ExRateID,
        //        ExRate = policy.ExRate,
        //        ExCurrency = policy.ExCurrency,
        //        PremiumRate = policy.PremiumRate,
        //        ProportionRate = policy.ProportionRate,
        //        SumInsured = policy.SumInsured,
        //        GrossPremium = policy.GrossPremium,
        //        SumInsuredFrgn = policy.SumInsuredFrgn,
        //        GrossPremiumFrgn = policy.GrossPremiumFrgn,
        //        ProRataDays = policy.ProRataDays,
        //        ProRataPremium = policy.ProRataPremium,
        //        NetAmount = 0,
        //        Deleted = 0,
        //        Active = 1,
        //        SubmittedBy = "E-CHANNEL",
        //        SubmittedOn = DateTime.Now
        //    };

        //    _db.PolicyDetails.Add(policyDetail);
        //    _db.SaveChanges();

        //    if (sectionsDto == null)
        //        throw new ArgumentException("Sections cannot be empty ", nameof(sectionsDto));

        //    decimal TotalSumInsured = 0;
        //    decimal TotalGrossPremium = 0;

        //    foreach (PolicySectionDto section in sectionsDto)
        //    {
        //        PolicyTempDetail ptd = new PolicyTempDetail();

        //        TotalSumInsured += section.SectionSumInsured;
        //        TotalGrossPremium += section.SectionPremium;

        //        if (section.GetType().Equals(typeof(AviationDto)))
        //        {
        //            AviationDto A = (AviationDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapAviationPolicySection(A);
        //        }
        //        else if (section.GetType().Equals(typeof(BondDto)))
        //        {
        //            BondDto B = (BondDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapBondPolicySection(B);
        //        }
        //        else if (section.GetType().Equals(typeof(EngineeringDto)))
        //        {
        //            EngineeringDto E = (EngineeringDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapEngineeringPolicySection(E);
        //        }
        //        else if (section.GetType().Equals(typeof(FireDto)))
        //        {
        //            FireDto F = (FireDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapFirePolicySection(F);
        //        }
        //        else if (section.GetType().Equals(typeof(GeneralAccidentDto)))
        //        {
        //            GeneralAccidentDto G = (GeneralAccidentDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapGeneralAccidentPolicySection(G);
        //        }
        //        else if (section.GetType().Equals(typeof(MarineCargoDto)))
        //        {
        //            MarineCargoDto M = (MarineCargoDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapMarineCargoPolicySection(M);
        //        }
        //        else if (section.GetType().Equals(typeof(MarineHullDto)))
        //        {
        //            MarineHullDto H = (MarineHullDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapMarineHullPolicySection(H);
        //        }
        //        else if (section.GetType().Equals(typeof(MotorDto)))
        //        {
        //            MotorDto M = (MotorDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapMotorPolicySection(M);
        //        }
        //        else if (section.GetType().Equals(typeof(OilGasDto)))
        //        {
        //            OilGasDto O = (OilGasDto)section;
        //            ptd.PolicyNo = policy.PolicyNo;
        //            //ptd.DetailID = policyDetail.DetailID;
        //            ptd.Tag = "NEW";
        //            ptd = ptd.MapOilGasPolicySection(O);
        //        }
        //        _db.PolicyTempDetails.Add(ptd);
        //    }

        //    PolicyDetailUpdate(policyDetail.PolicyNo, TotalSumInsured, TotalGrossPremium);

        //    var newNote = Extensions.MapDNNoteForPolicy(policy, policyDetail, TotalSumInsured, TotalGrossPremium);

        //    _db.DNCNNotes.Add(newNote);
        //    _db.SaveChanges();

        //    contextTransaction.Commit();
        //    return policy;
        //}

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


        public SubRisk SubRiskSelectThis(string subRiskID)
        {
            if (string.IsNullOrWhiteSpace(subRiskID))
                throw new ArgumentNullException("SubRisk ID cannot be empty ", nameof(subRiskID));

            var subRisk = _db.SubRisks.Where(O => O.SubRiskID == subRiskID).SingleOrDefault();

            if (subRisk != null)
                return subRisk;

            throw new KeyNotFoundException("SubRisk ID does not exist");
        }

        public async Task<List<SubRisk>> SubRisksSelectAsync(string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.SubRisks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                char[] chArray = new char[1] { ' ' };
                foreach (string A in searchText.Split(chArray))
                {
                    query = query.Where(O => O.SubRisk1.Contains(A));
                }
            }
            
            var subRisks = await query.OrderBy(o => o.SubRisk1).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            return subRisks;
        }

        private void ProductUpdateIsActive(string SubRiskID, long? SerialNo)
        {
            if (string.IsNullOrWhiteSpace(SubRiskID))
                throw new ArgumentNullException("SubRisk ID cannot be empty ", nameof(SubRiskID));

            long? nullable = SerialNo;
            long num = 0;
            if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                throw new ArgumentException("Serial No cannot be equal or less than 0 ", nameof(SerialNo));

            //get product
            SubRisk subRisk = SubRiskSelectThis(SubRiskID);
            if (subRisk == null)
                throw new KeyNotFoundException("SubRisk ID does not exist");

            subRisk.Active = (byte?)SerialNo;
            _db.SaveChanges();
        }
    }
}
