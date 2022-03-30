using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using Universal.Api.Contracts;
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
        public ActionResult<LoginResult> CustomerLogin(LoginRequest loginRequest, string agentId)
        {
            if (loginRequest == null)
                return BadRequest("request body is null");

            try
            {
                var insured = _repository.AuthenticateCustomer(agentId, loginRequest.id, loginRequest.password);

                if (insured == null)
                    return BadRequest("ID or password is incorrect.");

                string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn,
                                           loginRequest.id, "Customer");

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
            [FromQuery] string searchText, [FromQuery] int pageNo, [FromQuery] int pageSize)
        {
            try
            {
                var customers = await _repository.CustomerSelectAsync(searchText, pageNo, pageSize);
                return Ok(customers.Select(a => new CustomerResult(a)).ToList());
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
        public ActionResult<CustomerResult> GetCustomer(string customerId)
        {
            try
            {
                var customer = _repository.CustomerSelectThis(customerId);

                if (customer is null)
                {
                    return NotFound();
                }

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
        [HttpPost]
        public ActionResult<CustomerResult> Post(CreateNewCustomerRequest customerDetails)
        {
            try
            {
                var customer = _repository.CreateNewInsured(customerDetails, GetCurrUserId());
                _repository.SaveChanges();

                var uri = new Uri($"{Request.Path}/{ customer.InsuredID}", UriKind.Relative);
                return Created(uri, new CustomerResult(customer));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
