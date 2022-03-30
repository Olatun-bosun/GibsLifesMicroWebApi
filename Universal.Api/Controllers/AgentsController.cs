using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Contracts;
using Microsoft.AspNetCore.Http;

namespace Universal.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AgentsController : SecureControllerBase
    {
        private readonly Settings _settings;
        public AgentsController(Repository repository, Settings settings) : base(repository)
        {
            _settings = settings;
        }

        ///<summary>
        /// Creates a jwt token that can be used to access secured endpoints of the api.
        /// </summary>
        /// <returns>A jwt token and it's expiry time.</returns>
        [HttpPost("Login"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginResult> AgentLogin(LoginRequest loginCreds)
        {
            try
            {
                if (loginCreds == null)
                    return Problem("request body is null", statusCode: 400);

                try
                {
                    var validUser = _repository.AuthenticateAgent(loginCreds.id, loginCreds.password);
                    if (validUser == null)
                        return Problem("SID or password is incorrect.", statusCode: 400);

                    string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn, loginCreds.id, "Agent");

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
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a collection of Agents.
        /// </summary>
        /// <returns>A collection of agents.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentResult>>> ListAgentsAsync(
            [FromQuery] string searchText, [FromQuery]int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var agents = await _repository.PartySelectAsync(GetCurrUserId(), searchText, pageNo, pageSize);
                return Ok(agents.Select(a => new AgentResult(a)).ToList());
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single agent.
        /// </summary>
        /// <param name="agentId">Id of the agent to get.</param>
        /// <returns>The agent with the Id entered.</returns>
        [HttpGet("{agentId}")]
        public ActionResult<AgentResult> GetAgent(string agentId)
        {
            try
            {
                var agent = _repository.PartySelectThis(agentId);
                if (agent is null)
                {
                    return NotFound();
                }

                return Ok(new AgentResult(agent));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create an Agent.
        /// </summary>
        /// <returns>A newly created Agent</returns>
        [HttpPost]
        public ActionResult<AgentResult> Post(CreateNewAgentRequest agentDetails)
        {
            try
            {
                var party = _repository.PartyCreate(agentDetails);
                _repository.SaveChanges();
                var uri = new Uri($"{Request.Path}/{ party.PartyID}", UriKind.Relative);

                return Created(uri, new AgentResult(party));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
