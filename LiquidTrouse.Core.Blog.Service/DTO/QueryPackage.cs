using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    public enum SortBy
    {
        Title,
        CreationDatetime,
        LastModifiedDatetime,
    }

    public enum SortOrder
    {
        Asc,
        Desc
    }

    public class QueryPackage
    {
        private string _queryString;
        public string QueryString
        {
            get { return _queryString; }
            set { _queryString = value; }
        }

        private SortBy _sortBy = SortBy.LastModifiedDatetime;
        public SortBy SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }

        private SortOrder _sortOrder = SortOrder.Desc;
        public SortOrder SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        private DateTime? _startDate = null;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime? _endDate = null;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
    }
}
