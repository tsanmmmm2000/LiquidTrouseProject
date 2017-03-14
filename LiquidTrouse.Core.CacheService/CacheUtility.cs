using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.CacheService
{
    public class CacheUtility
    {
        public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;
        public static readonly TimeSpan DefaultSlidingExpiration = new TimeSpan(0, 20, 0);

        private static CacheRepositoryFactory _cacheRepository;
        public static CacheRepositoryFactory CacheRepository
        {
            get
            {
                _cacheRepository = new CacheRepositoryFactory(Utility.ApplicationContext);
                return _cacheRepository;
            }
        }
    }
}
