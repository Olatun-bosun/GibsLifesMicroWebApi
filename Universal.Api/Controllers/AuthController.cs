//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using Universal.Api.Contracts;
//using Universal.Api.Data.Repositories;

//namespace Universal.Api.Controllers
//{
//    [Route("api/v1/[controller]")]
//    [AllowAnonymous]
//    public class AuthController : SecureControllerBase
//    {
//        private readonly Settings _settings;
//        public AuthController(Repository repository, Settings settings) : base(repository)
//        {
//            _settings = settings;
//        }

//        /// <summary>
//        /// Creates a jwt token that can be used to access secured endpoints of the api.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST /Auth
//        ///     {
//        ///        "sid": "GibsUser1",
//        ///        "token": "gibs117#"
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="loginCreds"></param>
//        /// <returns>A jwt token and it's expiry time.</returns>
//        [HttpPost]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<LoginResponse> Login(LoginRequest loginCreds)
//        {
//            if (loginCreds == null)
//                return Problem("request body is null", statusCode: 400);

//            try
//            {
//                var validUser = _repository.AuthenticateAdmin(loginCreds.sid, loginCreds.token);
//                if (validUser == null)
//                    return Problem("SID or password is incorrect.", statusCode: 400);

//                string token = CreateToken(_settings.JwtSecret,
//                                       _settings.JwtExpiresIn, loginCreds.sid, "Admin");

//                var response = new LoginResponse
//                {
//                    TokenType = "Bearer",
//                    ExpiresIn = _settings.JwtExpiresIn,
//                    AccessToken = token
//                };
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return ExceptionResult(ex);
//            }
//        }

//        /// <summary>
//        /// Deletes the jwt token that's associated with this user.
//        /// </summary>
//        [HttpDelete("current")]
//        public ActionResult Logout()
//        {
//            //destroy the token
//            return Ok();
//        }
//    }
//}
