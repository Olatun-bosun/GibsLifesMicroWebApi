using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibsLifesMicroWebApi.Models
{
    public class PolicyDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DetailID { get; set; }
        //[ForeignKey("PolicyNo")]
        //public string PolicyNo { get; set; }

        public string CoPolicyNo { get; set; }

        public DateTime? EntryDate { get; set; }

        public string EndorsementNo { get; set; }

        public string BizOption { get; set; }

        public string DNCNNo { get; set; }

        public string CertOrDocNo { get; set; }

        public string InsuredName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? ExRate { get; set; }

        public string ExCurrency { get; set; }

        public double? PremiumRate { get; set; }

        public double? ProportionRate { get; set; }

        public decimal? SumInsured { get; set; }

        public decimal? GrossPremium { get; set; }

        public decimal? SumInsuredFrgn { get; set; }

        public decimal? GrossPremiumFrgn { get; set; }

        public int? ProRataDays { get; set; }

        public decimal? ProRataPremium { get; set; }

        public decimal? NetAmount { get; set; }

        public byte? Deleted { get; set; }

        public byte? Active { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

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

        public string Field11 { get; set; }

        public string Field12 { get; set; }

        public string Field13 { get; set; }

        public string Field14 { get; set; }

        public string Field15 { get; set; }

        public string Field16 { get; set; }

        public string Field17 { get; set; }

        public string Field18 { get; set; }

        public string Field19 { get; set; }

        public string Field20 { get; set; }

        public string Field21 { get; set; }

        public string Field22 { get; set; }

        public string Field23 { get; set; }

        public string Field24 { get; set; }

        public string Field25 { get; set; }

        public string Field26 { get; set; }

        public string Field27 { get; set; }

        public string Field28 { get; set; }

        public string Field29 { get; set; }

        public string Field30 { get; set; }

        public string Field31 { get; set; }

        public string Field32 { get; set; }

        public string Field33 { get; set; }

        public string Field34 { get; set; }

        public string Field35 { get; set; }

        public string Field36 { get; set; }

        public string Field37 { get; set; }

        public string Field38 { get; set; }

        public string Field39 { get; set; }

        public string Field40 { get; set; }

        public string Field41 { get; set; }

        public string Field42 { get; set; }

        public string Field43 { get; set; }

        public string Field44 { get; set; }

        public string Field45 { get; set; }

        public string Field46 { get; set; }

        public string Field47 { get; set; }

        public string Field48 { get; set; }

        public string Field49 { get; set; }

        public string Field50 { get; set; }

        public decimal? TotalRiskValue { get; set; }

        public virtual Policy Policy { get; set; }

    }
}
