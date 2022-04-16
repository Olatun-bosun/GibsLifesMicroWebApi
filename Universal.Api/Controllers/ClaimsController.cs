using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;
using Universal.Api.Data;

namespace Universal.Api.Controllers
{
    [Authorize(Roles = "APP,AGENT,CUST")]
    public class ClaimsController : SecureControllerBase
    {
        public ClaimsController(Repository repository, AuthContext authContext) : base(repository, authContext)
        {
        }

        /// <summary>
        /// Fetch a collection of Claims.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A collection of Claims.</returns>
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ClaimResult>>> ListClaims([FromQuery] FilterPaging filter)
        {
            try
            {
                var claims = await _repository.SelectClaimsAsync(filter);
                return claims.Select((x => new ClaimResult(x))).ToList();
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
