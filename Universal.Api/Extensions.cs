using System;
using Universal.Api.Models;
using Universal.Api.Contracts.V1;
using System.Security.Principal;

namespace Universal.Api
{
    public static class Extensions
    {
        public static bool IsCustomer(this IPrincipal principal)
        {
            return principal.IsInRole("Customer");
        }

        public static bool IsAgent(this IPrincipal principal)
        {
            return principal.IsInRole("Agent");
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsAviation A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsBond A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsEngineering A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsFire A, Policy policy)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policy.PolicyNo
            };

            //ptd.CertOrDocNo = strAutoNumber;
            //ptd.EntryDate = policy.TransDate;
            //ptd.SubmittedBy = policy.SubmittedBy;
            //ptd.SubmittedOn = DateTime.Now;

            //ptd.InsuredName = policy.InsSurname;
            //ptd.StartDate = policy.StartDate;
            //ptd.EndDate = policy.EndDate;
            //ptd.ExRate = policy.ExRate;
            //ptd.ExCurrency = policy.ExCurrency;
            //ptd.PremiumRate = policy.PremiumRate;
            //ptd.ProportionRate = policy.ProportionRate;
            //ptd.SumInsured = policy.SumInsured;
            //ptd.TotalRiskValue = policy.SumInsured;
            //ptd.GrossPremium = policy.GrossPremium;
            //ptd.SumInsuredFrgn = policy.SumInsuredFrgn;
            //ptd.GrossPremiumFrgn = policy.GrossPremiumFrgn;
            //ptd.ProRataDays = policy.ProRataDays;
            //ptd.ProRataPremium = policy.ProRataPremium;
            //A.
            //ptd.NetAmount = Me.txtTotalNetPremium.Text; //Val(Me.txtNetPremium.Text);
            //ptd.Deleted = 0;
            //ptd.Active = 1;

            ////add other specific details
            //ptd.Field29 = policy.InsSurname;
            //ptd.Field26 = Me.txtContent.Text;
            //ptd.Field17 = A.Description;
            //ptd.Field25 = A.SectionName;
            //ptd.Field28 = Me.txtContent.Text;
            //ptd.Field27 = Me.txtLienClauses.Text;
            //ptd.Field1 = Me.txtFEADisc.Text;
            //ptd.Field2 = Me.txtLTADisc.Text;
            //ptd.Field30 = Me.txtA1Val.Text;
            //ptd.Field21 = Me.cbFireOption.SelectedItem.Text;

            //ptd.Field34 = Me.txtD1.Text;
            //ptd.Field35 = Me.txtD2.Text;
            //ptd.Field45 = Me.cdbReversalTypes.SelectedItem.Value;
            //ptd.Field48 = Me.cdbEndorseOption.SelectedItem.Value;

            //ptd.Field50 = "PENDING";



            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsGeneralAccident A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsMarineCargo A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsMarineHull A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsMotor A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

        public static PolicyDetail MapPolicyDetail(this PolicyAsOilGas A, string policyNo)
        {
            var ptd = new PolicyDetail
            {
                PolicyNo = policyNo
            };


            return ptd;
        }

    }
}
