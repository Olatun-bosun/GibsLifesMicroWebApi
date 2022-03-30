using System;

namespace Universal.Api.Contracts.V1
{
    public class PolicyDetailDto
    {
        public DateTime EntryDate { get; set; }
        public decimal SumInsured { get; set; }
        public decimal GrossPremium { get; set; }

        public string RiskID { get; set; }

    }

    public class AviationDto : PolicyDetailDto
    {
        public string Interest { get; set; }
        public string AircraftID { get; set; }
        public string AircraftMake { get; set; }
        public string AircraftModel { get; set; }
        public string RegMarks { get; set; }
        public string SpareEquipments { get; set; }
        public int MaximumCrew { get; set; }
        public int PassengerSeating { get; set; }
        public int LicensedPassengers { get; set; }
        public string YearOfMfg { get; set; }
        public string CrewPersonalAccidents { get; set; }
        public string EngineType { get; set; }
        public string Usage { get; set; }
        public string GeographicalArea { get; set; }
        public int DeclaredPassengers { get; set; }
        public int NumberOfEngines { get; set; }
        public int NumberOfPilots { get; set; }
        public string Deductibles { get; set; }
        public bool NightFlight { get; set; }
        public decimal AircraftSumInsured { get; set; }
        public decimal AircraftGrossPremium { get; set; }
        public decimal AggregateSumInsured { get; set; }
        public double ProfitCommDiscount { get; set; }
        public double NCDFLevyDiscount { get; set; }
        public double NAICOMLevyDiscount { get; set; }
        public decimal AggregateGrossPremium { get; set; }
        public decimal NetAggregatePremium { get; set; }
    }

    public class BondDto : PolicyDetailDto
    {
        public int OurShare { get; set; }
        public string PrincipalName { get; set; }
        public string ContractorName { get; set; }
        public string BorrowerName { get; set; }
        public string DirectorName { get; set; }
        public decimal TotalBondValue { get; set; }
        public string NatureOfContract { get; set; }
        public int BondDuration { get; set; }
        public string ContractFrom { get; set; }
        public string ContractTo { get; set; }
        public string PrimaryGuarantor { get; set; }
        public string Remarks { get; set; }
        public string AddressOfPrincipal { get; set; }
        public string AddressOfContractor { get; set; }
        public string AddressOfBorrower { get; set; }
        public string AwardDate { get; set; }
        public string ContractWork { get; set; }
        public string BondFrom { get; set; }
        public string BondTo { get; set; }
        public decimal TotalContractValue { get; set; }
        public double PercOfContractValue { get; set; }
        public string BondIssueDate { get; set; }
        public string RiskSMIID { get; set; }
        public double Rate { get; set; }
        public decimal TotalSumInsured { get; set; }
        public string Description { get; set; }
        public decimal OurShareSumInsured { get; set; }
        public string OurSharePremium { get; set; }
    }

    public class EngineeringDto : PolicyDetailDto
    {
        public int OurShare { get; set; }
        public string ContractorName { get; set; }
        public string ScopeOfContract { get; set; }
        public string ProjectConsultant { get; set; }
        public string PrincipalName { get; set; }
        public decimal MajorExcess { get; set; }
        public string RiskDescription { get; set; }
        public string ContractAwardDate { get; set; }
        public decimal TPPDExcess { get; set; }
        public string AnyOneYear { get; set; }
        public bool PlantUnderMaintenance { get; set; }
        public string RiskClassification { get; set; }
        public string PropertyDescription { get; set; }
        public string EstimatedContractTerms { get; set; }
        public string PrincipalAddress { get; set; }
        public string RiskAddress { get; set; }
        public string MinorExcess { get; set; }
        public string IndustryId { get; set; }
        public string Remarks { get; set; }
        public string AnyOneLimit { get; set; }
        public bool SurveyRequired { get; set; }
        public string MaintenanceFrom { get; set; }
        public string MaintenanceTo { get; set; }
    }

    public class FireDto : PolicyDetailDto
    {
        //public decimal GrossPremium { get; set; } // already inherited.
        public int Multiplier { get; set; }
        public double WarLoading { get; set; }
        public string RiskSMIId { get; set; }
        public double Rate { get; set; }
        public decimal TotalSumInsured { get; set; }
        public string Description { get; set; }
        public decimal OurShareSumInsured { get; set; }
        public decimal OurSharePremium { get; set; }
    }

