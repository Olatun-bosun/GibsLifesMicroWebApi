using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibsLifesMicroWebApi.Models
{
    public class SubRisk
    {
        [Key]
        public string SubRiskID { get; set; }

        public string RiskID { get; set; }
        [Column("SubRisk")]
        public string SubRiskName { get; set; }

        public string Description { get; set; }

        public byte? Deleted { get; set; }

        public byte? Active { get; set; }

        public string InsuranceTypeID { get; set; }

        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
