﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class AgentResult
    {
        public AgentResult()
        {
        }

        public AgentResult(Models.Party party)
        {
            AgentId = party.ApiId;
            AgentName = party.Party1;
            Address = party.Address;
            PhoneLine2 = party.LandPhone;
            PhoneLine1 = party.mobilePhone;
            Email = party.Email;
            //CommissionRate = (decimal)party.ComRate;
            //CreditLimit = (decimal)party.CreditLimit;
            InsuranceContact = party.InsContact;
            //FinancialContact = party.FinContact;
            Remarks = party.Remarks;
        }

        [Required]
        public string AgentId { get; set; }
        [Required]
        public string AgentName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneLine1 { get; set; }
        public string PhoneLine2 { get; set; }
        [Required]
        public string Email { get; set; }
        //public decimal CommissionRate { get; set; }
        //public decimal CreditLimit { get; set; }
        //public string RPCNumber { get; set; }
        public string InsuranceContact { get; set; }
        //public string FinancialContact { get; set; }
        public string Remarks { get; set; }
    }

    public class CreateNewAgentRequest
    {
        [Required]
        public string AgentName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneLine1 { get; set; }
        public string PhoneLine2 { get; set; }
        [Required]
        public string Email { get; set; }
        public string InsuranceContact { get; set; }
        public string Remarks { get; set; }
    }
}
