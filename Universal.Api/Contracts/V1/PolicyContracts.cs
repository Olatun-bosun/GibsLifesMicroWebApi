using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Universal.Api.Models;

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

        //public RiskDetail()
        //{

        //}

        public RiskDetail(PolicyDetail pd)
        {
            FromPolicyDetail(pd);
        }

        public abstract void FromPolicyDetail(PolicyDetail pd);

        public abstract PolicyDetail ToPolicyDetail();
    }

    public class PolicyResult
    {
        public PolicyResult(Policy policy)
        {
            PolicyNo = policy.PolicyNo;
            AgentID = policy.PartyID;
            CustomerID = policy.InsuredClient.InsuredID;
            ProductID = policy.SubRiskID;
            //ProductClass = policy.
            EntryDate = policy.TransDate.Value;
            StartDate = policy.StartDate.Value;
            EndDate = policy.EndDate.Value;

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

    public class PolicyResult<T> : PolicyResult where T : RiskDetail
    {
        public PolicyResult(Policy policy) : base(policy)
        {
            PolicySections = new List<T>();

            foreach (var pd in policy.PolicyDetails)
            {
                var t = Activator.CreateInstance(typeof(T), new object[] { pd }) as T;
                PolicySections.Add(t);
            }
        }

        public List<T> PolicySections { get; set; }
    }
}
