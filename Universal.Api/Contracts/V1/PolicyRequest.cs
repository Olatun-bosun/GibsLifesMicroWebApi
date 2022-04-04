using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class CreateNew<T> where T : PolicyRequest
    {
        [Required]
        public string AgentId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public decimal TotalSumInsured { get; set; }
        [Required]
        public decimal TotalGrossPremium { get; set; }

        [Required]
        public List<T> PolicyDetails { get; set; }
        public List<Document> Documents { get; set; }
    }

    public class Document
    {
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string Description { get; set; }
    }


    public abstract class PolicyRequest
    {
        //[Required]
        //public decimal SumInsured { get; set; }
        //[Required]
        //public decimal GrossPremium { get; set; }

        public abstract Models.PolicyDetail MapToPolicyDetail();
    }

    public class CreateNewPolicyAsAviation : CreateNew<PolicyAsAviation> { }
    public class CreateNewPolicyAsBond : CreateNew<PolicyAsBond> { }
    public class CreateNewPolicyAsEngineering : CreateNew<PolicyAsEngineering> { }
    public class CreateNewPolicyAsFire : CreateNew<PolicyAsFire> { }
    public class CreateNewPolicyAsGeneralAccident : CreateNew<PolicyAsGeneralAccident> { }
    public class CreateNewPolicyAsMarineCargo : CreateNew<PolicyAsMarineCargo> { }
    public class CreateNewPolicyAsMarineHull : CreateNew<PolicyAsMarineHull> { }
    public class CreateNewPolicyAsMotor : CreateNew<PolicyAsMotor> { }
    public class CreateNewPolicyAsOilGas : CreateNew<PolicyAsOilGas> { }
}
