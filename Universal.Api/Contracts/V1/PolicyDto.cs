using System;
using System.Collections.Generic;

namespace Universal.Api.Contracts.V1
{
    public class PolicyDto
    {
        public PolicyDto()
        {
        }
        public PolicyDto(Models.Policy policy)
        {
            PolicyNo = policy.PolicyNo;
            EntryDate = policy.TransDate.Value;
            ProductID = policy.SubRiskID;
            AgentID = policy.PartyID;
            BranchID = policy.BranchID;
            StartDate = policy.StartDate.Value;
            EndDate = policy.EndDate.Value;
            //BizChannel = policy.SourceType;
            BizSource = policy.BizSource;
            Insured.LastName = policy.InsSurname;
            Insured.FirstName = policy.InsFirstname;
            Insured.OtherName = policy.InsOthernames;
            Insured.Address = policy.InsAddress;
            Insured.MobilePhone = policy.InsMobilePhone;
            Insured.Telephone = policy.InsLandPhone;
            Insured.Email = policy.InsEmail;
            Insured.Industry = policy.InsOccupation;
            Insured.StateOfOrigin = policy.InsStateID;
            Insured.CustomerId = policy.InsuredID;
        }

        public string PolicyNo { get; set; }
        public string ProductID { get; set; }
        public string AgentID { get; set; }
        public string CustomerID { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BizSource { get; set; }
        public string BranchID { get; set; }
        //public string BizChannel { get; set; }
        public CustomerDto Insured { get; set; } = new CustomerDto();
    }

    public class NewPolicyDto<T> : PolicyDto where T : PolicySectionDto
    {
        public List<T> PolicySection { get; set; }
    }

}
