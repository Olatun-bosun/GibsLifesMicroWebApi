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
                    query = query.Where(O => O.SubRisk1.Contains(A));
                }
            }

            var subRisks = await query.OrderBy(o => o.SubRisk1).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            return subRisks;
        }

        private void ProductUpdateIsActive(string SubRiskID, long? SerialNo)
        {
            if (string.IsNullOrWhiteSpace(SubRiskID))
                throw new ArgumentNullException("SubRisk ID cannot be empty ", nameof(SubRiskID));

            long? nullable = SerialNo;
            long num = 0;
            if ((nullable.GetValueOrDefault() <= num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                throw new ArgumentException("Serial No cannot be equal or less than 0 ", nameof(SerialNo));

            //get product
            SubRisk subRisk = SubRiskSelectThis(SubRiskID);
            if (subRisk == null)
                throw new KeyNotFoundException("SubRisk ID does not exist");

            subRisk.Active = (byte?)SerialNo;
            _db.SaveChanges();
        }
    }
}
