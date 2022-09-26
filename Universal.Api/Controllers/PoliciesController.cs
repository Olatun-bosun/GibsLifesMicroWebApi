using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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

        #region Get Policy

        [HttpGet("Agric/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsAgric>>> GetAgricPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsAgric>(policyNo);
        }

        [HttpGet("Aviation/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsAviation>>> GetAviationPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsAviation>(policyNo);
        }

        [HttpGet("Bond/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsBond>>> GetBondPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsBond>(policyNo);
        }

        [HttpGet("Engineering/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsEngineering>>> GetEngineeringPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsEngineering>(policyNo);
        }

        [HttpGet("Fire/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsFire>>> GetFirePolicy(string policyNo)
        {
            return GetPolicy<PolicyAsFire>(policyNo);
        }

        [HttpGet("Accident/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsAccident>>> GetAccidentPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsAccident>(policyNo);
        }

        [HttpGet("MarineCargo/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsMarineCargo>>> GetMarineCargoPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsMarineCargo>(policyNo);
        }

        [HttpGet("MarineHull/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsMarineHull>>> GetMarineHullPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsMarineHull>(policyNo);
        }

        [HttpGet("Motor/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsMotor>>> GetMotorPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsMotor>(policyNo);
        }

        [HttpGet("OilGas/{policyNo}")]
        public Task<ActionResult<PolicyResult<PolicyAsOilGas>>> GetOilGasPolicy(string policyNo)
        {
            return GetPolicy<PolicyAsOilGas>(policyNo);
        }

        #endregion


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
        public Task<ActionResult<PolicyResult<PolicyAsAgric>>> NewAgricPolicy(CreateNew<PolicyAsAgric> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Aviation")]
        public Task<ActionResult<PolicyResult<PolicyAsAviation>>> NewAviationPolicy(CreateNew<PolicyAsAviation> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Bond")]
        public Task<ActionResult<PolicyResult<PolicyAsBond>>> NewBondPolicy(CreateNew<PolicyAsBond> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Engineering")]
        public Task<ActionResult<PolicyResult<PolicyAsEngineering>>> NewEngineeringPolicy(CreateNew<PolicyAsEngineering> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Fire")]
        public Task<ActionResult<PolicyResult<PolicyAsFire>>> NewFirePolicy(CreateNew<PolicyAsFire> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Accident")]
        public Task<ActionResult<PolicyResult<PolicyAsAccident>>> NewAccidentPolicy(CreateNew<PolicyAsAccident> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("MarineCargo")]
        public Task<ActionResult<PolicyResult<PolicyAsMarineCargo>>> NewMarineCargoPolicy(CreateNew<PolicyAsMarineCargo> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("MarineHull")]
        public Task<ActionResult<PolicyResult<PolicyAsMarineHull>>> NewMarineHullPolicy(CreateNew<PolicyAsMarineHull> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("Motor")]
        public Task<ActionResult<PolicyResult<PolicyAsMotor>>> NewMotorPolicy(CreateNew<PolicyAsMotor> policyDto)
        {
            return NewPolicy(policyDto);
        }

        [HttpPost("OilGas")]
        public Task<ActionResult<PolicyResult<PolicyAsOilGas>>> NewOilGasPolicy(CreateNew<PolicyAsOilGas> policyDto)
        {
            return NewPolicy(policyDto);
        }

        #endregion

        private async Task<ActionResult<PolicyResult<T>>> NewPolicy<T>(CreateNew<T> newPolicyDto) 
            where T : RiskDetail
        {
            try
            {
                var policy = await _repository.PolicyCreateAsync(newPolicyDto);
                await _repository.SaveChangesAsync();

                var uri = new Uri($"{Request.Path}/{policy.PolicyNo}", UriKind.Relative);
                return Created(uri, new PolicyResult<T>(policy));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        private async Task<ActionResult<PolicyResult<T>>> GetPolicy<T>(string policyNo)
            where T : RiskDetail
        {
            try
            {
                //policyNo contains / forward slashes
                policyNo = System.Web.HttpUtility.UrlDecode(policyNo);
                var policy = await _repository.PolicySelectThisAsync(policyNo);

                if (policy is null)
                    return NotFound();

                return Ok(new PolicyResult<T>(policy));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}
