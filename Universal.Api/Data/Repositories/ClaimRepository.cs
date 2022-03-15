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
        public async Task<List<Claim>> ClaimsSelectAsync(FilterPaging filter, string partyId)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.ClaimsReserved.Where(c => c.PartyID == partyId);
            if (filter.CanSearchDate)
            {
                query = query.Where(x => (x.EntryDate >= filter.DateFrom) &&
                                         (x.EntryDate <= filter.DateTo));
            }

            var claims = await query.OrderByDescending(c => c.EntryDate)
                                    .Skip(filter.SkipCount)
                                    .Take(filter.PageSize)
                                    .ToListAsync();
            return claims;
        }

        public Claim ClaimSelectThis(string claimNo)
        {
            if (string.IsNullOrWhiteSpace(claimNo))
                throw new ArgumentNullException("Claim No cannot be empty ", nameof(claimNo));

            return _db.ClaimsReserved.Where(O => O.ClaimNo == claimNo).SingleOrDefault();
        }

        public Claim ClaimCreate(ClaimDto claimDto)
        {
            Policy policy = PolicySelectThis(claimDto.PolicyNo);

            if (policy == null)
                throw new KeyNotFoundException("Policy No you supplied is invalid");

            var claim = new Claim
            {
                //NotificatnNo = GenerateNotificationNo(policy.SubRiskID, policy.BranchID),
                PolicyNo = claimDto.PolicyNo,
                NotifyDate = claimDto.NotifyDate.ToUniversalTime(),
                LossDate = claimDto.LossDate.ToUniversalTime(),
                LossDetails = claimDto.LossDetails,
                //InsuredName = policy.InsFullName,
                BranchID = policy.BranchID,
                //SumInsured = policy.SumInsured,
                InsuredID = policy.InsuredID,
                //RegStatus = "PENDING",
                Approval = 0,
                Active = 1,
                SubmittedBy = "E-CHANNEL",
                SubmittedOn = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1).AddDays(-1.0),
                EntryDate = DateTime.Now,
                Party = policy.Party,
                SubRisk = policy.SubRisk,
                //Premium = policy.GrossPremium
            };
            _db.ClaimsReserved.Add(claim);
            //_db.SaveChanges();
            return claim;
        }
    }
}
