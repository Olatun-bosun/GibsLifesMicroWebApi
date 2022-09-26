using System;
using System.Collections.Generic;
using Universal.Api.Models;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsFire : RiskDetail
    {
        //public PolicyAsFire()
        //{

        //}
        public PolicyAsFire(PolicyDetail pd) : base(pd)
        {
        }

        public int SectionID { get; set; }// section ID is serial number 1, 2, auto generated
        public string Section { get; set; }// 'Enum :- List all product for selection e.g fire, burglary
        public string RiskSMIID { get; set; }//
        public double FireRate { get; set; }
        public double PerilsRate { get; set; }


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
            throw new NotImplementedException();
        }

        //public string MaterialWall { get; set; }
        //public string MaterialRoof { get; set; }
        //public string WindowsBuglary { get; set; }


        public override PolicyDetail ToPolicyDetail()
        {
            //this.txtEndorseNo.Text = "AUTO",
            //this.txtCertOrDocNo.Text = "[AUTO]";
            var EndorseNo = "AUTO";
            var CertOrDocNo = "[AUTO]";
            return new PolicyDetail
            {
                PolicyNo = this.txtPolicyNo.Text,

                // create a new instance
                string EndorseType = Request.QueryString("type") + "",
                EndorsementNo = EndorseNo,


                BizOption = "BROKERS/AGENT/DIRECT",
                DNCNNo = this.dncnno1.SelectedValue, // not known yet

                CertOrDocNo = "[AUTO]",

                EntryDate = (DateTime)this.txtEntryDate.Text,
                SubmittedBy = mod_main.getLoggedOnUsername(),
                SubmittedOn = DateTime.Now,

                InsuredName = this.txtInsured.Text,
                StartDate = (DateTime)this.txtStartDate.Text,
                EndDate = (DateTime)this.txtExpiryDate.Text,
                ExRate = Convert.ToDouble(this.txtExchRate.Text),
                ExCurrency = this.cdbCurrency.SelectedValue,
                PremiumRate = Convert.ToDecimal(this.TxtPremRate.Text),
                ProportionRate = this.txtBusProp.Text,
                SumInsured = Convert.ToDouble(this.txtNetSumInsured.Text),
                TotalRiskValue = SectionSumInsured,
                GrossPremium = Convert.ToDecimal(this.txtBasicPremium.Text),
                SumInsuredFrgn = Convert.ToDecimal(this.txtFrgSumInsured.Text),
                GrossPremiumFrgn = Convert.ToDecimal(this.txtFrgnPremium.Text),
                ProRataDays = Val(this.txtCoverDays.Text),
                ProRataPremium = Convert.ToDecimal(this.txtProRataPrem.Text),

                NetAmount = Convert.ToDecimal(this.txtTotalNetPremium.Text), // Val(Me.txtNetPremium.Text)
                Deleted = 0,
                Active = 1,
                InsuredName = this.txtInsured.Text,

                // add other specific details
                Field29 = this.txtInsured.Text,
                Field26 = this.txtContent.Text,
                Field17 = PropertyDescription,
                Field25 = Section,
                Field28 = this.txtContent.Text,
                Field27 = this.txtLienClauses.Text,
                Field1 = this.txtFEADisc.Text,
                Field2 = this.txtLTADisc.Text,
                Field30 = this.txtA1Val.Text,
                Field21 = this.cbFireOption.SelectedItem.Text,

                Field34 = this.txtD1.Text,
                Field35 = this.txtD2.Text,
                Field45 = this.cdbReversalTypes.SelectedItem.Value,
                Field48 = EndorsementType,

                Field50 = "PENDING",
            };
        }
    }



    public class NameValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
