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
        public Task<List<Claim>> SelectClaimsAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.ClaimsReserved.Where(x => x.Deleted == 0);

            if (_authContext.User is AppUser u)
                query = query.Where(x => x.SubmittedBy == $"{SUBMITTED_BY}/{u.AppId}");

            else if (_authContext.User is AgentUser a)
                query = query.Where(x => x.PartyID == a.PartyId);

            else if (_authContext.User is CustomerUser c)
                query = query.Where(x => x.InsuredID == c.InsuredId);

            if (filter.CanSearchDate)
            {
                query = query.Where(x => (x.EntryDate >= filter.DateFrom) &&
                                         (x.EntryDate <= filter.DateTo));
            }

            return query.OrderByDescending(x => x.EntryDate)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public Task<Claim> ClaimSelectThisAsync(string claimNo)
        {
            if (string.IsNullOrWhiteSpace(claimNo))
                throw new ArgumentNullException("Claim No cannot be empty ", nameof(claimNo));

            return _db.ClaimsReserved.Where(x => x.ClaimNo == claimNo).SingleOrDefaultAsync();
        }

        public async Task<Claim> ClaimCreateAsync(ClaimResult claimDto)
        {
            var policy = await PolicySelectThisAsync(claimDto.PolicyNo);

            if (policy is null)
                throw new KeyNotFoundException("Policy No you supplied is invalid");

            var claim = new Claim
            {
                //NotificatnNo = GenerateNotificationNo(policy.SubRiskID, policy.BranchID),
                PolicyNo = claimDto.PolicyNo,
                NotifyDate = claimDto.LossNotifyDate,
                LossDate = claimDto.LossDate,
                LossDetails = claimDto.LossDescription,
                //InsuredName = policy.InsFullName,
                BranchID = policy.BranchID,
                //SumInsured = policy.SumInsured,
                InsuredID = policy.InsuredID,
                //RegStatus = "PENDING",
                Approval = 0,
                Active = 1,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1).AddDays(-1.0),
                EntryDate = DateTime.Now,
                Party = policy.Party,
                SubRisk = policy.SubRisk,
                //Premium = policy.GrossPremium
            };

            _db.ClaimsReserved.Add(claim);
            return claim;
        }
    }
}
