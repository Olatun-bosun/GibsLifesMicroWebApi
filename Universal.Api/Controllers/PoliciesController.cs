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
using Universal.Api.Contracts.V1.RiskDetails;

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
                //policyNo contains / forward slashes
                policyNo = System.Web.HttpUtility.UrlDecode(policyNo);
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

        [HttpPost("renew/{policyNo}")]
        public async Task<ActionResult<PolicyResult>> RenewPolicy(string policyNo, DateTime effectiveDate)
        {
            try
            {
                //policyNo contains / forward slashes
                policyNo = System.Web.HttpUtility.UrlDecode(policyNo);
                //var result = _repository.PolicyUpdate(policyNo, effectiveDate);

                //var policy = _repository.PolicySelectThis(policyNo);
                //return Ok(new PolicyDto(policy));
                await Task.Delay(1000);

                return NotFound(policyNo);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        [HttpDelete("{policyNo}")]
        public async Task<ActionResult<string>> TerminatePolicy(string policyNo, string remarks)
        {
            try
            {
                //policyNo contains / forward slashes
                policyNo = System.Web.HttpUtility.UrlDecode(policyNo);
                await Task.Delay(1000);

                return NotFound(policyNo);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #region Create Policy

        [HttpPost("Agric")]
        public Task<ActionResult<PolicyResult>> NewAgricPolicy(CreateNew<PolicyAsAgric> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Aviation")]
        public Task<ActionResult<PolicyResult>> NewAviationPolicy(CreateNew<PolicyAsAviation> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Bond")]
        public Task<ActionResult<PolicyResult>> NewBondPolicy(CreateNew<PolicyAsBond> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Engineering")]
        public Task<ActionResult<PolicyResult>> NewEngineeringPolicy(CreateNew<PolicyAsEngineering> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Fire")]
        public Task<ActionResult<PolicyResult>> NewFirePolicy(CreateNew<PolicyAsFire> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Accident")]
        public Task<ActionResult<PolicyResult>> NewAccidentPolicy(CreateNew<PolicyAsAccident> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("MarineCargo")]
        public Task<ActionResult<PolicyResult>> NewMarineCargoPolicy(CreateNew<PolicyAsMarineCargo> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("MarineHull")]
        public Task<ActionResult<PolicyResult>> NewMarineHullPolicy(CreateNew<PolicyAsMarineHull> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Motor")]
        public Task<ActionResult<PolicyResult>> NewMotorPolicy(CreateNew<PolicyAsMotor> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("OilGas")]
        public Task<ActionResult<PolicyResult>> NewOilGasPolicy(CreateNew<PolicyAsOilGas> policyDto)
        {
            return NewPolicy(policyDto);
        }

        #endregion

        private async Task<ActionResult<PolicyResult>> NewPolicy<T>(CreateNew<T> newPolicyDto) 
            where T : RiskDetail
        {
            try
            {
                var policy = await _repository.PolicyCreateAsync(newPolicyDto);
                await _repository.SaveChangesAsync();

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
