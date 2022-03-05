using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Universal.Api.Data.Repositories;
using Universal.Api.Contracts.V1;
using Microsoft.AspNetCore.Authorization;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AgentsController : SecureControllerBase
    {
        public AgentsController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Fetch a collection of Agents.
        /// </summary>
        /// <returns>A collection of agents.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentDto>>> ListAgentsAsync(
            [FromQuery] string searchText, [FromQuery]int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var agents = await _repository.PartySelectAsync(searchText, pageNo, pageSize);
                return Ok(agents.Select(a => new AgentDto(a)).ToList());
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
        public ActionResult<AgentDto> GetAgent(string agentId)
        {
            try
            {
                var agent = _repository.PartySelectThis(agentId);
                if (agent is null)
                {
                    return NotFound();
                }

                return Ok(new AgentDto(agent));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create an Agent.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Agents
        ///     {
        ///        "agentID": "string",
        ///         "agentName": "string",
        ///         "address": "string",
        ///         "mobilePhone": "string",
        ///         "telephone": "string",
        ///         "email": "string",
        ///         "commRate": 0,
        ///         "creditLimit": 0,
        ///         "rpcNumber": "string",
        ///         "website": "string",
        ///         "insContact": "string",
        ///         "finContact": "string",
        ///         "remarks": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="agentDetails"></param>
        /// <returns>A newly created Agent</returns>
        [HttpPost]
        public ActionResult<AgentDto> Post(AgentDto agentDetails)
        {
            try
            {
                agentDetails.AgentID = _repository.PartyCreate(agentDetails);
                var uri = new Uri($"{Request.Path}/{ agentDetails.AgentID}", UriKind.Relative);

                return Created(uri, agentDetails);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
