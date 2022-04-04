using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public Task<InsuredClient> CustomerLoginAsync(string agentId, string customerId, string password)
        {
            return _db.InsuredClients
                      .Where(x => x.ApiId == $"{agentId}/{customerId}"
                               && x.ApiPassword == password
                               && x.ApiStatus == "ENABLED").SingleOrDefaultAsync();
        }

        public async Task<InsuredClient> CustomerCreateAsync(CreateNewCustomerRequest newCustomerDto, string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
                throw new ArgumentNullException(nameof(agentId), "The agent id is required.");

            //check for duplicate
            var duplicate = await CustomerSelectThisAsync(newCustomerDto.PhoneLine1);

            if (duplicate != null)
                throw new ArgumentException("Customer already exists");

            var newInsured = new InsuredClient
            {
                InsuredID = GetNextAutoNumber("[AUTO]", "INSURED", BRANCH_ID),

                Address = newCustomerDto.Address,
                Email = newCustomerDto.Email,
                FirstName = newCustomerDto.FirstName,
                Surname = newCustomerDto.LastName,
                //FullName = policyDto.Title + " " + policyDto.LastName + " " + policyDto.FirstName + " " + policyDto.LastName,
                //DOB = policyDto.DateOfBirth,
                //InsuredType = policyDto.InsuredType,
                LandPhone = newCustomerDto.PhoneLine2,
                MobilePhone = newCustomerDto.PhoneLine1,
                //MeansID = policyDto.Identification,
                //MeansIDNo = policyDto.IdentificationNo,
                Occupation = newCustomerDto.Industry,
                OtherNames = newCustomerDto.OtherName,
                //Profile = policyDto.RiskProfiling,
                SubmittedBy = SUBMITTED_BY,
                SubmittedOn = DateTime.Now,
                Active = 1,
                Deleted = 0,

                ApiId = $"{agentId}/{newCustomerDto.PhoneLine1}",
                ApiPassword = newCustomerDto.Password,
                ApiStatus = "ACTIVE", 
            };

            _db.InsuredClients.Add(newInsured);
            return newInsured;
        }

        public Task<InsuredClient> CustomerSelectThisAsync(string customerID)
        {
            if (string.IsNullOrWhiteSpace(customerID))
                throw new ArgumentNullException(nameof(customerID));

            return _db.InsuredClients.Where(x => x.InsuredID == customerID).SingleOrDefaultAsync();
        }

        public Task<List<InsuredClient>> CustomerSelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.InsuredClients.AsQueryable();

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.Surname.Contains(item) || x.FirstName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.Surname)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }
    }
}
