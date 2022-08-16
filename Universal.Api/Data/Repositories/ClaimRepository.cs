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
                        //.Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }

        public Task<Claim> ClaimSelectThisAsync(string claimNo)
        {
            if (string.IsNullOrWhiteSpace(claimNo))
                throw new ArgumentNullException("Claim No cannot be empty ", nameof(claimNo));

            return _db.ClaimsReserved.Where(x => x.ClaimNo == claimNo).SingleOrDefaultAsync();
        }

        public async Task<Claim> ClaimCreateAsync(CreateNewClaimRequest claimDto)
        {
            var policy = await PolicySelectThisAsync(claimDto.PolicyNo);

            if (policy is null)
                throw new KeyNotFoundException("Policy No you supplied is invalid");

            var claim = new Claim
            {
                ClaimNo = GetNextAutoNumber("CLAIM", policy.SubRiskID, policy.BranchID),
                BranchID = policy.BranchID,
                PolicyNo = claimDto.PolicyNo,
                SubRiskID = policy.SubRiskID,
                SubRisk = policy.SubRisk,
                PartyID = policy.PartyID,
                Party = policy.Party,
                //refDNCNNo = claimDto.debitNoteNo, //TODO
                InsuredID = policy.InsuredID,
                InsuredName = policy.InsFullname,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,

                LossType = claimDto.LossType,
                LossDate = claimDto.LossDate,
                LossDetails = claimDto.LossDescription,
                NotifyDate = claimDto.LossNotifyDate,

                PropRate = (decimal?)policy.ProportionRate,
                UndYear = policy.StartDate.Value.Year,
                AmtReserved = 1000, //TODO
                AmtPaid = 0,

                //vehicle/marine/other info
                //Field1 = "",
                //Field2 = "",
                //Field3 = "",
                //Field4 = "",
                //Field5 = "",
                //Field6 = "",
                //Field7 = "",
                //Field8 = "",
                //Field9 = "",
                //Field10 = "",
                //Field11 = "",
                //Field12 = "",

                EntryDate = DateTime.Now,
                SubmittedBy = $"{SUBMITTED_BY}/{_authContext.User.AppId}",
                SubmittedOn = DateTime.Now,
                Approval = 0,
                Active = 1,
            };

            _db.ClaimsReserved.Add(claim);
            return claim;
        }
    }
}
