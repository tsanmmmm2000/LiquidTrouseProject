using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.Service.DTO
{
    public class QueryPackage
    {
        private string _queryString;
        public string QueryString
        {
            get { return _queryString; }
            set { _queryString = value; }
        }

        private SortingInfo _sortingInfo;
        public SortingInfo SortingInfo
        {
            get { return _sortingInfo; }
            set { _sortingInfo = value; }
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
