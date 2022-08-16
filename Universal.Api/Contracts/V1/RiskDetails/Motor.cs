using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsMotor : RiskDetail
    {
        public enum VehicleTypeEnum
        {
            VAN,
            BUS,
            JEEP,
            MINI_BUS,
            MINI_TRUCK, //PICKUP
            MID_TRUCK,
            TRUNK_TRUCK,//GEN CARTAGE
            MOTORCYCLE,
            SALOON,
            TRICYCLE,
            TRACTOR,
            OTHER
        }

        public enum VehicleUsageEnum
        {
            PRIVATE,
            COMMERCIAL
        }

        public enum VehicleCoverEnum
        {
            COMPREHENSIVE,
            THIRD_PARTY_ONLY,
        }

        [Required]
        public string VehicleRegNo { get; set; }
        [Required]
        public VehicleTypeEnum? VehicleTypeID { get; set; }
        [Required]
        public string VehicleUser { get; set; }
        [Required]
        public string EngineNumber { get; set; }
        [Required]
        public string ChasisNumber { get; set; }
        [Required]
        public VehicleUsageEnum? VehicleUsage { get; set; }
        [Required]
        public int NumberOfSeats { get; set; }
        [Required]
        public string StateOfIssue { get; set; }//ENUM
        [Required]
        public string VehicleMake { get; set; }
        [Required]
        public string VehicleModel { get; set; }
        [Required]
        public int ManufactureYear { get; set; }
        [Required]
        public string VehicleColour { get; set; }

        public string EngineCapacityHP { get; set; }
        [Required]
        public VehicleCoverEnum? CoverType { get; set; }

        public override Models.PolicyDetail MapToPolicyDetail()
        {
            return new Models.PolicyDetail
            {

            };
        }
    }
}
