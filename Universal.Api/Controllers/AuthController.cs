using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;
using Universal.Api.Data;

namespace Universal.Api.Controllers
{
    public class AuthController : SecureControllerBase
    {
        private readonly Settings _settings;
        public AuthController(Repository repository, AuthContext authContext, Settings settings) : base(repository, authContext)
        {
            _settings = settings;
        }

        /// <summary>
        /// Your App must Login using this endpoint. It creates a JWT Token that must be used to access secured endpoints of the API.
        /// </summary>
        /// <returns>A JWT Token and it's expiry time.</returns>
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<LoginResult>> AppLogin([FromBody] AppLoginRequest login)
        {
            if (login is null)
                return BadRequest("Request body is null");

            try
            {
                var apiUser = await _repository.AppLogin(login.AppId, login.Password);

                if (apiUser is null)
                    return NotFound("AppID or Password is incorrect");

                else if (apiUser.Status == "DISABLED")
                    return Unauthorized("This App has not been activated");

                string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn,
                                           login.AppId, "", "", "APP");

                var response = new LoginResult
                {
                    TokenType = "Bearer",
                    ExpiresIn = _settings.JwtExpiresIn,
                    AccessToken = token
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

    }
}
