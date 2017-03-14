using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.CacheService.Domain
{
    public class CacheKey
    {
        private string _keyName = string.Empty;

        public CacheKey(string keyName)
        {
            _keyName = keyName;
        }

        public string KeyName
        {
            get { return _keyName; }
        }
    }
}
