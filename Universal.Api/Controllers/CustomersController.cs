using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Authorize(Roles = "Agent")]
    public class CustomersController : SecureControllerBase
    {
        private readonly Settings _settings;
        public CustomersController(Repository repository, Settings settings) : base(repository)
        {
            _settings = settings;
        }

        [HttpPost("Login/{agentId}"), AllowAnonymous]
        public async Task<ActionResult<LoginResult>> CustomerLogin(LoginRequest loginRequest, string agentId)
        {
            if (loginRequest is null)
                return BadRequest("Request body is null");

            try
            {
                var insured = await _repository.CustomerLoginAsync(agentId, loginRequest.Id, loginRequest.Password);

                if (insured is null)
                    return BadRequest("ID or Password is incorrect");

                string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn,
                                           loginRequest.Id, "Customer");

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
        /// Fetch a collection of Customers.
        /// </summary>
        /// <returns>A collection of customers.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResult>>> ListCustomersAsync(
            [FromQuery] FilterPaging filter)
        {
            try
            {
                var customers = await _repository.CustomerSelectAsync(filter);
                return Ok(customers.Select(x => new CustomerResult(x)).ToList());
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
        public async Task<ActionResult<CustomerResult>> GetCustomer(string customerId)
        {
            try
            {
                var customer = await _repository.CustomerSelectThisAsync(customerId);

                if (customer is null)
                    return NotFound();

                return Ok(new CustomerResult(customer));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }

        /// <summary>
        /// Create a customer.
        /// </summary>
        /// <returns>A newly created customer</returns>
        [HttpPost("{agentId}"), AllowAnonymous]
        public async Task<ActionResult<CustomerResult>> Post(CreateNewCustomerRequest request, string agentId)
        {
            try
            {
                var customer = await _repository.CustomerCreateAsync(request, agentId);
                _repository.SaveChanges();

                var uri = new Uri($"{Request.Path}/{customer.InsuredID}", UriKind.Relative);
                return Created(uri, new CustomerResult(customer));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
