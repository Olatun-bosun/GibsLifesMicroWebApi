using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Universal.Api.Models;
using Universal.Api.Contracts.V1;

namespace Universal.Api.Data.Repositories
{
    public partial class Repository
    {
        public Task<SubRisk> ProductSelectThisAsync(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentNullException(nameof(productId));

            return _db.SubRisks.Where(x => x.SubRiskID == productId).SingleOrDefaultAsync();
        }

        public Task<List<SubRisk>> ProductSelectAsync(FilterPaging filter)
        {
            if (filter == null)
                filter = new FilterPaging();

            var query = _db.SubRisks.AsQueryable();

            foreach (string item in filter.SearchTextItems)
                query = query.Where(x => x.SubRiskName.Contains(item)).AsQueryable();

            return query.OrderBy(x => x.SubRiskName)
                        .Skip(filter.SkipCount)
                        .Take(filter.PageSize)
                        .ToListAsync();
        }
    }
}
