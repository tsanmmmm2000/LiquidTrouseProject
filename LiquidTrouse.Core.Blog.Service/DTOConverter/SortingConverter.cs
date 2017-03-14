using LiquidTrouse.Core.Blog.DataAccess.Domain;
using LiquidTrouse.Core.Blog.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTOConverter
{
    public class SortingConverter
    {
        public SortingConverter() { }

        public Sorting ToDomainObject(SortingInfo sortingInfo)
        {
            var sorting = new Sorting();
            if (sortingInfo != null)
            {
                sorting.SortBy = sortingInfo.SortBy.ToString();
                sorting.SortDirection = sortingInfo.SortDirection.ToString().ToLower();
            }
            return sorting;
        }
    }
}
