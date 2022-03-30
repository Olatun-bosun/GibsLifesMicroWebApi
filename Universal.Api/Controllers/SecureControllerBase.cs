using System;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class SecureControllerBase : ControllerBase
    {
        protected readonly Repository _repository;

        public SecureControllerBase(Repository repository)
        {
            _repository = repository;
        }

        protected string GetCurrUserId()
        {
            return User?.FindFirst(c => c.Type == ClaimTypes.Name).Value;
        }

        protected ActionResult ExceptionResult(Exception ex)
        {
            if (ex is null)
                return BadRequest("Empty API error message");

            if (ex is ArgumentException || ex is ArgumentNullException || ex is KeyNotFoundException)
                return BadRequest(ex.Message);

            if (ex.InnerException != null)
                return StatusCode(500, ex.Message + " --- inner exception --- " + ex.InnerException.ToString());

            return StatusCode(500, ex.Message);
        }

        protected string CreateToken(string secret, int expiresIn, string username, string primaryRole)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                             new Claim(ClaimTypes.Email, username),
                             new Claim(ClaimTypes.Name, username),
                             new Claim(ClaimTypes.Role, primaryRole)
                }),
                Expires = DateTime.UtcNow.AddSeconds(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
