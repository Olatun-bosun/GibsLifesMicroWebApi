using System;

namespace Universal.Api.Contracts.V1
{
    public class ProductDto
    {
        public ProductDto()
        {
        }

        public ProductDto(Models.SubRisk product)
        {
            ID = product.SubRiskID;
            RiskID = product.RiskID;
            //MidRiskID = product.MidRiskID;
            //MidRisk = product.MidRisk;
            SubRisk = product.SubRisk1;
            Description = product.Description;
            Deleted = Convert.ToBoolean(product.Deleted);
            Active = Convert.ToBoolean(product.Active);
        }

        public string ID { get; set; }

        public string RiskID { get; set; }

        //public string MidRiskID { get; set; }

        //public string MidRisk { get; set; }

        public string SubRisk { get; set; }

        public string Description { get; set; }

        public bool Deleted { get; set; }

        public bool Active { get; set; }
    }
}
