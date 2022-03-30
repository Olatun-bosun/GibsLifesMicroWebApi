using System;

namespace Universal.Api.Contracts.V1
{
    public class ProductResult
    {
        public ProductResult()
        {
        }

        public ProductResult(Models.SubRisk product)
        {
            ProductId = product.SubRiskID;
            RiskId = product.RiskID;
            ProductName = product.SubRiskName;
            Description = product.Description;
        }

        public string ProductId { get; set; }

        public string RiskId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }
    }
}
