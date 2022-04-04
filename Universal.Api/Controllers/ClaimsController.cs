using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
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
                    var claims = await _repository.SelectAgentClaimsAsync(GetCurrUserId(), filter);
                    return claims.Select((x => new ClaimResult(x))).ToList();
                }
                else
                {
                    var claims = await _repository.SelectCustomerClaimsAsync(GetCurrUserId(), filter);
                    return claims.Select((x => new ClaimResult(x))).ToList();
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
        public async Task<ActionResult<ClaimResult>> GetClaim(string claimNo)
        {
            try
            {
                var claim = await _repository.ClaimSelectThisAsync(claimNo);

                if (claim is null)
                    return NotFound();

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
        public async Task<ActionResult<ClaimResult>> CreateClaim(ClaimResult request)
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
