using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class AgentDto
    {
        public AgentDto()
        {
        }

        public AgentDto(Models.Party agent)
        {
            AgentID = agent.ApiId;
            AgentName = agent.Party1;
            Address = agent.Address;
            Telephone = agent.LandPhone;
            MobilePhone = agent.mobilePhone;
            Email = agent.Email;
            CommRate = (decimal)agent.ComRate;
            CreditLimit = (decimal)agent.CreditLimit;
            InsContact = agent.InsContact;
            FinContact = agent.FinContact;
            Remarks = agent.Remarks;
            Status = agent.ApiStatus;
        }

        [Required]
        public string AgentID { get; set; }
        [Required]
        public string AgentName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string MobilePhone { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Email { get; set; }
        public decimal CommRate { get; set; }
        public decimal CreditLimit { get; set; }
        public string RPCNumber { get; set; }
        public string Website { get; set; }
        public string InsContact { get; set; }
        public string FinContact { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
