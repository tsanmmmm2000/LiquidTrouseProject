using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace LiquidTrouse.Core.CacheService.Impl.InMemory
{
    public sealed class InMemoryCache : MemoryCache
    {
        public InMemoryCache() : base("defaultInMemoryCache") { }

        public new IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return base.GetEnumerator();
        }
    }
}
