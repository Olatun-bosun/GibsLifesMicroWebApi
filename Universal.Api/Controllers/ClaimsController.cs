using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    public class ClaimsController : SecureControllerBase
    {
        public ClaimsController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Fetch a collection of Claims.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A collection of Claims.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClaimDto>>> ListClaimsAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                var claims = await _repository.ClaimsSelectAsync(filter);
                return claims.Select((c => new ClaimDto(c))).ToList();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single claim.
        /// </summary>
        /// <param name="claimNo"></param>
        /// <returns>The claim with the claimNo entered.</returns>
        [HttpGet("{claimNo}")]
        public ActionResult<ClaimDto> GetClaim(string claimNo)
        {
            try
            {
                var claim = _repository.ClaimSelectThis(claimNo);
                if (claim is null)
                {
                    return NotFound();
                }

                return Ok(new ClaimDto(claim));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create a Claim.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Claims
        ///     {
        ///        "policyNo": "string",
        ///        "notifyDate": "2021-09-14T11:57:34.461Z",
        ///        "lossDate": "2021-09-14T11:57:34.462Z",
        ///        "lossType": "string",
        ///        "lossDetails": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="claimDetails"></param>
        /// <returns>A newly created claim</returns>
        [HttpPost]
        public ActionResult<ClaimDto> CreateClaim(ClaimDto claimDetails)
        {
            try
            {
                //var claim = _repository.ClaimCreate(claimDetails);
                //var uri = new Uri($"{Request.Path}/{claim.NotificatnNo}", UriKind.Relative);

                //return Created(uri, new ClaimDto(claim));
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        
    }
}
