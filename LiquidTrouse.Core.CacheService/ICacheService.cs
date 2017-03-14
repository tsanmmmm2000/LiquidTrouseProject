using LiquidTrouse.Core.CacheService.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiquidTrouse.Core.CacheService
{
    public interface ICacheService
    {
        CacheObject Set(string cacheKey, CacheObject cacheObject);
        CacheObject Get<T>(string cacheKey, Func<T> GetDataCallback) where T : class;
        CacheObject Get(string cacheKey);
        void Remove(string cacheKey);
        void Clear();
        bool Contains(string cacheKey);
        IDictionaryEnumerator GetEnumerator();
    }
}
