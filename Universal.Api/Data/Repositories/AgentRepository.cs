﻿using System;
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
            return _db.Parties
                      .Where(x => (x.PartyID == agentId || x.Email == agentId)
                                && x.ApiPassword == password
                                && x.SubmittedBy == $"{SUBMITTED_BY}/{appId}").SingleOrDefaultAsync();
        }

        public Task<Party> PartySelectThisAsync(string agentId)
        {
            if (string.IsNullOrWhiteSpace(agentId))
                throw new ArgumentNullException("Agent ID cannot be empty", "AgentID");

            return _db.Parties.Where(x => x.PartyID == agentId || 
                                          x.Email   == agentId).SingleOrDefaultAsync();
        }

        public Task<List<Party>> PartySelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.Parties.Where(x => x.Active == 1);

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.PartyName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.PartyName)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public async Task<Party> PartyCreateAsync(CreateNewAgentRequest newAgentDto)
        {
            //check for duplicate
            var existing = await _db.Parties.Where(x => x.ApiId.Contains(newAgentDto.Email) ||
                                                        x.Email       == newAgentDto.Email  ||
                                                        x.mobilePhone == newAgentDto.PhoneLine1).FirstOrDefaultAsync();
            if (existing != null)
                throw new ArgumentException("Duplicate agent found");

            Party party = new Party()
            {
                PartyID = GetNextAutoNumber("AGENTS", BRANCH_ID),

                PartyName = newAgentDto.AgentName,
                Address = newAgentDto.Address,
                LandPhone = newAgentDto.PhoneLine2,
                mobilePhone = newAgentDto.PhoneLine1,
                Email = newAgentDto.Email,
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

                ApiId = newAgentDto.Email,
                ApiPassword = newAgentDto.Password,
                ApiStatus = "PENDING",
            };

            _db.Parties.Add(party);
            return party;
        }
    }
}
