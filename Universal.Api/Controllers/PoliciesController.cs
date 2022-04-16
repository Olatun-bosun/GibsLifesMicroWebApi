using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Controllers;
using Universal.Api.Data.Repositories;
using Universal.Api.Data;

namespace Universal.Api.Contracts.V1
{
    [Authorize(Roles = "APP,AGENT,CUST")]
    public class PoliciesController : SecureControllerBase
    {
        public PoliciesController(Repository repository, AuthContext authContext) : base(repository, authContext)
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PolicyResult>>> ListPolicies([FromQuery] FilterPaging filter)
        {
            try
            {
                var policy = await _repository.PolicySelectAsync(filter);
                return policy.Select(x => new PolicyResult(x)).ToList();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpGet("{policyNo}")]
        public async Task<ActionResult<PolicyResult>> GetPolicy(string policyNo)
        {
            try
            {
                var policy = await _repository.PolicySelectThisAsync(policyNo);

                if(policy is null)
                    return NotFound();

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
        public Task<ActionResult<PolicyResult>> NewAviationPolicy(CreateNewPolicyAsAviation policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("bond")]
        public Task<ActionResult<PolicyResult>> NewBondPolicy(CreateNewPolicyAsBond policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("engineering")]
        public Task<ActionResult<PolicyResult>> NewEngineeringPolicy(CreateNewPolicyAsEngineering policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("fire")]
        public Task<ActionResult<PolicyResult>> NewFirePolicy(CreateNewPolicyAsFire policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("accident")]
        public Task<ActionResult<PolicyResult>> NewAccidentPolicy(CreateNewPolicyAsGeneralAccident policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("marinecargo")]
        public Task<ActionResult<PolicyResult>> NewMarineCargoPolicy(CreateNewPolicyAsMarineCargo policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("marinehull")]
        public Task<ActionResult<PolicyResult>> NewMarineHullPolicy(CreateNewPolicyAsMarineHull policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("motor")]
        public Task<ActionResult<PolicyResult>> NewMotorPolicy(CreateNewPolicyAsMotor policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("oilgas")]
        public Task<ActionResult<PolicyResult>> NewOilGasPolicy(CreateNewPolicyAsOilGas policyDto)
        {
            return NewPolicy(policyDto);
        }

        #endregion

        private async Task<ActionResult<PolicyResult>> NewPolicy<T>(CreateNew<T> newPolicyDto) 
            where T : PolicyRequest
        {
            try
            {
                var policy = await _repository.PolicyCreateAsync(newPolicyDto);
                _repository.SaveChanges();

                var uri = new Uri($"{Request.Path}/{policy.PolicyNo}", UriKind.Relative);
                return Created(uri, new PolicyResult(policy));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
