using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : SecureControllerBase
    {
        public CustomersController(IRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// Fetch a collection of Customers.
        /// </summary>
        /// <returns>A collection of customers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> ListCustomersAsync(
            [FromQuery] string searchText, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var customers = await _repository.CustomerSelectAsync(searchText, pageNo, pageSize);
                return Ok(customers.Select(a => new CustomerDto(a)).ToList());
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Fetch a single customer.
        /// </summary>
        /// <param name="customerId">Id of the customer to get.</param>
        /// <returns>The customer with the Id entered.</returns>
        [HttpGet("{customerId}")]
        public ActionResult<CustomerDto> GetCustomer(string customerId)
        {
            try
            {
                var customer = _repository.CustomerSelectThis(customerId);
                if (customer is null)
                {
                    return NotFound();
                }

                return Ok(new CustomerDto(customer));
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
        ///     POST /Customer
        ///     {
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "otherName": "string",
        ///         "address": "string",
        ///         "mobilePhone": "string",
        ///         "telephone": "string",
        ///         "email": "string",
        ///         "industry": "string",
        ///     }
        ///
        /// </remarks>
        /// <param name="customerDetails"></param>
        /// <returns>A newly created customer</returns>
        [HttpPost]
        public ActionResult<AgentDto> Post(AgentDto customerDetails)
        {
            try
            {
                throw new NotImplementedException();
                //agentDetails.AgentID = _repository.PartyCreate(customerDetails);
                //var uri = new Uri($"{Request.Path}/{ customerDetails.AgentID}", UriKind.Relative);

                //return Created(uri, customerDetails);
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
