using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core
{
    public struct PageOf<T>
    {
        public int TotalCount;
        public int PageSize;
        public int PageIndex;
        public IEnumerable<T> Results;
    }
}
