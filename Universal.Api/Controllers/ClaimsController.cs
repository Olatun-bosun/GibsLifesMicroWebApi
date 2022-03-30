using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class ClaimsController : SecureControllerBase
    {
        public ClaimsController(Repository repository) : base(repository)
        {
        }

        /// <summary>
        /// Fetch a collection of Claims.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A collection of Claims.</returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ClaimResult>>> ListClaimsAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                if (User.IsAgent())
                {
                    var claims = await _repository.SelectAgentClaimsAsync(filter, GetCurrUserId());
                    return claims.Select((c => new ClaimResult(c))).ToList();
                }
                else
                {
                    var claims = await _repository.SelectCustomerClaimsAsync(filter, GetCurrUserId());
                    return claims.Select((c => new ClaimResult(c))).ToList();
                }
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
        public ActionResult<ClaimResult> GetClaim(string claimNo)
        {
            try
            {
                var claim = _repository.ClaimSelectThis(claimNo);
                if (claim is null)
                {
                    return NotFound();
                }

                return Ok(new ClaimResult(claim));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create a Claim.
        /// </summary>
        /// <returns>A newly created claim</returns>
        [HttpPost]
        public ActionResult<ClaimResult> CreateClaim(ClaimResult claimDetails)
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
