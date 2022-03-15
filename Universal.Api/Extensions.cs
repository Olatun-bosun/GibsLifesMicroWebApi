﻿using System;
using Universal.Api.Models;
using Universal.Api.Contracts.V1;

namespace Universal.Api
{
    public static class Extensions
    {
        public static PolicyDetail MapAviationPolicySection(this PolicyDetail ptd, AviationDto A)
        {
            ptd.SectionID = A.SectionID;
            ptd.ContentSection = A.Interest;
            ptd.Field1 = A.EngineType;
            ptd.Field2 = A.AircraftMake;
            ptd.Field3 = A.SpareEquipments;
            ptd.Field4 = A.RegMarks;
            ptd.Field5 = A.AircraftModel;
            ptd.Field6 = A.Usage;
            ptd.Field7 = A.GeographicalArea;
            ptd.Field10 = A.MaximumCrew.ToString();
            ptd.Field11 = A.PassengerSeating.ToString();
            ptd.Field12 = A.DeclaredPassengers.ToString();
            ptd.Field13 = A.LicensedPassengers.ToString();
            ptd.Field14 = A.NumberOfEngines.ToString();
            ptd.Field15 = A.YearOfMfg.ToString();
            ptd.Field17 = A.NumberOfPilots.ToString();
            ptd.Field18 = A.CrewPersonalAccidents;
            ptd.Field19 = "False";
            ptd.Field20 = A.DeclaredPassengers.ToString();
            ptd.Field21 = A.Deductibles;
            ptd.Field23 = A.SpareEquipments;
            ptd.Field30 = A.SectionName;
            ptd.RiskSum = A.AggregateSumInsured;
            ptd.OtherSum = A.AggregateSumInsured;
            ptd.SectionPremium = A.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = A.AggregateSumInsured;
            ptd.A13 = A.AircraftGrossPremium;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "H";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapAviationPolicySection(this PolicyDetail ptd, PolicyDetail A)
        {
            ptd.SectionID = A.SectionID;
            ptd.ContentSection = A.ContentSection;
            ptd.Field1 = A.Field1;
            ptd.Field2 = A.Field2;
            ptd.Field3 = A.Field3;
            ptd.Field4 = A.Field4;
            ptd.Field5 = A.Field5;
            ptd.Field6 = A.Field6;
            ptd.Field7 = A.Field7;
            ptd.Field10 = A.Field10;
            ptd.Field11 = A.Field11;
            ptd.Field12 = A.Field12;
            ptd.Field13 = A.Field13;
            ptd.Field14 = A.Field14;
            ptd.Field15 = A.Field15;
            ptd.Field17 = A.Field17;
            ptd.Field18 = A.Field18;
            ptd.Field19 = "False";
            ptd.Field20 = A.Field20;
            ptd.Field21 = A.Field21;
            ptd.Field23 = A.Field23;
            ptd.Field30 = A.Field30;
            ptd.RiskSum = A.RiskSum;
            ptd.OtherSum = A.OtherSum;
            ptd.SectionPremium = A.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = A.A12;
            ptd.A13 = A.A13;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "H";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapBondPolicySection(this PolicyDetail ptd, BondDto B)
        {
            ptd.SectionID = B.ContractorName;
            ptd.Field1 = B.PrincipalName;
            ptd.Field2 = B.NatureOfContract;
            ptd.Field3 = B.BorrowerName;
            ptd.Field4 = B.DirectorName;
            ptd.Field5 = B.ContractFrom.ToString();
            ptd.Field6 = B.ContractTo.ToString();
            ptd.Field7 = B.PrimaryGuarantor;
            ptd.Field8 = B.Remarks;
            ptd.Field9 = B.BondIssueDate.ToString();
            ptd.Field10 = B.PercOfContractValue.ToString();
            ptd.Field11 = B.TotalContractValue.ToString();
            ptd.Field12 = B.BondDuration.ToString();
            ptd.Field13 = B.BondTo.ToString();
            ptd.Field14 = B.BondFrom.ToString();
            ptd.Field15 = B.TotalBondValue.ToString();
            ptd.Field16 = B.ContractWork;
            ptd.Field17 = B.AwardDate.ToString();
            ptd.Field18 = B.AddressOfBorrower;
            ptd.Field19 = B.AddressOfContractor;
            ptd.Field21 = B.AddressOfPrincipal;
            ptd.RiskSum = B.SectionSumInsured;
            ptd.OtherSum = B.SectionSumInsured;
            ptd.SectionPremium = B.SectionPremium;
            ptd.A3 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(B);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapBondPolicySection(this PolicyDetail ptd, PolicyDetail B)
        {
            ptd.SectionID = B.SectionID;
            ptd.Field1 = B.Field1;
            ptd.Field2 = B.Field2;
            ptd.Field3 = B.Field3;
            ptd.Field4 = B.Field4;
            ptd.Field5 = B.Field5;
            ptd.Field6 = B.Field6;
            ptd.Field7 = B.Field7;
            ptd.Field8 = B.Field8;
            ptd.RiskSum = B.RiskSum;
            ptd.OtherSum = B.OtherSum;
            ptd.SectionPremium = B.SectionPremium;
            ptd.A3 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(B);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapEngineeringPolicySection(this PolicyDetail ptd, EngineeringDto E)
        {
            ptd.SectionID = E.SectionID;
            ptd.ContentSection = E.SectionName;
            ptd.Field1 = E.PropertyDescription;
            ptd.Field3 = E.SectionName;
            ptd.Field27 = E.MaintenanceFrom.ToString();
            ptd.Field28 = E.MaintenanceTo.ToString();
            ptd.RiskSum = E.SectionSumInsured;
            ptd.SectionPremium = E.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A30 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A36 = 0;
            ptd.A37 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "G";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapEngineeringPolicySection(this PolicyDetail ptd, PolicyDetail E)
        {
            ptd.SectionID = E.SectionID;
            ptd.ContentSection = E.ContentSection;
            ptd.Field1 = E.Field1;
            ptd.Field3 = E.Field3;
            ptd.SectionPremium = E.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A30 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A36 = 0;
            ptd.A37 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "G";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapFirePolicySection(this PolicyDetail ptd, FireDto F)
        {
            ptd.SectionID = F.SectionID;
            ptd.ContentSection = F.SectionName;
            ptd.Location = F.RiskLocation;
            ptd.Field2 = "";
            ptd.RiskSum = F.SectionSumInsured;
            ptd.SectionPremium = F.SectionPremium;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A10 = F.TotalSumInsured;
            ptd.A11 = F.GrossPremium;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(F);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapFirePolicySection(this PolicyDetail ptd, PolicyDetail F)
        {
            ptd.SectionID = F.SectionID;
            ptd.Field2 = "";
            ptd.SectionPremium = F.SectionPremium;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(F);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapGeneralAccidentPolicySection(this PolicyDetail ptd, GeneralAccidentDto G)
        {
            ptd.SectionID = G.SectionID;
            ptd.Field2 = G.LienClauses;
            ptd.Field3 = G.SectionName;
            ptd.RiskSum = G.SectionSumInsured;
            ptd.SectionPremium = G.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A30 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A36 = 0;
            ptd.A37 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(G);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapGeneralAccidentPolicySection(this PolicyDetail ptd, PolicyDetail G)
        {
            ptd.SectionID = G.SectionID;
            ptd.SectionPremium = G.SectionPremium;
            ptd.A3 = 0;
            ptd.A6 = 0;
            ptd.A7 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A12 = 0;
            ptd.A13 = 0;
            ptd.A14 = 0;
            ptd.A15 = 0;
            ptd.A16 = 0;
            ptd.A17 = 0.0;
            ptd.A18 = 0;
            ptd.A19 = 0;
            ptd.A20 = 0;
            ptd.A30 = 0;
            ptd.A21 = 0;
            ptd.A22 = 0;
            ptd.A23 = 0;
            ptd.A24 = 0;
            ptd.A25 = 0;
            ptd.A26 = 0;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A36 = 0;
            ptd.A37 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(G);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMarineCargoPolicySection(this PolicyDetail ptd, MarineCargoDto M)
        {
            ptd.SectionID = M.SectionID;
            ptd.ContentSection = M.ConveyanceID;
            ptd.Location = M.FromCountryID;
            ptd.Field3 = M.ToCountryID;
            ptd.Field2 = "Single Transit";
            ptd.Field2 = "Open Cover";
            ptd.Field5 = M.MarksAndNumbers;
            ptd.Field6 = M.CertificateNo;
            ptd.Field7 = M.PackageTypeID;
            ptd.Field9 = M.LienClause;
            ptd.Field11 = M.TINNumber;
            ptd.Field12 = M.MarksAndNumbers;
            ptd.Field13 = M.BasisOfValuation;
            ptd.A1 = (decimal)M.PremiumRate;
            ptd.SectionPremium = M.SectionPremium;
            ptd.A17 = M.PremiumRate;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(M);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMarineCargoPolicySection(this PolicyDetail ptd, PolicyDetail M)
        {
            ptd.SectionID = M.SectionID;
            ptd.Field2 = "Single Transit";
            ptd.Field2 = "Open Cover";
            ptd.SectionPremium = M.SectionPremium;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = nameof(M);
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMarineHullPolicySection(this PolicyDetail ptd, MarineHullDto H)
        {
            ptd.SectionID = H.VesselStateID;
            ptd.ContentSection = H.NameOfVessel;
            ptd.Field1 = "";
            ptd.Field2 = H.Builder;
            ptd.Field3 = H.VesselOperation;
            ptd.Field4 = H.Construction;
            ptd.Field6 = H.Excess;
            ptd.Field9 = H.VesselClass;
            ptd.Field10 = H.TerritorialLimits;
            ptd.Field11 = H.Length.ToString();
            ptd.Field12 = H.VesselTone;
            ptd.Field13 = H.Depth.ToString();
            ptd.Field14 = H.Draft.ToString();
            ptd.Field15 = "";
            ptd.Field16 = H.EngineType;
            ptd.RiskSum = H.SectionSumInsured;
            ptd.SectionPremium = H.SectionPremium;
            ptd.A3 = 0;
            ptd.A4 = (decimal)H.Rate;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "MH";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMarineHullPolicySection(this PolicyDetail ptd, PolicyDetail H)
        {
            ptd.Field1 = "";
            ptd.Field15 = "";
            ptd.SectionPremium = H.SectionPremium;
            ptd.A3 = 0;
            ptd.A8 = 0;
            ptd.A9 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "MH";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMotorPolicySection(this PolicyDetail ptd, MotorDto M)
        {
            ptd.SectionID = M.DeclarationNo;
            ptd.Field1 = M.VehicleRegNo;
            ptd.Field2 = M.VehicleTypeID;
            ptd.Field3 = M.StateOfIssueID;
            ptd.Field4 = M.VehicleColour;
            ptd.Field5 = M.MfgYear.ToString();
            ptd.Field6 = M.EngineCapacityHP;
            ptd.Field7 = M.EngineNumber;
            ptd.Field8 = M.ChasisNumber;
            ptd.Field9 = M.NumberOfSeats.ToString();
            ptd.Field23 = M.VehicleMakeID.ToUpper();
            ptd.Field24 = M.VehicleModelID.ToUpper();
            ptd.Field13 = M.VehicleUsageID;
            ptd.Field18 = M.WaxCode;
            ptd.Field20 = M.DeclarationNo;
            ptd.Field17 = M.CoverTypeID;
            ptd.Field21 = "";
            ptd.Field22 = "";
            ptd.A4 = M.PCSSValue;
            ptd.A7 = (decimal)M.SRCCValue;
            ptd.A16 = (decimal)M.TPFPRate;
            ptd.A17 = M.PremiumRate;
            ptd.A18 = M.BusinessProportion;
            ptd.A19 = M.TPPDValue;
            ptd.A21 = (decimal)M.PluralityDiscount;
            ptd.A23 = 0;
            ptd.A25 = M.ProRataPremium;
            ptd.A33 = (decimal)M.ExcessBuyBack;
            ptd.A36 = M.PremiumDue;
            ptd.A37 = M.ProRataPremium;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "V";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapMotorPolicySection(this PolicyDetail ptd, PolicyDetail M)
        {
            ptd.Field21 = "";
            ptd.Field22 = "";
            ptd.A23 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A50 = 0;
            ptd.Field30 = "V";
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapOilGasPolicySection(this PolicyDetail ptd, OilGasDto O)
        {
            ptd.SectionID = O.SectionID;
            ptd.ContentSection = O.InsuredSubscription;
            ptd.Field1 = O.OrderHereon;
            ptd.Field2 = O.ProjectPeriodFrom.ToString();
            ptd.Field3 = O.ProjectPeriodTo.ToString();
            ptd.Field4 = O.MaintenanceFrom.ToString();
            ptd.Field5 = O.MaintenanceTo.ToString();
            ptd.Field6 = O.ChoiceOfLaw.ToString();
            ptd.Field7 = O.Deductibles.ToString();
            ptd.Field8 = O.Situation;
            ptd.Field9 = O.Conditions;
            ptd.Field10 = O.RiskLocation;
            ptd.Field11 = O.DeductionsForRI.ToString();
            ptd.Field12 = O.DeductionFromPremium.ToString();
            ptd.Field13 = O.Remarks;
            ptd.SectionPremium = O.SectionPremium;
            ptd.A26 = O.SectionSumInsured;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static PolicyDetail MapOilGasPolicySection(this PolicyDetail ptd, PolicyDetail O)
        {
            ptd.SectionID = O.SectionID;
            ptd.SectionPremium = O.SectionPremium;
            ptd.A27 = 0;
            ptd.A28 = 0;
            ptd.A29 = 0;
            ptd.A30 = 0;
            ptd.A31 = 0;
            ptd.A32 = 0;
            ptd.A33 = 0;
            ptd.A34 = 0;
            ptd.A35 = 0;
            ptd.A38 = 0;
            ptd.A39 = 0;
            ptd.A40 = 0;
            ptd.A41 = 0;
            ptd.A42 = 0;
            ptd.A43 = 0;
            ptd.A44 = 0;
            ptd.A45 = 0;
            ptd.A46 = 0;
            ptd.A47 = 0;
            ptd.A48 = 0;
            ptd.A49 = 0;
            ptd.A50 = 0;
            ptd.TransGUID = Guid.NewGuid().ToString();
            ptd.SubmitBy = "E-CHANNEL";
            ptd.SubmitOn = DateTime.Now;
            return ptd;
        }

        public static DNCNNote MapDNNoteForPolicy(Policy P, PolicyDetail PD, Decimal TotalSumInsured, Decimal TotalGrossPremium)
        {
            string str = Guid.NewGuid().ToString().Split('-')[0];
            return new DNCNNote()
            {
                DNCNNo = str,
                refDNCNNo = str,
                PolicyNo = PD.PolicyNo,
                CoPolicyNo = PD.CoPolicyNo,
                BranchID = P.BranchID,
                BizSource = P.BizSource,
                BizOption = PD.BizOption,
                NoteType = "DN",
                BillingDate = DateTime.Now,
                SubRiskID = P.SubRiskID,
                SubRisk = P.SubRisk,
                PartyID = P.PartyID,
                Party = P.Party,
                PartyRate = 0,
                InsuredID = P.InsuredID,
                InsuredName = PD.InsuredName,
                StartDate = PD.StartDate,
                EndDate = PD.EndDate,
                SumInsured = TotalSumInsured,
                GrossPremium = TotalGrossPremium,
                Commission = 0,
                PropRate = 100.0,
                ProRataDays = 12L,
                ProRataPremium = 0,
                VatRate = 0.0,
                VatAmount = 0,
                NetAmount = 0,
                Narration = "Being policy premium  for Policy No. " + PD.PolicyNo,
                ExRate = 1.0,
                ExCurrency = "NAIRA",
                SumInsuredFrgn = 0,
                GrossPremiumFrgn = 0,
                Approval = 1,
                HasTreaty = 1,
                Remarks = "NORMAL",
                TopMostValue = 0,
                PMLValue = 0,
                PaymentType = "NORMAL",
                Deleted = 0,
                DeletedOn = DateTime.Now,
                Active = 1,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                TotalRiskValue = 0,
                TotalPremium = 0,
                RetProp = 0.0,
                RetValue = 0,
                RetPremium = 0,
                DBDate = DateTime.Now,
                A1 = 0,
                A2 = 0,
                A3 = 0,
                A4 = 0,
                A5 = 0,
                A6 = 0,
                A7 = 0,
                A8 = 0,
                A9 = 0,
                A10 = 0
            };
        }
    }
}