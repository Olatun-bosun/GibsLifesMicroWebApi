using System;
using System.Collections.Generic;
using Universal.Api.Models;

namespace Universal.Api.Contracts.V1.RiskDetails
{
    public class PolicyAsAccident : RiskDetail
    {
        //public PolicyAsAccident()
        //{

        //}
        public PolicyAsAccident(PolicyDetail pd) : base(pd)
        {
        }

        public string CoverType { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ScopeofCover { get; set; }
        public string LienClauses { get; set; }

        //public List<NameValue> Attributes { get; set; }
        public List<PersonRequest> Members { get; set; }

        public override void FromPolicyDetail(PolicyDetail pd)
        {
            throw new NotImplementedException();
        }

        public override PolicyDetail ToPolicyDetail()
        {
            return new PolicyDetail
            {
                
                PolicyNo = this.txtPolicyNo,
                this.txtEndorseNo.Text = "AUTO",
                this.txtCertOrDocNo.Text = "[AUTO]",

                this.txtInsured.Text = this.txtInsured.Text,
                // rec.EndorsementNo = Me.txtEndorseNo.Text
                //string strAutoNumber;

                // create a new instance

                EndorsementNo = "AUTO",
                BizOption = //depends on edit type   NEW/RENEWAL/ADDITIONAL;
                DNCNNo = this.dncnno1.SelectedValue, // not known yet

                CertOrDocNo = "[AUTO]",
                rec.EntryDate = (DateTime)this.txtEntryDate.Text,

                SubmittedBy = mod_main.getLoggedOnUsername(),
                SubmittedOn = DateTime.Now,
                InsuredName = this.txtInsured.Text,
                StartDate = (DateTime)this.txtStartDate.Text,
                EndDate = (DateTime)this.txtExpiryDate.Text,
                ExRate = System.Convert.ToDecimal(this.txtExchRate.Text),
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
                NetAmount = Convert.ToDouble(this.txtTotalNetPremium.Text),  // Val(Me.txtNetPremium.Text)
                Deleted = 0,
                Active = 1,

                // add other specific details
                Field21 = CoverType,

                Field24 = this.cbConveyance.SelectedItem.Value,
                Field29 = this.txtInsured,
                Field26 = this.txtContent,
                Field17 = this.txtSection,
                Field25 = this.txtPptyDesc,
                Field28 = this.txtReinPpty,
                Field27 = this.txtLienClauses.Text,  // Me.cbGeoglimit.SelectedItem.Value


                Field1 = this.txtFEADisc.Text,
                Field2 = this.txtLTADisc.Text,
                Field3 = this.txtstockDisc.Text,
                Field4 = this.txtInitialPremiumValue.Text,
                Field5 = this.txtStaffName,
                Field22 = this.txtMERate,
                Field23 = this.txtGDRate,
                Field30 = this.txtLoadingMedical.Text,
                Field31 = this.txtloadingGrpdisc.Text,
                Field32 = this.txtNonOccDisc.Text,
                Field33 = this.txtloadingRSCC.Text,
                Field34 = this.txtLoadingOthers.Text,
                Field35 = this.txtFEADiscValue.Text,
                Field36 = this.txtLoyaltyRate.Text,
                Field37 = this.txtLoyaltyDiscValue.Text,
                Field38 = this.txtOtherDiscRate,
                Field39 = this.txtOtherDiscValue,
                Field40 = this.txtGrpDiscRate.Text,
                Field41 = this.txtLTADiscValue.Text,
                Field42 = this.txtVehicleNumber.Text,
                Field43 = this.txtDeclarationNumber.Text,
                Field44 = this.txtVehiclefrom.Text,
                Field46 = this.txtVehicleTo.Text,
                Field47 = this.txtPerCapitaCharge.Text,
                Field49 = this.txttotalEmployees.Text,

                Field45 = this.cdbReversalTypes.SelectedItem.Value,
                Field48 = "", // Endorsementtype 'NEW/RENEWAL/ADDITIONAL'
                Field50 = "PENDING",
            };
        }
    }
}
