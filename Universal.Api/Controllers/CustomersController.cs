﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Universal.Api.Contracts.V1;
using Universal.Api.Data.Repositories;
using Universal.Api.Data;

namespace Universal.Api.Controllers
{
    [Authorize(Roles = "APP,AGENT")]
    public class CustomersController : SecureControllerBase
    {
        private readonly Settings _settings;
        public CustomersController(Repository repository, AuthContext authContext, Settings settings) : base(repository, authContext)
        {
            _settings = settings;
        }

        /// <summary>
        /// Customers should Login using this endpoint. It creates a JWT Token that can be used to access secured endpoints of the API.
        /// </summary>
        /// <returns>A JWT Token and it's expiry time.</returns>
        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult<LoginResult>> CustomerLogin(CustomerLoginRequest login)
        {
            if (login is null)
                return BadRequest("Request body is null");

            try
            {
                var insured = await _repository.CustomerSelectThisAsync(login.AppID, login.CustomerID, login.Password);

                if (insured is null)
                    return NotFound("ID or Password is incorrect");

                else if (insured.ApiStatus != "ENABLED")
                    return Unauthorized("This Customer has not been activated");

                string token = CreateToken(_settings.JwtSecret,
                                           _settings.JwtExpiresIn,
                                           login.AppID, 
                                           login.CustomerID, 
                                           insured.InsuredID, "CUST");
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
        /// <returns>A collection of Customers.</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CustomerResult>>> ListCustomers([FromQuery] FilterPaging filter)
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
        /// Fetch a single Customer using either Email, Phone, or Universal CustomerId.
        /// </summary>
        /// <param name="customerId">Email, Phone or Id of the Customer to get.</param>
        /// <returns>The Customer with the Email, Phone or Id supplied.</returns>
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

        ///// <summary>
        ///// Fetch Customers using a combination of last name(surname) and first name.
        ///// </summary>
        ///// <param name="email">Email of the Customers to get.</param>
        ///// <param name="phone">Phone number of the Customers to get.</param>
        ///// <returns>The Customer with the details supplied.</returns>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CustomerResult>>> ListCustomers([FromQuery] string email, [FromQuery] string phone)
        //{
        //    try
        //    {
        //        var customers = await _repository.CustomerSelectThisAsync(email, phone);

        //        return Ok(customers.Select(x => new CustomerResult(x)).ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        return ExceptionResult(ex);
        //    }
        //}

        /// <summary>
        /// Create a Customer.
        /// </summary>
        /// <returns>A newly created Customer</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerResult>> CreateCustomer(CreateNewCustomerRequest request)
        {
            try
            {
                var customer = _repository.CustomerCreate(request);
                await _repository.SaveChangesAsync();

                var uri = new Uri($"{Request.Path}/{customer.ApiId}", UriKind.Relative);
                return Created(uri, new CustomerResult(customer));
            }
            catch (Exception ex)
            {
                return ExceptionResult(ex);
            }
        }
    }
}
