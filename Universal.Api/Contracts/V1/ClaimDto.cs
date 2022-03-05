using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class ClaimDto
    {
        public ClaimDto()
        {
        }

        public ClaimDto(Models.Claim claim)
        {
            PolicyNo = claim.PolicyNo;
            NotifyDate = claim.NotifyDate.Value;
            LossDate = claim.LossDate.Value;
            LossDetails = claim.LossDetails;
            LossType = claim.LossType;
        }

        [Required]
        public string PolicyNo { get; set; }
        public DateTime NotifyDate { get; set; }
        public DateTime LossDate { get; set; }
        [Required]
        public string LossType { get; set; }
        [Required]
        public string LossDetails { get; set; }
    }
}
