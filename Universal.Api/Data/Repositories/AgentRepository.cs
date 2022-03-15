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
        public Party AuthenticateAgent(string apiId, string password)
        {
            return _db.Parties
                        .Where(a => a.ApiId == apiId
                                && a.ApiPassword == password
                                && a.ApiStatus == "ENABLED").SingleOrDefault();
        }

        public async Task<List<Party>> PartySelectAsync(string partyId, string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.Parties.Where(p => p.PartyID == partyId);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                char[] chArray = new char[1] { ' ' };
                foreach (string A in searchText.Split(chArray))
                {
                    query = query.Where(O => O.Party1.Contains(A)).AsQueryable();
                }
            }

            var agents = await query.OrderBy(o => o.Party1).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            return agents;
        }

        public Party PartySelectThis(string agentID)
        {
            if (string.IsNullOrWhiteSpace(agentID))
                throw new ArgumentNullException("Agent ID cannot be empty ", "AgentID");

            var party = _db.Parties.Where(O => O.PartyID == agentID).SingleOrDefault();

            if (party != null)
                return party;

            throw new KeyNotFoundException("Agent ID does not exist");
        }

        public Party PartyCreate(AgentDto agentDto)
        {
            if (string.IsNullOrWhiteSpace(agentDto.AgentID))
                throw new ArgumentNullException("Agent Id is required.", nameof(agentDto.AgentID));
            if (agentDto.CommRate <= decimal.Zero)
                throw new ArgumentException("Comm Rate cannot be less than or equal to zero ", nameof(agentDto.CommRate));

            if (agentDto.CreditLimit <= decimal.Zero)
                throw new ArgumentException("Credit Limit cannot be less than or equal to zero ", nameof(agentDto.CreditLimit));

            Party agent = new Party()
            {
                Party1 = agentDto.AgentName,
                Address = agentDto.Address,
                LandPhone = agentDto.Telephone,
                mobilePhone = agentDto.MobilePhone,
                Email = agentDto.Email,
                ComRate = agentDto.CommRate,
                CreditLimit = agentDto.CreditLimit,
                PartyType = "AG",
                InsContact = agentDto.InsContact,
                FinContact = agentDto.FinContact,
                Remarks = agentDto.Remarks,
                ApiId = agentDto.AgentID,
                ApiPassword = "password",
                ApiStatus = "ENABLED"
            };
            _db.Parties.Add(agent);
            _db.SaveChanges();

            return agent;
        }
    }
}
