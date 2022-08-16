using System.Collections.Generic;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsAccident : RiskDetail
    {
        public string CoverType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ScopeofCover { get; set; }
        public string LienClauses { get; set; }

        //public List<NameValue> Attributes { get; set; }
        public List<PersonRequest> Members { get; set; }

        public override Models.PolicyDetail MapToPolicyDetail()
        {
            return new Models.PolicyDetail
            {

            };
        }
    }
}
