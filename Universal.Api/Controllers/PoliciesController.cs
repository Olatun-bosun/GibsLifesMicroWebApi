using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Controllers;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Contracts.V1
{
    public class PoliciesController : SecureControllerBase
    {
        public PoliciesController(Repository repository) : base(repository)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolicyResult>>> ListPoliciesAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                var policy = await _repository.PolicySelectAsync(filter, GetCurrUserId());
                return policy.Select((O => new PolicyResult(O))).ToList();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{policyNo}")]
        public ActionResult<PolicyResult> GetPolicy(string policyNo)
        {
            try
            {
                //this.CheckApiKey(ApiKey);
                var policy = _repository.PolicySelectThis(policyNo);

                if(policy is null)
                {
                    return NotFound();
                }
                return Ok(new PolicyResult(policy));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #region Renew / Delete Policy

        //[HttpPost("{policyNo}/renew")]
        //public ActionResult<PolicyDto> RenewPolicy(string policyNo, DateTime effectiveDate)
        //{
        //    var result = _repository.PolicyUpdate(policyNo, effectiveDate);

        //    var policy = _repository.PolicySelectThis(policyNo);
        //    return Ok(new PolicyDto(policy));
        //}

        //[HttpDelete("{policyNo}")]
        //public ActionResult<string> TerminatePolicy(string policyNo, string remarks)
        //{
        //    try
        //    {
        //        //this.CheckApiKey(ApiKey);
        //        //_repository.PolicyDelete(policyNo);
        //        return Ok(remarks);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        //[HttpGet("quote")]
        //public ActionResult<PolicyDto> QuotePolicy()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region Create Policy

        [HttpPost("aviation")]
        public ActionResult<PolicyResult> NewAviationPolicy(CreateNewPolicyAsAviation policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("bond")]
        public ActionResult<PolicyResult> NewBondPolicy(CreateNewPolicyAsBond policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("engineering")]
        public ActionResult<PolicyResult> NewEngineeringPolicy(CreateNewPolicyAsEngineering policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("fire")]
        public ActionResult<PolicyResult> NewFirePolicy(CreateNewPolicyAsFire policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("accident")]
        public ActionResult<PolicyResult> NewAccidentPolicy(CreateNewPolicyAsGeneralAccident policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("marinecargo")]
        public ActionResult<PolicyResult> NewMarineCargoPolicy(CreateNewPolicyAsMarineCargo policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("marinehull")]
        public ActionResult<PolicyResult> NewMarineHullPolicy(CreateNewPolicyAsMarineHull policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("motor")]
        public ActionResult<PolicyResult> NewMotorPolicy(CreateNewPolicyAsMotor policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        [HttpPost("oilgas")]
        public ActionResult<PolicyResult> NewOilGasPolicy(CreateNewPolicyAsOilGas policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicyDetails);
        }

        private ActionResult<PolicyResult> NewPolicy<T>(CreateNew<T> policyDto, 
                                                     IEnumerable<PolicyRequest> sectionsDto) 
            where T : PolicyRequest
        {
            try
            {
                //var policy = _repository.PolicyCreate(policyDto, sectionsDto);
                //_repository.SaveChanges();

                ////var to = policy.InsEmail;
                ////var cc = "jelamah@cornerstone.com.ng";
                ////var bcc = "oseniwasiu@inttecktechnologies.com";
                ////var pdf = Documents.Certificate.ToPdfStream(policy);

                ////Documents.Certificate.SendEmailAsync(to, cc, bcc, pdf);

                var uri = new Uri($"{Request.Path}/demo_policy_number", UriKind.Relative);

                return Created(uri, null);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #endregion
    }
}
