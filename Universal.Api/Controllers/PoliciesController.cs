using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Controllers;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Contracts.V1
{
    [Route("api/[controller]")]
    public class PoliciesController : SecureControllerBase
    {
        public PoliciesController(IRepository repository) : base(repository)
        {
        }


        /// <summary>
        /// Fetch a collection of policies.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A collection of policies.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolicyDto>>> ListPoliciesAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                var policy = await _repository.PolicySelectAsync(filter, GetCurrUserPartyId());
                return policy.Select((O => new PolicyDto(O))).ToList();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single policy.
        /// </summary>
        /// <param name="policyNo"></param>
        /// <returns>The policy matching the policyNo that was entered.</returns>
        [HttpGet("{policyNo}")]
        public ActionResult<PolicyDto> GetPolicy(string policyNo)
        {
            try
            {
                //this.CheckApiKey(ApiKey);
                var policy = _repository.PolicySelectThis(policyNo);
                if(policy is null)
                {
                    return NotFound();
                }
                return Ok(new PolicyDto(policy));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpPost("{policyNo}/renew")]
        //public ActionResult<PolicyDto> RenewPolicy(string policyNo, DateTime effectiveDate)
        //{
        //    var result = _repository.PolicyUpdate(policyNo, effectiveDate);

        //    var policy = _repository.PolicySelectThis(policyNo);
        //    return Ok(new PolicyDto(policy));
        //}

        /// <summary>
        /// Delete a single policy.
        /// </summary>
        /// <param name="policyNo"></param>
        /// <param name="remarks"></param>
        /// <returns>remarks.</returns>
        [HttpDelete("{policyNo}")]
        public ActionResult<string> TerminatePolicy(string policyNo, string remarks)
        {
            try
            {
                //this.CheckApiKey(ApiKey);
                //_repository.PolicyDelete(policyNo);
                return Ok(remarks);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        //[HttpGet("quote")]
        //public ActionResult<PolicyDto> QuotePolicy()
        //{
        //    throw new NotImplementedException();
        //}

        #region NewPolicy

        /// <summary>
        /// Create Aviation policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/aviation
        ///     {
        ///        "policyNo": "98932787832",
        ///        "productID": "5000",
        ///        "agentID": "0828923",
        ///        "entryDate": "2021-08-27T22:38:47.317Z",
        ///        "startDate": "2021-08-27T22:38:47.317Z",
        ///        "endDate": "2022-08-27T22:38:47.317Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "100",
        ///        "bizChannel": "ECHANNEL",
        ///        "insured": {
        ///             "insuredType": "INDIVIDUAL",
        ///             "title": "Mr.",
        ///             "firstName": "Kolade",
        ///             "lastName": "Wande",
        ///             "otherName": "Coal",
        ///             "insuredCode": "0973",
        ///             "addressProof": "12,uyeyueuye",
        ///             "stateOfOrigin": "n/a",
        ///             "telephone": "9828976323",
        ///             "dateOfBirth": "2012-08-27T22:38:47.317Z",
        ///             "identification": "INTL PASSPORT",
        ///             "industry": "Farming",
        ///             "mobilePhone": "08023140962",
        ///             "nationality": "NIGERIA",
        ///             "localGovtArea": "ALimosho",
        ///             "email": "oseniwasiu@yahoo.com",
        ///             "riskProfiling": "n/o",
        ///             "identificationNo": "0998098980"
        ///         },
        ///         "policySection": [ 
        ///         {
        ///             "policySectionID": 0,
        ///             "sectionID": "n/a",
        ///             "sectionName": "n/a",
        ///             "sectionSumInsured": 2500000,
        ///             "sectionPremium": 2500,
        ///             "riskLocation": "Abuja",
        ///             "interest": "being kjdwjkweiuhjkdshg",
        ///             "aircraftID": "kjhdwju",
        ///             "aircraftMake": "jhkdwjkiu",
        ///             "aircraftModel": "mnwdkjh",
        ///             "regMarks": "7892378",
        ///             "spareEquipments": "YES",
        ///             "maximumCrew": 5,
        ///             "passengerSeating": 25,
        ///             "licensedPassengers": 25,
        ///             "yearOfMfg": "1912",
        ///             "crewPersonalAccidents": "YES",
        ///             "engineType": "hjsd7623",
        ///             "usage": "COMMERCIAL",
        ///             "geographicalArea": "LAgos",
        ///             "declaredPassengers": 15,
        ///             "numberOfEngines": 4,
        ///             "numberOfPilots": 3,
        ///             "deductibles": "yuwuywd",
        ///             "nightFlight": true,
        ///             "aircraftSumInsured": 3490000,
        ///             "aircraftGrossPremium": 2500,
        ///             "aggregateSumInsured": 35000,
        ///             "profitCommDiscount": 5,
        ///             "ncdfLevyDiscount": 1,
        ///             "naicomLevyDiscount": 1,
        ///             "aggregateGrossPremium": 5000,
        ///             "netAggregatePremium": 5000
        ///         }]
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created Aviation Policy</returns>
        [HttpPost("aviation")]
        public ActionResult<PolicyDto> NewAviationPolicy(NewPolicyDto<AviationDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        [HttpPost("bond")]
        public ActionResult<PolicyDto> NewBondPolicy(NewPolicyDto<BondDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create engineering policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/engineering
        ///     {
        ///        "policyNo": "P/111/6012/2017/00007",
        ///        "productID": "601201",
        ///        "agentID": "AG982",
        ///        "entryDate": "2021-09-01T21:29:13.153Z",
        ///        "startDate": "2021-09-01T21:29:13.153Z",
        ///        "endDate": "2021-09-01T21:29:13.153Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "200",
        ///        "bizChannel": "ECHANNEL",
        ///        "insured": {
        ///            "insuredType": "CORPORATION",
        ///            "title": "n/a",
        ///            "firstName": "n/a",
        ///            "lastName": "GHG ENGINEERING COMPANY LIMITED",
        ///            "otherName": "",
        ///            "insuredCode": "0909392","insuredCode": "0909392",
        ///            "addressProof": "12, ishola street",
        ///            "stateOfOrigin": "Kaduna",
        ///            "telephone": "8932782332",
        ///            "dateOfBirth": "2021-09-01T21:29:13.153Z",
        ///            "identification": "INTL PASSPORT",
        ///            "industry": "Micr Corpoaration",
        ///            "mobilePhone": "08012323098",
        ///            "nationality": "NIGERIA",
        ///            "localGovtArea": "Souther Kaduna",
        ///            "email": "olawale@yahoo.com",
        ///            "riskProfiling": "POLITICAL EXPOSED PERSON",
        ///            "identificationNo": "06787878"
        ///        },
        ///        "policySection": [
        ///         {
        ///            "policySectionID": 0,
        ///            "sectionID": "601201",
        ///            "sectionName": "CONTRACTORS ALL RISK",
        ///            "sectionSumInsured": 540000,
        ///            "sectionPremium": 23000,
        ///            "riskLocation": "Abuja",
        ///            "ourShare": 100,
        ///            "contractorName": "GHG ENGINEERING COMPANY LIMITED",
        ///            "scopeOfContract": "MAJOR: 500,000.00 OR 10% AND MINOR: 250,000.00 OR 10%",
        ///            "projectConsultant": "Alikis Nigeria limited",
        ///            "principalName": "GHG ENGINEERING COMPANY LIMITED",
        ///            "majorExcess": 0,
        ///            "riskDescription": "ACCIDENTAL DAMAGE (DUE TO FIRE, LIGHTNING, FLOOD, ETC)",
        ///            "contractAwardDate": "",
        ///            "tppdExcess": 0,
        ///            "anyOneYear": "",
        ///            "plantUnderMaintenance": true,
        ///            "riskClassification": "CONTRACTORS EQUIPMENTS AND MACHINERY - ON SITE",
        ///            "propertyDescription": "ACCIDENTAL DAMAGE (DUE TO FIRE, LIGHTNING, FLOOD, ETC)",
        ///            "estimatedContractTerms": "",
        ///            "principalAddress": "12, juyhhweghewjhwejhwej",
        ///            "riskAddress": "12, juyhhweghewjhwejhwej",
        ///            "minorExcess": "n/a",
        ///            "industryID": "n/a",
        ///            "remarks": "n/a",
        ///            "anyOneLimit": "MAJOR: 500,000.00 OR 10% AND MINOR: 250,000.00 OR 10%",
        ///            "surveyRequired": true,
        ///            "maintenanceFrom": "12/01/2019",
        ///            "maintenanceTo": "12/20/2019"
        ///         }
        ///        ] 
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created engineering insurance policy</returns>
        [HttpPost("engineering")]
        public ActionResult<PolicyDto> NewEngineeringPolicy(NewPolicyDto<EngineeringDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create fire policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/fire
        ///     {
        ///        "policyNo": "IEJHIUE34",
        ///        "productID": "2021",
        ///        "agentID": "BR-0464",
        ///        "entryDate": "2021-09-01T20:16:45.102Z",
        ///        "startDate": "2021-09-01T20:16:45.102Z",
        ///        "endDate": "2021-09-01T20:16:45.102Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "100",
        ///        "bizChannel": "AGENT",
        ///        "insured": {
        ///            "insuredType": "CORPORATION",
        ///            "title": "n/a",
        ///            "firstName": "string",
        ///            "lastName": "JKK NIGERIA LIMITED",
        ///            "otherName": "string",
        ///            "insuredCode": "0909834",
        ///            "addressProof": "12, adesola str agbado lagos",
        ///            "stateOfOrigin": "Lagos",
        ///            "telephone": "808222322",
        ///            "dateOfBirth": "2021-09-01T20:16:45.102Z",
        ///            "identification": "DRIVERS LICENSED",
        ///            "industry": "Engineering",
        ///            "mobilePhone": "08023140962",
        ///            "nationality": "NIGERIA",
        ///            "localGovtArea": "ALimosho",
        ///            "email": "estable@yahoo.com",
        ///            "riskProfiling": "Political Exposed person",
        ///            "identificationNo": "08899383"
        ///        },
        ///        "policySection": [
        ///             {
        ///               "policySectionID": 0,
        ///               "sectionID": "0012",
        ///               "sectionName": "FIRE",
        ///               "sectionSumInsured": 340000,
        ///               "sectionPremium": 12000,
        ///               "riskLocation": "ABUJA",
        ///               "grossPremium": 12000,
        ///               "multiplier": 0,
        ///               "warLoading": 0,
        ///               "riskSMIID": "Building & Furniture and Fitting",
        ///               "rate": 5,
        ///               "totalSumInsured":340000 ,
        ///               "description": "Being 5 story building in Number 34 aderogba close",
        ///               "ourShareSumInsured": 340000 ,
        ///               "ourSharePremium":12000
        ///             }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created fire insurance policy</returns>
        [HttpPost("fire")]
        public ActionResult<PolicyDto> NewFirePolicy(NewPolicyDto<FireDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create accident policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/accident
        ///     {
        ///        "policyNo": "P/300/3032/2013/00002",
        ///        "productID": "3032",
        ///        "agentID": "BR-0498",
        ///        "entryDate": "2021-09-01T20:29:53.342Z",
        ///        "startDate": "2021-09-01T20:29:53.343Z",
        ///        "endDate": "2021-09-01T20:29:53.343Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "200",
        ///        "bizChannel": "E-CHANNEL",
        ///        "insured": {
        ///            "insuredType": "CORPORATION",
        ///            "title": "n/a",
        ///            "firstName": "n/a",
        ///            "lastName": "ADEKAL VENTURES ENT",
        ///            "otherName": "n/a",
        ///            "insuredCode": "0829293",
        ///            "addressProof": "40,jaolati road makinwa str",
        ///            "stateOfOrigin": "Lagos",
        ///            "telephone": "0808732333",
        ///            "dateOfBirth": "2021-09-01T20:29:53.343Z",
        ///            "identification": "DRIVING LICENSED",
        ///            "industry": "Information Technologies",
        ///            "mobilePhone": "08023140962",
        ///            "nationality": "NIGERIAN",
        ///            "localGovtArea": "Alimosho",
        ///            "email": "etuha@yahoo.com",
        ///            "riskProfiling": "NON POLITICAL EXPOSED",
        ///            "identificationNo": "0098238323"
        ///        },
        ///        "policySection": [
        ///         {
        ///            "policySectionID": 0,
        ///            "sectionID": "3032001",
        ///            "sectionName": "FAMILY PA",
        ///            "sectionSumInsured":18000000.00,
        ///            "sectionPremium": 48552.00,
        ///            "riskLocation": "ABUJA",
        ///            "ourShare": 100,
        ///            "contractorName": "MRS. IFEOMA CHUKWUOGO",
        ///            "model": "n/a",
        ///            "contractAwardDate": "08-02-1978",
        ///            "majorExcess": 0,
        ///            "riskDescription": "being insurance of family member personal accident",
        ///            "plantUnderMaintenance": true,
        ///            "lienClauses": "n/a",
        ///            "remarks": "n/a",
        ///            "surveyRequired": true,
        ///            "industryID": "n/a",
        ///            "mfgDetails": "PERMANENT DISABILITY",
        ///            "riskSMIID": "3032002",
        ///            "rate":5,
        ///            "totalSumInsured": 18000000.00,
        ///            "description": "48552.00",
        ///            "ourShareSumInsured": 18000000.00,
        ///            "ourSharePremium": 48552.00
        ///         }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created accident insurance policy</returns>
        [HttpPost("accident")]
        public ActionResult<PolicyDto> NewAccidentPolicy(NewPolicyDto<GeneralAccidentDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        /// <summary>
        /// Create marine cargo policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/marinecargo
        ///     {
        ///        "policyNo": "MC/108/4002/2017/00103",
        ///        "productID": "4002",
        ///        "agentID": "0993203",
        ///        "entryDate": "2021-09-01T20:50:56.624Z",
        ///        "startDate": "2021-09-01T20:50:56.624Z",
        ///        "endDate": "2021-09-01T20:50:56.624Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "100",
        ///        "bizChannel": "ECHANNEL",
        ///        "insured": {
        ///            "insuredType": "INDIVIDUAL",
        ///            "title": "Mr",
        ///            "firstName": "Makinde",
        ///            "lastName": "Adegbola",
        ///            "otherName": "wonuola",
        ///            "insuredCode": "000234",
        ///            "addressProof": "12, isijola str, mafoluku ",
        ///            "stateOfOrigin": "Ogun",
        ///            "telephone": "7834900430",
        ///            "dateOfBirth": "2021-09-01T20:50:56.624Z",
        ///            "identification": "INTL PASSPORT",
        ///            "industry": "Manufacturing",
        ///            "mobilePhone": "08023140962",
        ///            "nationality": "NIGERIA",
        ///            "localGovtArea": "Abeokuta South",
        ///            "email": "makinde@yahoo.com",
        ///            "riskProfiling": "POLITICAL EXPOSED PERSON",
        ///            "identificationNo": "9889333"
        ///        },
        ///        "policySection": [
        ///         {
        ///            "policySectionID": 0,
        ///            "sectionID": "n/a",
        ///            "sectionName": "n/a",
        ///            "sectionSumInsured": 2500000,
        ///            "sectionPremium": 12000,
        ///            "riskLocation": "Abuja",
        ///            "vesselType": "Single Transit",
        ///            "fromCountryID": "China",
        ///            "lienClause": "string",
        ///            "vesselOperation": "NAME OF CONVEYING  VESSEL AND MARKS & NUMBER OF PACKAGES/CONTAINERS MUST BE FURNISHED TO  LATER THAN 3 DAYS OF DEPARTURE FROM PORT OF SUPPLY.",
        ///            "certificateNo": "000008383",
        ///            "conveyanceID": "string",
        ///            "tinNumber": "12334-100",
        ///            "subjectMatter": "ICCA",
        ///            "toCountryID": "Nigeria",
        ///            "packageTypeID": "uywejwejh",
        ///            "proformaInvoiceNo": "09093998390",
        ///            "marksAndNumbers": "jhweiejjknwekjwejkwen",
        ///            "premiumRate": 12.5,
        ///            "basisOfValuation": "100%",
        ///            "otherDiscountRate": 0
        ///         }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created marine cargo insurance Policy</returns>
        [HttpPost("marinecargo")]
        public ActionResult<PolicyDto> NewMarineCargoPolicy(NewPolicyDto<MarineCargoDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        [HttpPost("marinehull")]
        public ActionResult<PolicyDto> NewMarineHullPolicy(NewPolicyDto<MarineHullDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create motor policy.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Policies/motor
        ///     {
        ///        "policyNo": "10010010776",
        ///        "productID": "1001",
        ///        "agentID": "SG-0027",
        ///        "entryDate": "2021-08-27T22:10:16.047Z",
        ///        "startDate": "2021-08-27T22:10:16.047Z",
        ///        "endDate": "2022-08-27T22:10:16.047Z",
        ///        "bizSource": "DIRECT",
        ///        "branchID": "100",
        ///        "bizChannel": "ECHANNEL",
        ///        "insured": {
        ///            "insuredType": "INDIVIDUAL",
        ///            "title": "ENGR",
        ///            "firstName": "IDRIS",
        ///            "lastName": "ABIODUN ",
        ///            "otherName": "ADESANYA",
        ///            "insuredCode": "569987",
        ///            "addressProof": "324, BORNU WAY, ALAGOMEJI, LAGOS.",
        ///            "stateOfOrigin": "n/a",
        ///            "telephone": "08023140786",
        ///            "dateOfBirth": "2021-08-27T22:10:16.048Z",
        ///            "identification": "DRIVER LICENSED",
        ///            "industry": "Film ",
        ///            "mobilePhone": "08023140962",
        ///            "nationality": "Nigeria",
        ///            "localGovtArea": "Alimosho",
        ///            "email": "023140962",
        ///            "riskProfiling": "string",
        ///            "identificationNo": "088982112"
        ///        },
        ///        "policySection": [
        ///         {
        ///            "policySectionID": 12,
        ///            "sectionID": "n/a",
        ///            "sectionName": "n/a",
        ///            "sectionSumInsured": 50000,
        ///            "sectionPremium": 5000,
        ///            "riskLocation": "Abuja",
        ///            "certificateTypeID": "891292",
        ///            "declarationNo": "78127898",
        ///            "vehicleRegNo": "LA 7272 IKEJ",
        ///            "vehicleTypeID": "SALON",
        ///            "vehicleUser": "Adewale Documu",
        ///            "engineNumber": "34232332ew23e",
        ///            "chasisNumber": "3232we23ew",
        ///            "vehicleUsageID": "PRIVATE",
        ///            "numberOfSeats": 5,
        ///            "stateOfIssueID": "Lagos",
        ///            "vehicleMakeID": "Toyota",
        ///            "vehicleModelID": "Camry",
        ///            "mfgYear": "1920",
        ///            "vehicleColour": "Blue",
        ///            "engineCapacityHP": "5.0",
        ///            "coverTypeID": "COMPREHENSIVE",
        ///            "waxCode": "W.A.X 1",
        ///            "vehicleValue": 500000,
        ///            "basicPremium":5000,
        ///            "proRataPremium": 5000,
        ///            "premiumRate": 5,
        ///            "coverDays": 365,
        ///            "tpfpRate": 0,
        ///            "tppdValue": 0,
        ///            "srccValue": 0,
        ///            "excessBuyBack": 0,
        ///            "pcssValue": 0,
        ///            "premiumDue": 5000,
        ///            "pluralityDiscount": 50,
        ///            "noClaimDiscount": 20,
        ///            "businessProportion": 100
        ///         }
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="policyDto"></param>
        /// <returns>A newly created motor insurance Policy</returns>
        [HttpPost("motor")]
        public ActionResult<PolicyDto> NewMotorPolicy(NewPolicyDto<MotorDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        [HttpPost("oilgas")]
        public ActionResult<PolicyDto> NewOilGasPolicy(NewPolicyDto<OilGasDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        private ActionResult<PolicyDto> NewPolicy(PolicyDto policyDto, IEnumerable<PolicySectionDto> sectionsDto)
        {
            try
            {
                //this.CheckApiKey(ApiKey);
                //var policy = _repository.PolicyCreate(policyDto, sectionsDto);

                //var to = policy.InsEmail;
                //var cc = "jelamah@cornerstone.com.ng";
                //var bcc = "oseniwasiu@inttecktechnologies.com";
                //var pdf = Documents.Certificate.ToPdfStream(policy);

                //Documents.Certificate.SendEmailAsync(to, cc, bcc, pdf);

                //var uri = new Uri($"{Request.Path}/{policy.PolicyNo}", UriKind.Relative);

                //return Created(uri, policy);

                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #endregion
    }
}
