using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class SecureControllerBase : ControllerBase
    {
        protected readonly IRepository _repository;

        public SecureControllerBase(IRepository repository)
        {
            _repository = repository;
        }

        protected string GetCurrUserPartyId()
        {
            return User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
        }

        protected ActionResult ExceptionResult(Exception ex)
        {
            if (ex is null)
                return BadRequest("Empty API error message");

            if (ex is ArgumentException || ex is ArgumentNullException || ex is KeyNotFoundException)
                return BadRequest(ex.Message);

            if (ex.InnerException != null)
                return InternalServerError(ex.Message + " --- inner exception --- " + ex.InnerException.ToString());

            return InternalServerError(ex.Message);
        }

        protected ActionResult InternalServerError(string message)
        {
            return Problem(detail: message, statusCode: 500);
        }
    }
}
