using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Models
{
    public class PolicyTempDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DetailID { get; set; }

        public string PolicyNo { get; set; }

        public string ENDTNum { get; set; }

        public string SectionID { get; set; }

        public string ContentSection { get; set; }

        public string Location { get; set; }

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

        public Decimal? RiskSum { get; set; }

        public Decimal? OtherSum { get; set; }

        public Decimal? SectionPremium { get; set; }

        public DateTime? TStartDate { get; set; }

        public DateTime? TEndDate { get; set; }

        public Decimal? A1 { get; set; }

        public Decimal? A2 { get; set; }

        public Decimal? A3 { get; set; }

        public Decimal? A4 { get; set; }

        public long? A5 { get; set; }

        public Decimal? A6 { get; set; }

        public Decimal? A7 { get; set; }

        public Decimal? A8 { get; set; }

        public Decimal? A9 { get; set; }

        public Decimal? A10 { get; set; }

        public Decimal? A11 { get; set; }

        public Decimal? A12 { get; set; }

        public Decimal? A13 { get; set; }

        public Decimal? A14 { get; set; }

        public Decimal? A15 { get; set; }

        public Decimal? A16 { get; set; }

        public double? A17 { get; set; }

        public Decimal? A18 { get; set; }

        public Decimal? A19 { get; set; }

        public Decimal? A20 { get; set; }

        public Decimal? A21 { get; set; }

        public Decimal? A22 { get; set; }

        public Decimal? A23 { get; set; }

        public Decimal? A24 { get; set; }

        public Decimal? A25 { get; set; }

        public Decimal? A26 { get; set; }

        public Decimal? A27 { get; set; }

        public Decimal? A28 { get; set; }

        public Decimal? A29 { get; set; }

        public Decimal? A30 { get; set; }

        public Decimal? A31 { get; set; }

        public Decimal? A32 { get; set; }

        public Decimal? A33 { get; set; }

        public Decimal? A34 { get; set; }

        public Decimal? A35 { get; set; }

        public Decimal? A36 { get; set; }

        public Decimal? A37 { get; set; }

        public Decimal? A38 { get; set; }

        public Decimal? A39 { get; set; }

        public Decimal? A40 { get; set; }

        public Decimal? A41 { get; set; }

        public Decimal? A42 { get; set; }

        public Decimal? A43 { get; set; }

        public Decimal? A44 { get; set; }

        public Decimal? A45 { get; set; }

        public Decimal? A46 { get; set; }

        public Decimal? A47 { get; set; }

        public Decimal? A48 { get; set; }

        public Decimal? A49 { get; set; }

        public Decimal? A50 { get; set; }

        public string Tag { get; set; }

        public string TransGUID { get; set; }

        public string SubmitBy { get; set; }

        public DateTime? SubmitOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
