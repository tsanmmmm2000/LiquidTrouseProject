using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess
{
    public interface ISearchDao
    {
        IList Search(
            string keyword,
            string sortBy,
            string sortOrder,
            DateTime? startDate,
            DateTime? endDate,
            int pageIndex, 
            int pageSize);

        int GetTotalCount(
            string keyword,
            string sortBy,
            DateTime? startDate,
            DateTime? endDate);
    }
}
