using System;
using System.ComponentModel.DataAnnotations;

namespace Universal.Api.Contracts.V1
{
    public class FilterPaging
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        [Range(1, 1000)]
        public int PageNo { get; set; } = 1;

        [Range(5, 500)]
        public int PageSize { get; set; } = 20;

        internal int SkipCount => PageSize * (PageNo - 1);

        public FilterPaging()
        {
            PageNo = 1;
            PageSize = 20;
        }

        public FilterPaging(
                        DateTime? dateFrom = null,
                        DateTime? dateTo = null,
                        int pageNo = 1,
                        int pageSize = 20)
        {
            if (pageNo < 1)
                pageNo = 1;

            if (pageSize < 5)
                pageSize = 5;

            if (pageSize > 100)
                pageSize = 100;

            DateFrom = dateFrom;
            DateTo = dateTo;
            PageNo = pageNo;
            PageSize = pageSize;
        }
        internal bool CanSearchDate
        {
            get
            {
                return (DateFrom.HasValue && DateTo.HasValue);
            }
        }
    }
}
