using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.CacheService
{
    public class CacheRepositoryFactory
    {
        public CacheRepositoryFactory() { }

        public CacheRepositoryFactory(IApplicationContext context)
        {
            Utility.ApplicationContext = context;
        }

        public ICacheService GetCacheService()
        {
            return Utility.ApplicationContext["CacheService"] as ICacheService;
        }
    }
}
