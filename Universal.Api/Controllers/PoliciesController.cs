using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Controllers;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Contracts.V1
{
    [Route("api/v1/[controller]")]
    public class PoliciesController : SecureControllerBase
    {
        public PoliciesController(Repository repository) : base(repository)
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
                var policy = await _repository.PolicySelectAsync(filter, GetCurrUserId());
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

        #region NewPolicy

        /// <summary>
        /// Create Aviation policy.
        /// </summary>
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
        /// <returns>A newly created engineering insurance policy</returns>
        [HttpPost("engineering")]
        public ActionResult<PolicyDto> NewEngineeringPolicy(NewPolicyDto<EngineeringDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create fire policy.
        /// </summary>
        /// <returns>A newly created fire insurance policy</returns>
        [HttpPost("fire")]
        public ActionResult<PolicyDto> NewFirePolicy(NewPolicyDto<FireDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }


        /// <summary>
        /// Create accident policy.
        /// </summary>
        /// <returns>A newly created accident insurance policy</returns>
        [HttpPost("accident")]
        public ActionResult<PolicyDto> NewAccidentPolicy(NewPolicyDto<GeneralAccidentDto> policyDto)
        {
            return NewPolicy(policyDto, policyDto.PolicySection);
        }

        /// <summary>
        /// Create marine cargo policy.
        /// </summary>
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

        private ActionResult<PolicyDto> NewPolicy(PolicyDto policyDto, IEnumerable<PolicyDetailDto> sectionsDto)
        {
            try
            {
                throw new NotImplementedException();
                //var policy = _repository.PolicyCreate(policyDto, sectionsDto);
                //_repository.SaveChanges();

                ////var to = policy.InsEmail;
                ////var cc = "jelamah@cornerstone.com.ng";
                ////var bcc = "oseniwasiu@inttecktechnologies.com";
                ////var pdf = Documents.Certificate.ToPdfStream(policy);

                ////Documents.Certificate.SendEmailAsync(to, cc, bcc, pdf);

                //var uri = new Uri($"{Request.Path}/{policy.PolicyNo}", UriKind.Relative);

                //return Created(uri, policy);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        #endregion
    }
}
