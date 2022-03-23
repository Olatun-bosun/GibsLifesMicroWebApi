using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {

        public InsuredClient AuthenticateCustomer(string agentId, string customerId, string password)
        {
            return _db.InsuredClients
                        .Where(a => a.ApiId == $"{agentId}/{customerId}"
                                && a.ApiPassword == password
                                && a.ApiStatus == "ENABLED").SingleOrDefault();
        }

        public InsuredClient CreateNewInsured(CreateCustomerDto customerDto, string agentId)
        {
            //check for duplicate
            var foundInsured = CustomerSelectThis(customerDto.PhoneLine1);
            if (foundInsured != null)
                throw new ArgumentException("Customer already exists.");

            var newInsured = new InsuredClient
            {
                InsuredID = Guid.NewGuid().ToString().Split('-')[0].ToUpper(),
                Address = customerDto.Address,
                Email = customerDto.Email,
                FirstName = customerDto.FirstName,
                //FullName = policyDto.Title + " " + policyDto.LastName + " " + policyDto.FirstName + " " + policyDto.LastName,
                //DOB = policyDto.DateOfBirth,
                //InsuredType = policyDto.InsuredType,
                LandPhone = customerDto.PhoneLine2,
                MobilePhone = customerDto.PhoneLine1,
                //MeansID = policyDto.Identification,
                //MeansIDNo = policyDto.IdentificationNo,
                Occupation = customerDto.Industry,
                OtherNames = customerDto.OtherName,
                //Profile = policyDto.RiskProfiling,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                Surname = customerDto.LastName,
                Active = 1,
                Deleted = 0,

                ApiId = $"{agentId}/{customerDto.PhoneLine1}",
                ApiPassword = "password",
                ApiStatus = "ACTIVE",
            };

            _db.InsuredClients.Add(newInsured);
            return newInsured;
        }

        public InsuredClient CustomerSelectThis(string customerID)
        {
            if (string.IsNullOrWhiteSpace(customerID))
                throw new ArgumentNullException(nameof(customerID), "Customer ID cannot be empty.");

            var insuredClient = _db.InsuredClients.Where(I => I.InsuredID == customerID).SingleOrDefault();

            if (insuredClient != null)
                return insuredClient;

            throw new KeyNotFoundException("Customer ID does not exist.");
        }

        public async Task<List<InsuredClient>> CustomerSelectAsync(string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.InsuredClients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(C => C.Surname.Contains(searchText) || C.FirstName.Contains(searchText));
            }

            var skipCount = (pageNo - 1) * pageSize;
            var customers = await query.OrderBy(o => o.Surname).Skip(skipCount).Take(pageSize).ToListAsync();
            return customers;
        }
    }
}
