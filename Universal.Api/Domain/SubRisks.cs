using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GibsLifesMicroWebApi.Models
{
    public class SubRisks
    {
        [Key]
        public string SubRiskID { get; set; }

        public string RiskID { get; set; }
        [Column("SubRisk")]
        public string SubRisk { get; set; }
        public string Description { get; set; }

        public byte? Deleted { get; set; }

        public byte? Active { get; set; }


        public string SubmittedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
