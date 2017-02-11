using LiquidTrouse.Core.Blog.DataAccess.Domain;
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
            Sorting sorting,
            DateTime? startDate,
            DateTime? endDate,
            int pageIndex, 
            int pageSize);

        int GetTotalCount(
            string keyword,
            Sorting sorting,
            DateTime? startDate,
            DateTime? endDate);
    }
}
