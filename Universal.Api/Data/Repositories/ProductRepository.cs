using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Universal.Api.Models;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public SubRisk SubRiskSelectThis(string subRiskID)
        {
            if (string.IsNullOrWhiteSpace(subRiskID))
                throw new ArgumentNullException("SubRisk ID cannot be empty ", nameof(subRiskID));

            var subRisk = _db.SubRisks.Where(O => O.SubRiskID == subRiskID).SingleOrDefault();

            if (subRisk != null)
                return subRisk;

            throw new KeyNotFoundException("SubRisk ID does not exist");
        }

        public async Task<List<SubRisk>> SubRisksSelectAsync(string searchText, int pageNo, int pageSize)
        {
            if (pageNo <= 0)
                pageNo = 1;
            if (pageSize <= 0)
                pageSize = 25;

            var query = _db.SubRisks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                char[] chArray = new char[1] { ' ' };
                foreach (string A in searchText.Split(chArray))
                {
                    query = query.Where(O => O.SubRiskName.Contains(A));
                }
            }

            var subRisks = await query.OrderBy(o => o.SubRiskName).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            return subRisks;
        }
    }
}
