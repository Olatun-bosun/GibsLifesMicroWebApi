using System.Collections.Generic;
using Universal.Api.Models;

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

        public override void FromPolicyDetail(PolicyDetail pd)
        {
            CoverType = pd.Field11;
            Location = pd.Field12;
            Description = pd.Field13;
            ScopeofCover = pd.Field14;
            LienClauses = pd.Field15;
            Members = JsonSerializer.Deserialize<List<PersonRequest>>(pd.Field17);
        }

        public override Models.PolicyDetail ToPolicyDetail()
        {
            return new Models.PolicyDetail
            {

            };
        }
    }
}
