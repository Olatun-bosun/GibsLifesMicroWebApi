using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Models
{
    public class DNCNNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DNCNID { get; set; }

        public string DNCNNo { get; set; }

        public string refDNCNNo { get; set; }

        public string ReceiptNo { get; set; }

        public string PolicyNo { get; set; }

        public string CoPolicyNo { get; set; }

        public string BranchID { get; set; }

        public string CompanyID { get; set; }

        public string BizSource { get; set; }

        public string BizOption { get; set; }

        public string NoteType { get; set; }

        public DateTime? BillingDate { get; set; }

        public string SubRiskID { get; set; }

        public string SubRisk { get; set; }

        public string PartyID { get; set; }

        public string Party { get; set; }

        public Decimal? PartyRate { get; set; }

        public string InsuredID { get; set; }

        public string InsuredName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string MktStaffID { get; set; }

        public string MktStaff { get; set; }

        public Decimal? SumInsured { get; set; }

        public Decimal? GrossPremium { get; set; }

        public Decimal? Commission { get; set; }

        public double? PropRate { get; set; }

        public long? ProRataDays { get; set; }

        public Decimal? ProRataPremium { get; set; }

        public double? VatRate { get; set; }

        public Decimal? VatAmount { get; set; }

        public Decimal? NetAmount { get; set; }

        public string Narration { get; set; }

        public double? ExRate { get; set; }

        public string ExCurrency { get; set; }

        public Decimal? SumInsuredFrgn { get; set; }

        public Decimal? GrossPremiumFrgn { get; set; }

        public byte? Approval { get; set; }

        public byte? HasTreaty { get; set; }

        public string Remarks { get; set; }

        public string Transguid { get; set; }

        public string SourceType { get; set; }

        public string RIClass { get; set; }

        public Decimal? TopMostValue { get; set; }

        public Decimal? PMLValue { get; set; }

        public string PaymentType { get; set; }

        public string ChequeNo { get; set; }

        public string Field1 { get; set; }

        public string Field2 { get; set; }

        public string Field3 { get; set; }

        public string Field4 { get; set; }

        public string Field5 { get; set; }

        public string Field6 { get; set; }

        public string Field7 { get; set; }

        public string Field8 { get; set; }

        public string Field9 { get; set; }

        public string Field10 { get; set; }

        public byte? Deleted { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        public byte? Active { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public Decimal? TotalRiskValue { get; set; }

        public Decimal? TotalPremium { get; set; }

        public string LeaderID { get; set; }

        public string Leader { get; set; }

        public double? RetProp { get; set; }

        public Decimal? RetValue { get; set; }

        public Decimal? RetPremium { get; set; }

        public DateTime? DBDate { get; set; }

        public DateTime? CRRefDate { get; set; }

        public string MktUnitID { get; set; }

        public string mktUnit { get; set; }

        public string CoverType { get; set; }

        public Decimal? A1 { get; set; }

        public Decimal? A2 { get; set; }

        public Decimal? A3 { get; set; }

        public Decimal? A4 { get; set; }

        public Decimal? A5 { get; set; }

        public Decimal? A6 { get; set; }

        public Decimal? A7 { get; set; }

        public Decimal? A8 { get; set; }

        public Decimal? A9 { get; set; }

        public Decimal? A10 { get; set; }

        public DateTime? ExtraDate1 { get; set; }

        public DateTime? ExtraDate2 { get; set; }
    }
}
