using System;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Universal.Api.Data.Repositories;
using Universal.Api.Data;

namespace Universal.Api.Controllers
{
    [ApiController]
    [ValidateModel]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class SecureControllerBase : ControllerBase
    {
        protected readonly Repository _repository;
        protected readonly AuthContext _authContext;

        public SecureControllerBase(Repository repository, AuthContext authContext)
        {
            _repository = repository;
            _authContext = authContext;
        }

        protected ActionResult ExceptionResult(Exception ex)
        {
            if (ex is null)
                return StatusCode(500, "SecureControllerBase.ExceptionResult() ex parameter cannot be null");

            if (ex is ArgumentException || ex is ArgumentNullException || ex is KeyNotFoundException)
                return BadRequest(ex.Message);

            if (ex.InnerException != null)
                return StatusCode(500, ex.Message + "\n\n\n --- inner exception --- " + ex.InnerException.ToString());

            return StatusCode(500, ex.Message);
        }

        protected string CreateToken(string secret, int expiresIn, 
                                     string appId, string userId, string tableId, string role)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("AppID", appId),
                    new Claim("UserID", userId),
                    new Claim("TableID", tableId),
                    new Claim(ClaimTypes.Name, $"{appId}/{userId}"),
                    new Claim(ClaimTypes.Role, role)
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
