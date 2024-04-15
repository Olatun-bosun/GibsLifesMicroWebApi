using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GibsLifesMicroWebApi.Data.Repositories;
using GibsLifesMicroWebApi.Contracts.V1;
using GibsLifesMicroWebApi.Data;

namespace GibsLifesMicroWebApi.Controllers
{
    [Authorize(Roles = "APP")]
    public class AgentsController : SecureControllerBase
    {
        private readonly Settings _settings;
        public AgentsController(Repository repository, AuthContext authContext, Settings settings) : base(repository, authContext)
        {
            _settings = settings;
        }

        // <summary>
        // Agents should Login using this endpoint. It creates a JWT Token that can be used to access secured endpoints of the API.
        // </summary>
        // <returns>A JWT Token and it's expiry time.</returns>
        //[HttpPost("Login"), AllowAnonymous]
        //public async Task<ActionResult<LoginResult>> LoginAgent(AgentLoginRequest login)
        //{
        //    if (login is null)
        //        return BadRequest("Request body is null");

        //    try
        //    {
        //        var party = await _repository.PartyLoginAsync(login.AppID, login.AgentID, login.Password);

        //        if (party is null)
        //            return NotFound("AgentID or Password is incorrect");

        //        else if (party.ApiStatus != "ENABLED")
        //            return Unauthorized("This Agent has not been activated");

        //        string token = CreateToken(_settings.JwtSecret,
        //                                   _settings.JwtExpiresIn,
        //                                   login.AppID, 
        //                                   login.AgentID, 
        //                                   party.PartyID, "AGENT");
        //        var response = new LoginResult
        //        {
        //            TokenType = "Bearer",
        //            ExpiresIn = _settings.JwtExpiresIn,
        //            AccessToken = token
        //        };
        //        return Ok(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        /// <summary>
        /// Fetch a collection of Agents.
        /// </summary>
        /// <returns>A collection of Agents.</returns>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AgentResult>>> ListAgents([FromQuery] FilterPaging filter)
        //{
        //    try
        //    {
        //        var agents = await _repository.PartySelectAsync(filter);
        //        return Ok(agents.Select(x => new AgentResult(x)).ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        /// <summary>
        /// Fetch a single Agent.
        /// </summary>
        /// <param name="agentId">Id of the Agent to get.</param>
        /// <returns>The Agent with the Id entered.</returns>
        [HttpGet("{agentId}")]
        public async Task<ActionResult> GetAgent(string agentId)
        {
            try
            {
                var agent = await _repository.AgentSelectThisAsync(agentId);

                if (agent is null)
                    return NotFound();

                return Ok(agent);
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
        public async Task<ActionResult> CreateAgent(CreateNewAgentRequest request)
        {
            try
            {
                var agent = await _repository.AgentCreateAsync(request);
                await _repository.SaveChangesAsync();

                var uri = new Uri($"{Request.Path}/{agent.AgentID}", UriKind.Relative);
                return Ok(agent);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }


        [HttpDelete("{agentId}")]
        public async Task<ActionResult> DeleteAgent(string agentId)
        {
            try
            {
                var agent = await _repository.AgentDeleteAsync(agentId);

                if (agent is null)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
