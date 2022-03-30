using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Universal.Api.Contracts.V1
{
    public class PolicyResult
    {
        public PolicyResult()
        {
        }
        public PolicyResult(Models.Policy policy)
        {
            PolicyNo = policy.PolicyNo;
            AgentId = policy.PartyID;
            CustomerId = policy.InsuredClient.InsuredID; //TODO:
            ProductId = policy.SubRiskID;
            //ProductClass = policy.
            StartDate = policy.StartDate.Value;
            EntryDate = policy.TransDate.Value;
            EndDate = policy.EndDate.Value;
            Insured = new CustomerResult(policy);
        }

        public string PolicyNo { get; set; }
        public string AgentId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string ProductClass { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CustomerResult Insured { get; set; } = new CustomerResult();
    }

    public class CreateNew<T> where T : PolicyRequest
    {
        public string AgentId { get; set; }
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<T> PolicyDetails { get; set; }
        public List<Document> Documents { get; set; }
    }

    public class Document
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
    }

}
