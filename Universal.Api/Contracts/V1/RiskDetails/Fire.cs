using System.Text.Json;
using System.Collections.Generic;
using Universal.Api.Models;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsFire : RiskDetail
    {
        //public int SectionID { get; set; }// section ID is serial number 1, 2, auto generated
        //public string Section { get; set; }// 'Enum :- List all product for selection e.g fire, burglary
        //public string RiskSMIID { get; set; }//
        //public double FireRate { get; set; }
        //public double PerilsRate { get; set; }
        //-----------------------------------------------------------------//

        public string CoverType { get; set; }
        public string PropertyType { get; set; }
        public string PropertyAddress { get; set; }
        public string PropertyDescription { get; set; }//
        public string PropertyLengthOfStay { get; set; } //how long in property
        public string PropertyOccupants { get; set; }//no of staff
        public string TypeOfGoods { get; set; }
        public decimal EstimatedValue { get; set; }

        // generic fields
        public List<NameValue> Values { get; set; }

        public override void FromPolicyDetail(PolicyDetail pd)
        {
            CoverType = pd.Field11;
            PropertyType = pd.Field12;
            PropertyAddress = pd.Field13;
            PropertyDescription = pd.Field14;
            PropertyLengthOfStay = pd.Field15;
            PropertyOccupants = pd.Field16;
            TypeOfGoods = pd.Field18;

            Values = JsonSerializer.Deserialize<List<NameValue>>(pd.Field17);
        }

        public override PolicyDetail ToPolicyDetail()
        {
            //Field1  -  8  32   chars
            //Field9  - 16  64   chars
            //Field17 - 50  1024 chars

            return new PolicyDetail
            {
                Field11 = CoverType,
                Field12 = PropertyType,
                Field13 = PropertyAddress,
                Field14 = PropertyDescription,
                Field15 = PropertyLengthOfStay,
                Field16 = PropertyOccupants,
                Field18 = TypeOfGoods,

                Field17 = JsonSerializer.Serialize(Values),
            };
        }
    }
}
