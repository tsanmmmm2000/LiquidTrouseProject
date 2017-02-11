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
            SortBy sortBy;
            switch (sortByString.ToLower())
            {
                case "title":
                    sortBy = SortBy.Title;
                    break;
                case "hit":
                    sortBy = SortBy.Hit;
                    break;
                case "creationdatetime":
                    sortBy = SortBy.CreationDatetime;
                    break;
                case "lastmodifieddatetime":
                    sortBy = SortBy.LastModifiedDatetime;
                    break;
                default:
                    sortBy = SortBy.CreationDatetime;
                    break;
            }
            return sortBy;
        }
        public static SortDirection ParseSortDirection(string sortDirectionString)
        {
            SortDirection sortDirection;
            switch (sortDirectionString.ToLower())
            {
                case "asc":
                    sortDirection = SortDirection.Asc;
                    break;
                case "desc":
                    sortDirection = SortDirection.Desc;
                    break;
                default:
                    sortDirection = SortDirection.Desc;
                    break;
            }
            return sortDirection;
        }
    }
}
