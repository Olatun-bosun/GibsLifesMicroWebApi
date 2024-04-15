using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GibsLifesMicroWebApi.Contracts.V1;
using GibsLifesMicroWebApi.Models;

namespace GibsLifesMicroWebApi.Data.Repositories
{
    public partial class Repository
    {
        public Task<InsuredClient> CustomerSelectThisAsync(string appId, string customerId, string password)
        {
            return _db.InsuredClients.FirstOrDefaultAsync(x => 
                                  (x.InsuredID == customerId || x.Email == customerId)
                                //&& x.ApiPassword == password
                                && x.SubmittedBy == $"{SUBMITTED_BY}/{appId}");
        }

        public Task<InsuredClient> CustomerSelectThisAsync(string email, string mobile)
        {
            var query = _db.InsuredClients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(email))
                return query.FirstOrDefaultAsync(x => x.Email == email);

            if (!string.IsNullOrWhiteSpace(mobile))
                return query.FirstOrDefaultAsync(x => x.MobilePhone == mobile || 
                                                      x.LandPhone   == mobile);

            throw new ArgumentException("Please enter a valid email or phone number");
        }

        public Task<InsuredClient> CustomerSelectThisAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentNullException(nameof(customerId));

            if (customerId.IsValidEmail())
                return _db.InsuredClients.FirstOrDefaultAsync(x => x.Email == customerId);

            if (customerId.IsNumeric())
                return _db.InsuredClients.FirstOrDefaultAsync(x => x.MobilePhone == customerId 
                                                                || x.InsuredID   == customerId);

            return _db.InsuredClients.FirstOrDefaultAsync(x => x.InsuredID == customerId);
        }

        public async Task<InsuredClient> CustomerGetOrAddAsync<T>(CreateNew<T> newPolicyDto) 
            where T : RiskDetail
        {
            if (!string.IsNullOrWhiteSpace(newPolicyDto.CustomerID))
            {
                var insured = await CustomerSelectThisAsync(newPolicyDto.CustomerID);

                if (insured != null)
                    return insured;
            }

            if (newPolicyDto.Insured != null)
                return await CustomerCreate(newPolicyDto.Insured);

            throw new InvalidOperationException("Either you specify CustomerID for existing customer, " +
                "or you create a new customer by populating the Insured object");
        }

        public async Task<InsuredClient> CustomerCreate(CreateNewCustomerRequest newCustomerDto)
        {
            //int incNumber = 500;
            //++incNumber;

            //string nyNumber = "S" + incNumber.ToString("00");

            ////check for duplicate
            //var duplicate = await CustomerSelectThisAsync(newCustomerDto.Email, newCustomerDto.PhoneLine1);

            //if (duplicate != null)
            //    throw new ArgumentException("Customer already exists with this email or phone number");

            var newInsured = new InsuredClient
            {
                InsuredID = GetNextSerialNo("INSURED", BRANCH_ID).ToString(),

                Address = newCustomerDto.Address,
                Email = newCustomerDto.Email,
                FirstName = newCustomerDto.FirstName,
                Surname = newCustomerDto.LastName,
                MobilePhone = newCustomerDto.PhoneLine1,
                LandPhone = newCustomerDto.PhoneLine2,
                //Occupation = newCustomerDto.Industry,
                OtherNames = newCustomerDto.OtherName,  

                Remarks = newCustomerDto.KycNumber,

                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                Active = 1,
                Deleted = 0,
                TransID = true,
                //ApiId = newCustomerDto.Email,
                //ApiPassword = newCustomerDto.Password,
                //ApiStatus = "ENABLED", 
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
                        //.Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }
    }
}
