using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Universal.Api.Contracts.V1;
using Universal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public Task<Party> PartyLoginOrNullAsync(string apiId, string password)
        {
            return _db.Parties
                      .Where(x => x.ApiId == apiId
                               && x.ApiPassword == password
                               && x.ApiStatus == "ENABLED").SingleOrDefaultAsync();
        }

        public Task<List<Party>> PartySelectAsync(string partyId, FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Parties.Where(x => x.PartyID == partyId);

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.PartyName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.PartyName)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public Task<Party> PartySelectThisOrNullAsync(string agentID)
        {
            if (string.IsNullOrWhiteSpace(agentID))
                throw new ArgumentNullException("Agent ID cannot be empty ", "AgentID");

            return _db.Parties.Where(x => x.PartyID == agentID).SingleOrDefaultAsync();
        }

        public async Task<Party> PartyCreateAsync(CreateNewAgentRequest newAgentDto)
        {
            //check for duplicate
            var existing = await _db.Parties.Where(x => x.PartyID == newAgentDto.PhoneLine1 || 
                                                        x.Email   == newAgentDto.Email).FirstOrDefaultAsync();
            if (existing != null)
                throw new ArgumentException("Duplicate agent found");

            Party agent = new Party()
            {
                PartyID = "DEMO-" + Guid.NewGuid().ToString().ToUpper(),

                PartyName = newAgentDto.AgentName,
                Address = newAgentDto.Address,
                LandPhone = newAgentDto.PhoneLine2,
                mobilePhone = newAgentDto.PhoneLine1,
                Email = newAgentDto.Email,
                //ComRate = agentDto.CommissionRate,
                //CreditLimit = agentDto.CreditLimit,
                PartyType = "AG",
                //InsContact = agentDto.InsuranceContact,
                //FinContact = agentDto.FinancialContact,
                Remarks = newAgentDto.Remarks,
                ApiId = newAgentDto.Email,
                ApiPassword = newAgentDto.Password,
                ApiStatus = "PENDING"
            };

            _db.Parties.Add(agent);
            return agent;
        }
    }
}
