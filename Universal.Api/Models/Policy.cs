using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universal.Api.Models
{
    public class Policy
    {
        [Key]
        public string PolicyNo { get; set; }
        [ForeignKey("InsuredID")]
        public virtual InsuredClient InsuredClient { get; set; }
        public long PolicyID { get; set; }
        public DateTime? TransDate { get; set; }

        public string CoPolicyNo { get; set; }

        public string BranchID { get; set; }

        public string Branch { get; set; }

        public string BizSource { get; set; }

        public string SubRiskID { get; set; }

        public string SubRisk { get; set; }

        public string PartyID { get; set; }

        public string Party { get; set; }

        public string MktStaffID { get; set; }

        public string MktStaff { get; set; }
        
        public string InsuredID { get; set; }

        public string InsSurname { get; set; }

        public string InsFirstname { get; set; }

        public string InsOthernames { get; set; }

        public string InsAddress { get; set; }

        public string InsStateID { get; set; }

        public string InsMobilePhone { get; set; }

        public string InsLandPhone { get; set; }

        public string InsEmail { get; set; }

        public string InsFaxNo { get; set; }

        public string InsOccupation { get; set; }

        public byte? isProposal { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double? ExRate { get; set; }

        public string ExCurrency { get; set; }

        public double? PremiumRate { get; set; }

        public double? ProportionRate { get; set; }

        public Decimal? SumInsured { get; set; }

        public Decimal? GrossPremium { get; set; }

        public Decimal? SumInsuredFrgn { get; set; }

        public Decimal? GrossPremiumFrgn { get; set; }

        public int? ProRataDays { get; set; }

        public Decimal? ProRataPremium { get; set; }

        public string Remarks { get; set; }

        public byte? Deleted { get; set; }

        public byte? Active { get; set; }

        public string TransSTATUS { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string LeadID { get; set; }

        public string Leader { get; set; }

    }
}
