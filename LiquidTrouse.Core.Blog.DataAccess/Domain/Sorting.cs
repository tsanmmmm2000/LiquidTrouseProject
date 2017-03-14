using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.Blog.DataAccess.Domain
{
    public class Sorting
    {
        private string _sortBy = "CreationDatetime";
        private string _sortDirection = "desc"; 

        public Sorting() { }

        public string SortBy
        {
            get { return _sortBy; }
            set { _sortBy = value; }
        }

        public string SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }
    }
}
