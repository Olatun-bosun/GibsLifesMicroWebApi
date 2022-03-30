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

        public Party PartyCreate(CreateNewAgentRequest agentDto)
        {
            //check for duplicate
            var foundAgent = _db.Parties.Where(p => p.PartyID == agentDto.PhoneLine1 || p.Email == agentDto.Email).FirstOrDefault();
            if (foundAgent != null)
                throw new ArgumentException("Duplicate agent found");

            Party agent = new Party()
            {
                Party1 = agentDto.AgentName,
                Address = agentDto.Address,
                LandPhone = agentDto.PhoneLine2,
                mobilePhone = agentDto.PhoneLine1,
                Email = agentDto.Email,
                //ComRate = agentDto.CommissionRate,
                //CreditLimit = agentDto.CreditLimit,
                PartyType = "AG",
                InsContact = agentDto.InsuranceContact,
                //FinContact = agentDto.FinancialContact,
                Remarks = agentDto.Remarks,
                ApiId = agentDto.Email,
                ApiPassword = "password",
                ApiStatus = "PENDING"
            };
            _db.Parties.Add(agent);
            return agent;
        }
    }
}
