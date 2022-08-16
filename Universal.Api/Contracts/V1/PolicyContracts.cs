using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CreateNew<T> where T : RiskDetail
    {
        [Required]
        public string AgentID { get; set; }
        //[Required]
        public string CustomerID { get; set; }
        [Required]
        public string ProductID { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public string PaymentProviderID { get; set; }

        public string PaymentReferenceID { get; set; }
        //[Required]
        public CreateNewCustomerRequest Insured { get; set; }

        [Required]
        public List<T> PolicySections { get; set; }
    }

    public abstract class RiskDetail
    {
        [Required]
        public decimal SectionSumInsured { get; set; }
        [Required]
        public decimal SectionPremium { get; set; }

        public abstract Models.PolicyDetail MapToPolicyDetail();
    }

    public class PolicyResult
    {
        public PolicyResult(Models.Policy policy)
        {
            PolicyNo = policy.PolicyNo;
            AgentID = policy.PartyID;
            CustomerID = policy.InsuredClient.InsuredID; //TODO:
            ProductID = policy.SubRiskID;
            //ProductClass = policy.
            StartDate = policy.StartDate.Value;
            EntryDate = policy.TransDate.Value;
            EndDate = policy.EndDate.Value;

            //if (policy.Insured != null)
            Insured = new CustomerResult(policy);
        }

        public string PolicyNo { get; set; }
        public string AgentID { get; set; }
        public string CustomerID { get; set; }
        public string ProductID { get; set; }
        public string ProductClass { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CustomerResult Insured { get; set; }
    }

}
