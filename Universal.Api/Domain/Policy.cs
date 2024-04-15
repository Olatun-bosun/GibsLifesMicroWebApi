using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibsLifesMicroWebApi.Models
{
    public class Policy
    {
        [Key]
        public string PolicyNo { get; set; }
        [ForeignKey("InsuredID")]
        public virtual InsuredClient InsuredClient { get; set; }
        //public long PolicyID { get; set; }
        public DateTime? TransDate { get; set; }

        public string CoPolicyNo { get; set; }

        public string BranchID { get; set; }

        public string Branch { get; set; }

        public string BizSource { get; set; }

        public string SubRiskID { get; set; }

        [Column("SubRisk")]
        public string SubRiskName { get; set; }

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

        public byte? IsProposal { get; set; }

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



        //public string Z_NAICOM_UID { get; set; }
        //public string Z_NAICOM_STATUS { get; set; }
        //public DateTime? Z_NAICOM_SENT_ON { get; set; }
        //public string Z_NAICOM_ERROR { get; set; }
        //public string Z_NAICOM_JSON { get; set; }


        public virtual List<PolicyDetail> PolicyDetails { get; set; }

        public string InsFullname
        {
            get
            {
                return $"{InsSurname} {InsFirstname} {InsOthernames}".Trim();
            }
        }


       
        [NotMapped]
        public DNCNNote DebitNote { get; set; }
        [NotMapped]
        public SubRisk SubRisk { get; set; }
    }
}
