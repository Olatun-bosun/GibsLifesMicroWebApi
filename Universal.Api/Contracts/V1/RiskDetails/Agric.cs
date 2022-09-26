using System;
using System.ComponentModel.DataAnnotations;
using Universal.Api.Models;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsAgric : RiskDetail
    {
        //public PolicyAsAgric()
        //{

        //}
        public PolicyAsAgric(PolicyDetail pd) : base(pd)
        {
        }

        public string Data1 { get; set; }
        public string Data2 { get; set; }

        public override void FromPolicyDetail(PolicyDetail pd)
        {
            throw new System.NotImplementedException();
        }

        public override Models.PolicyDetail ToPolicyDetail()
        {
            return new Models.PolicyDetail
            {
                Field1 = Data1,
            };
        }
    }
}
