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
        public Task<InsuredClient> CustomerSelectThisAsync(string appId, string customerId, string password)
        {
            return _db.InsuredClients
                      .Where(x => (x.InsuredID == customerId || x.Email == customerId)
                                && x.ApiPassword == password
                                && x.SubmittedBy == $"{SUBMITTED_BY}/{appId}").SingleOrDefaultAsync();
        }

        public Task<InsuredClient> CustomerSelectThisAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentNullException(nameof(customerId));

            return _db.InsuredClients.Where(x => x.InsuredID == customerId).SingleOrDefaultAsync();
        }

        public async Task<InsuredClient> CustomerCreateAsync(CreateNewCustomerRequest newCustomerDto)
        {
            //check for duplicate
            var duplicate = await CustomerSelectThisAsync(newCustomerDto.Email);

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
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                Active = 1,
                Deleted = 0,

                ApiId = newCustomerDto.Email,
                ApiPassword = newCustomerDto.Password,
                ApiStatus = "ENABLED", 
            };

            _db.InsuredClients.Add(newInsured);
            return newInsured;
        }

        public Task<List<InsuredClient>> CustomerSelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.InsuredClients.AsQueryable();

            if (_authContext.User is AppUser u)
                query = query.Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{u.AppId}");

            //else if (_authContext.User is AgentUser a)
            //    query = query.Where(x => x.PartyID == a.PartyId);

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.Surname.Contains(item) || x.FirstName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.Surname)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }
    }
}
