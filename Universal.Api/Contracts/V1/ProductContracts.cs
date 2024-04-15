namespace GibsLifesMicroWebApi.Contracts.V1
{
    public class ProductResult
    {
        public ProductResult(Models.SubRisks product)
        {
            RiskID = product.RiskID;
            ProductID = product.SubRiskID;
            ProductName = product.SubRisk;
            Description = product.Description;

            //MidRiskID = product.MidClassID;
            //MidRisk = product.MidClass.MidClassName;
        }

        public string RiskID { get; set; }

        public string ProductID { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }
    }
}
