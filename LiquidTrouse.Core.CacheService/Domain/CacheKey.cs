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

        public override int GetHashCode()
        {
            return this.KeyName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            if (!(obj is CacheKey))
            {
                return false;
            }

            var cacheKey = (CacheKey)obj;
            var objKey = cacheKey.KeyName;
            var thisKey = this.KeyName;
            return (objKey.Equals(thisKey));
        }
    }
}
