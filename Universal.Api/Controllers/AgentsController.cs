using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;

namespace Universal.Api.Controllers
{
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
        public async Task<ActionResult<LoginResult>> AgentLogin([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest is null)
                return BadRequest("Request body is null");

            try
            {
                var agent = await _repository.PartyLoginOrNullAsync(loginRequest.Id, loginRequest.Password);

                if (agent is null)
                    return BadRequest("ID or Password is incorrect");

                string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn,
                                           loginRequest.Id, "Agent");

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

        /// <summary>
        /// Fetch a collection of Agents.
        /// </summary>
        /// <returns>A collection of agents.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentResult>>> ListAgentsAsync([FromQuery] FilterPaging filter)
        {
            try
            {
                var agents = await _repository.PartySelectAsync(GetCurrUserId(), filter);
                return Ok(agents.Select(x => new AgentResult(x)).ToList());
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
        public async Task<ActionResult<AgentResult>> GetAgent(string agentId)
        {
            try
            {
                var agent = await _repository.PartySelectThisOrNullAsync(agentId);

                if (agent is null)
                    return NotFound();

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
        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<AgentResult>> Post(CreateNewAgentRequest request)
        {
            try
            {
                var party = await _repository.PartyCreateAsync(request);
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
