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
        CacheObject Set(CacheKey cacheKey, CacheObject cacheObject);
        CacheObject Get<T>(CacheKey cacheKey, Func<T> GetDataCallback) where T : class;
        CacheObject Get(CacheKey cacheKey);
        void Remove(CacheKey cacheKey);
        void Remove(CacheKeyMatchEvaluator cacheKeyMatchEvaluator);
        void Clear();
        bool Contains(CacheKey cacheKey);
        IDictionary<CacheKey, CacheObject> GetCollection();
        IDictionary<CacheKey, CacheObject> GetCollection(CacheKeyMatchEvaluator cacheKeyMatchEvaluator);
    }
}
