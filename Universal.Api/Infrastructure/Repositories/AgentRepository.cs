using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GibsLifesMicroWebApi.Contracts.V1;
using GibsLifesMicroWebApi.Models;
using GibsLifesMicroWebApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GibsLifesMicroWebApi.Data.Repositories
{
    public partial class Repository
    {
        //public Task<Party?> PartyLoginAsync(string appId, string agentId, string password)
        //{
        //    return _db.Parties.FirstOrDefaultAsync(x => 
        //                          (x.PartyID == agentId || x.Email == agentId)
        //                        && x.ApiPassword == password
        //                        && x.SubmittedBy == $"{SUBMITTED_BY}/{appId}");
        //}

        public Task<Agents> AgentSelectThisAsync([FromRoute] string agentId)
        {
            string appId = _authContext.User.AppId;

            if (string.IsNullOrWhiteSpace(agentId))
                throw new ArgumentNullException(nameof(agentId), "Agent ID cannot be empty");

            return _db.Agents.Where(x => x.AgentID == agentId || 
                                          x.Email   == agentId)
                              //.Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{appId}")
                              .FirstOrDefaultAsync();
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

        public async Task<Agents> AgentCreateAsync(CreateNewAgentRequest dto)
        {
            string appId = _authContext.User.AppId;

            //check for duplicate
            var existing = await _db.Agents.Where(x =>/* x.ApiId.Contains(dto.Email) ||*/
                                                        x.Email == dto.Email ||
                                                        x.Phone1 == dto.Phone1)
                                            .Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{appId}")
                                            .FirstOrDefaultAsync();
            if (existing != null)
                throw new ArgumentException($"Duplicate agent found. ID={existing.AgentID} {existing.Email}, {existing.Phone1}");

            var agent = new Agents()
            {
                AgentID = GetNextAutoNumber("AGENTS", BRANCH_ID),

                 UnitID= dto.      UnitID,
                 Agent = dto.       Agent,      
                 Address= dto.     Address,    
                 Area= dto.        Area,       
                 City= dto.        City,       
                 State= dto.       State,      
                 Phone1= dto.      Phone1,     
                 Phone2= dto.      Phone2,     
                 FaxNo= dto.       FaxNo,      
                 Email= dto.       Email,      
                 InsPerson= dto.   InsPerson,  
                 FinPerson= dto.   FinPerson,  
                 Balance= dto.     Balance,    
                 CreditLimit= dto. CreditLimit,
                 ComRate= dto.     ComRate,    
                 Remark= dto.      Remark,     
                 AccountNo= dto.   AccountNo,  
                 Bankname= dto.    Bankname,   
                 Tag= dto.         Tag,        
                 Deleted= 0,    
                 SubmittedBy= dto. SubmittedBy,
                 SubmittedOn= DateTime.Now,
                 ModifiedBy= dto.  ModifiedBy, 
                 ModifiedOn= DateTime.Now, 


            };

            _db.Agents.Add(agent);
            return agent;
        }

        public async Task<Agents?> AgentDeleteAsync(string agentId)
        {
            string appId = _authContext.User.AppId;

            if (string.IsNullOrWhiteSpace(agentId))
                throw new ArgumentNullException(nameof(agentId), "Agent ID cannot be empty");

            var agent = await _db.Agents.Where(x => x.AgentID == agentId ||
                                                     x.Email   == agentId)
                              //.Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{appId}")
                              .FirstOrDefaultAsync();
            if (agent == null)
                return null;

            _db.Agents.Remove(agent);
            _db.SaveChanges();
            return agent;
        }

    }
}
