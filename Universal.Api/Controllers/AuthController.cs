using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Universal.Api.Data.Repositories;
using static Universal.Api.Contracts.SessionContract;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : SecureControllerBase
    {
        private readonly Settings _settings;
        public AuthController(IRepository repository, Settings settings) : base(repository)
        {
            _settings = settings;
        }

        /// <summary>
        /// Creates a jwt token that can be used to access secured endpoints of the api.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Auth
        ///     {
        ///        "sid": "GibsUser1",
        ///        "token": "gibs117#"
        ///     }
        ///
        /// </remarks>
        /// <param name="loginCreds"></param>
        /// <returns>A jwt token and it's expiry time.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginResponse> Login(LoginRequest loginCreds)
        {
            if (loginCreds == null)
                return Problem("request body is null", statusCode:400);

            try
            {
                var validUser = _repository.Authenticate(loginCreds.sid, loginCreds.token);
                if (validUser == null)
                    return Problem("SID or password is incorrect.", statusCode: 400);

                string token = CreateToken(_settings.JwtSecret,
                                       _settings.JwtExpiresIn, loginCreds.sid, "test");

                var response = new LoginResponse
                {
                    TokenType = "Bearer",
                    ExpiresIn = _settings.JwtExpiresIn,
                    AccessToken = token
                };
                return Ok(response);

                string CreateToken(string secret, int expiresIn, string username, string partyId)
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
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Deletes the jwt token that's associated with this user.
        /// </summary>
        [HttpDelete("current")]
        public ActionResult Logout()
        {
            //destroy the token
            return Ok();
        }
    }
}
