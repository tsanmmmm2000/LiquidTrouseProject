using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    public enum SortBy
    {
        Hit,
        Title,
        CreationDatetime,
        LastModifiedDatetime,
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }

    public class SortingInfo
    {
        private SortBy _sortBy = SortBy.LastModifiedDatetime;
        public SortBy SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }

        private SortDirection _sortDirection = SortDirection.Desc;
        public SortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        public static SortBy ParseSortBy(string sortByString)
        {
            try
            {
                return (SortBy)Enum.Parse(typeof(SortBy), sortByString, true);
            }
            catch
            {
                return SortBy.LastModifiedDatetime;
            }
        }
        public static SortDirection ParseSortDirection(string sortDirectionString)
        {
            try
            {
                return (SortDirection)Enum.Parse(typeof(SortDirection), sortDirectionString, true);
            }
            catch
            {
                return SortDirection.Desc;
            }
        }
    }
}
