using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Contracts;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;

namespace Universal.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Agent")]
    public class CustomersController : SecureControllerBase
    {
        private readonly Settings _settings;
        public CustomersController(Repository repository, Settings settings) : base(repository)
        {
            _settings = settings;
        }

        [HttpPost("Login/{agentId}"), AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginResponse> CustomerLogin(LoginRequest loginRequest, string agentId)
        {
            try
            {
                if (loginRequest == null)
                    return Problem("request body is null", statusCode: 400);

                try
                {
                    var validUser = _repository.AuthenticateCustomer(agentId, loginRequest.id, loginRequest.password);
                    if (validUser == null)
                        return Problem("SID or password is incorrect.", statusCode: 400);

                    string token = CreateToken(_settings.JwtSecret,
                                               _settings.JwtExpiresIn, 
                                               loginRequest.id, "Customer");

                    var response = new LoginResponse
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
        /// Create a customer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Customer
        ///     {
        ///         "agentId": "string",
        ///         "firstName": "string",
        ///         "lastName": "string",
        ///         "otherName": "string",
        ///         "address": "string",
        ///         "mobilePhone": "string",
        ///         "telephone": "string",
        ///         "email": "string",
        ///         "industry": "string",
        ///         "stateOfOrigin": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="customerDetails"></param>
        /// <returns>A newly created customer</returns>
        [HttpPost]
        public ActionResult<CustomerDto> Post(CustomerDto customerDetails)
        {
            try
            {
                var customer = _repository.CreateNewInsured(customerDetails, GetCurrUserId());
                _repository.SaveChanges();
                var uri = new Uri($"{Request.Path}/{ customerDetails.CustomerId}", UriKind.Relative);

                return Created(uri, new CustomerDto(customer));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