    public class GeneralAccidentDto : PolicyDetailDto
    {
        public int OurShare { get; set; }
        public string ContractorName { get; set; }
        public string Model { get; set; }
        public string ContractAwardDate { get; set; }
        public decimal MajorExcess { get; set; }
        public string RiskDescription { get; set; }
        public bool PlantUnderMaintenance { get; set; }
        public string LienClauses { get; set; }
        public string Remarks { get; set; }
        public bool SurveyRequired { get; set; }
        public string IndustryId { get; set; }
        public string MfgDetails { get; set; }
        public string RiskSMIId { get; set; }
        public double Rate { get; set; }
        public decimal TotalSumInsured { get; set; }
        public string Description { get; set; }
        public decimal OurShareSumInsured { get; set; }
        public decimal OurSharePremium { get; set; }
    }

    public class MarineCargoDto : PolicyDetailDto
    {
        public string VesselType { get; set; }
        public string FromCountryId { get; set; }
        public string LienClause { get; set; }
        public string VesselOperation { get; set; }
        public string CertificateNo { get; set; }
        public string ConveyanceId { get; set; }
        public string TINNumber { get; set; }
        public string SubjectMatter { get; set; }
        public string ToCountryId { get; set; }
        public string PackageTypeId { get; set; }
        public string ProformaInvoiceNo { get; set; }
        public string MarksAndNumbers { get; set; }
        public double PremiumRate { get; set; }
        public string BasisOfValuation { get; set; }
        public double OtherDiscountRate { get; set; }
    }

    public class MarineHullDto : PolicyDetailDto
    {
        public int OurShare { get; set; }
        public string VesselStateId { get; set; }
        public string NameOfVessel { get; set; }
        public string VesselTone { get; set; }
        public string VesselOperation { get; set; }
        public string YearBuilt { get; set; }
        public string TerritorialLimits { get; set; }
        public int Length { get; set; }
        public int Depth { get; set; }
        public string EngineType { get; set; }
        public string Builder { get; set; }
        public string VesselClass { get; set; }
        public string Construction { get; set; }
        public string Excess { get; set; }
        public string CountryOfMfg { get; set; }
        public int Beam { get; set; }
        public int Draft { get; set; }
        public double WarLoading { get; set; }
        public string RiskSMIId { get; set; }
        public double Rate { get; set; }
        public decimal TotalSumInsured { get; set; }
        public string Description { get; set; }
        public decimal OurShareSumInsured { get; set; }
        public decimal OurSharePremium { get; set; }
    }

    public class MotorDto : PolicyDetailDto
    {
        public string CertificateTypeId { get; set; }
        public string DeclarationNo { get; set; }
        public string VehicleRegNo { get; set; }
        public string VehicleTypeId { get; set; }
        public string VehicleUser { get; set; }
        public string EngineNumber { get; set; }
        public string ChasisNumber { get; set; }
        public string VehicleUsageId { get; set; }
        public int NumberOfSeats { get; set; }
        public string StateOfIssueId { get; set; }
        public string VehicleMakeId { get; set; }
        public string VehicleModelId { get; set; }
        public string MfgYear { get; set; }
        public string VehicleColour { get; set; }
        public string EngineCapacityHP { get; set; }
        public string CoverTypeId { get; set; }
        public string WaxCode { get; set; }
        public decimal VehicleValue { get; set; }
        public decimal BasicPremium { get; set; }
        public decimal ProRataPremium { get; set; }
        public double PremiumRate { get; set; }
        public int CoverDays { get; set; }
        public double TPFPRate { get; set; }
        public decimal TPPDValue { get; set; }
        public double SRCCValue { get; set; }
        public double ExcessBuyBack { get; set; }
        public decimal PCSSValue { get; set; }
        public decimal PremiumDue { get; set; }
        public double PluralityDiscount { get; set; }
        public double NoClaimDiscount { get; set; }
        public int BusinessProportion { get; set; }
    }

    public class OilGasDto : PolicyDetailDto
    {
        public int OurShare { get; set; }
        public string OrderHereon { get; set; }
        public string ProjectPeriodFrom { get; set; }
        public string ProjectPeriodTo { get; set; }
        public string MaintenanceFrom { get; set; }
        public string MaintenanceTo { get; set; }
        public decimal InterestCover { get; set; }
        public decimal Deductibles { get; set; }
        public string Conditions { get; set; }
        public decimal DeductionsForRI { get; set; }
        public string Remarks { get; set; }
        public string InsuredSubscription { get; set; }
        public decimal ChoiceOfLaw { get; set; }
        public string Situation { get; set; }
        public decimal DeductionFromPremium { get; set; }
    }
}
