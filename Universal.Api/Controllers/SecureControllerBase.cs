﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        protected readonly Repository _repository;

        public SecureControllerBase(Repository repository)
        {
            _repository = repository;
        }

        protected string GetCurrUserPartyId()
        {
            return User.FindFirst(c => c.Type == ClaimTypes.Name).Value ?? "test";
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

        protected string CreateToken(string secret, int expiresIn, string username, string partyId)
        {
            var key = Encoding.UTF8.GetBytes(secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                             new Claim(ClaimTypes.Email, username),
                             new Claim(ClaimTypes.Name, partyId),
                             new Claim(ClaimTypes.Role, "Agent")
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
