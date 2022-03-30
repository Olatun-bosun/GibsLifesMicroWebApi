using Microsoft.AspNetCore.Http;
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
            ProductId = policy.SubRiskID;
            AgentId = policy.PartyID;
            StartDate = policy.StartDate.Value;
            EndDate = policy.EndDate.Value;
            //BizChannel = policy.SourceType;
            Insured = new CustomerDto(policy);
            
        }

        public string PolicyNo { get; set; }
        public string ProductId { get; set; }
        public string AgentId { get; set; }
        public string CustomerId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public string SourceId { get; set; } //BizSource TODO:
        //public string BranchId { get; set; }
        public CustomerDto Insured { get; set; } = new CustomerDto();
    }

    public class NewPolicyDto<T> : PolicyDto where T : PolicyDetailDto
    {
        public List<T> PolicyDetails { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }

    public class DocumentDto
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

}
