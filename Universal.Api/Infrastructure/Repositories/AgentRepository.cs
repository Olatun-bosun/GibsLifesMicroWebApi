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
        public Task<Party> PartySelectThisAsync(string appId, string agentId, string password)
        {
            return _db.Parties.FirstOrDefaultAsync(x => 
                                  (x.PartyID == agentId || x.Email == agentId)
                                && x.ApiPassword == password
                                && x.SubmittedBy == $"{SUBMITTED_BY}/{appId}");
        }

        public Task<Party> PartySelectThisAsync(string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
                throw new ArgumentNullException("Agent ID cannot be empty", "AgentID");

            return _db.Parties.FirstOrDefaultAsync(x => x.PartyID == agentId || 
                                                        x.Email   == agentId);
        }

        public Task<List<Party>> PartySelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Parties.Where(x => x.Active == 1);

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.PartyName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.PartyName)
                        //.Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public async Task<Party> PartyCreateAsync(CreateNewAgentRequest dto)
        {
            //check for duplicate
            var existing = await _db.Parties.FirstOrDefaultAsync(x => x.ApiId.Contains(dto.Email) ||
                                                                      x.Email       == dto.Email  ||
                                                                      x.mobilePhone == dto.PhoneLine1);
            if (existing != null)
                throw new ArgumentException($"Duplicate agent found. ID={existing.PartyID} {existing.Email}, {existing.mobilePhone}");

            var party = new Party()
            {
                PartyID = GetNextAutoNumber("AGENTS", BRANCH_ID),

                PartyName = dto.AgentName,
                FirstName = dto.FirstName,
                OtherName = dto.OtherName,
                Address = dto.Address,
                LandPhone = dto.PhoneLine2,
                mobilePhone = dto.PhoneLine1,
                Email = dto.Email,
                ComRate = 20,
                CreditLimit = 0,
                PartyType = "AG",
                InsContact = null,
                FinContact = null,
                //Remarks = newAgentDto.Remarks,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                Active = 0,
                Deleted = 0,

                ApiId = dto.Email,
                ApiPassword = dto.Password,
                ApiStatus = "PENDING",
            };

            _db.Parties.Add(party);
            return party;
        }
    }
}
