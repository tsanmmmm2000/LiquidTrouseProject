using LiquidTrouse.Core.CacheService.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace LiquidTrouse.Core.CacheService.Impl.InMemory
{
    public class InMemoryCacheService : ICacheService
    {
        private static readonly InMemoryCache _cache = new InMemoryCache();
        private static IDictionary<CacheKey, CacheObject> _presistCacheObjects = new Dictionary<CacheKey, CacheObject>();
        private const string PersistenceFileName = "persistence.dat";

        private string _persistStoragePath = string.Empty;
        public string PersistStoragePath
        {
            set { _persistStoragePath = value; }
        }

        private bool _enablePersistence = false;
        public bool EnablePersistence
        {
            set 
            { 
                _enablePersistence = value;
                if (_enablePersistence)
                {
                    Recover();
                }
            }
        }

        public InMemoryCacheService() { }

        public CacheObject Get<T>(CacheKey cacheKey, Func<T> GetDataCallback) where T : class
        {
            lock (this)
            {
                var cacheObject = _cache.Get(cacheKey.KeyName) as CacheObject;
                if (cacheObject == null)
                {
                    cacheObject = new CacheObject();
                    cacheObject.Data = GetDataCallback();
                    cacheObject.DataType = GetDataCallback().GetType();
                    Set(cacheKey, cacheObject);
                }
                return cacheObject;
            }
        }
        public CacheObject Get(CacheKey cacheKey)
        {
            lock (this)
            {
                return _cache.Get(cacheKey.KeyName) as CacheObject;
            }
        }
        public CacheObject Set(CacheKey cacheKey, CacheObject cacheObject)
        {
            lock (this)
            {
                try
                {
                    var policy = new CacheItemPolicy();
                    policy.AbsoluteExpiration = DateTime.SpecifyKind(cacheObject.AbsoluteExpiration, DateTimeKind.Utc);
                    policy.SlidingExpiration = cacheObject.SlidingExpiration;
                    _cache.Set(cacheKey.KeyName, cacheObject, policy);
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(ex.Message, ex);
                }
                return cacheObject;
            }
        }
        public void Remove(CacheKey cacheKey)
        {
            lock (this)
            {
                try
                {
                    _cache.Remove(cacheKey.KeyName);
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(ex.Message, ex);
                }
            }
        }
        public void Remove(CacheKeyMatchEvaluator cacheKeyMatchEvaluator)
        {
            lock (this)
            {
                var cacheObjects = GetCollection(cacheKeyMatchEvaluator);
                foreach(KeyValuePair<CacheKey, CacheObject> keyValuePair in cacheObjects)
                {
                    Remove(keyValuePair.Key);
                }
            }
        }
        public bool Contains(CacheKey cacheKey)
        {
            lock (this)
            {
                return _cache.Contains(cacheKey.KeyName);
            }
        }
        public void Clear()
        {
            lock (this)
            {
                _cache.Dispose();
            }
        }
        public IDictionary<CacheKey, CacheObject> GetCollection()
        {
            lock (this)
            {
                return GetCollection(null);
            }
        }
        public IDictionary<CacheKey, CacheObject> GetCollection(CacheKeyMatchEvaluator cacheKeyMatchEvaluator)
        {
            lock (this)
            {
                var cacheObjects = new Dictionary<CacheKey, CacheObject>();
                try
                {
                    var cacheEnum = _cache.GetEnumerator();
                    while (cacheEnum.MoveNext())
                    {
                        var cacheKey = new CacheKey(cacheEnum.Current.Key);
                        var cacheObject = cacheEnum.Current.Value as CacheObject;
                        if ((cacheKeyMatchEvaluator != null && cacheKeyMatchEvaluator(cacheKey))
                            || cacheKeyMatchEvaluator == null)
                        {
                            if (!cacheObjects.ContainsKey(cacheKey))
                            {
                                cacheObjects.Add(cacheKey, cacheObject);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(ex.Message, ex);
                }
                return cacheObjects;
            }
        }

        private void RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            if (arguments.RemovedReason == CacheEntryRemovedReason.CacheSpecificEviction)
            {
                var cacheKey = new CacheKey(arguments.CacheItem.Key);
                Utility.DebugLog("Cache Removed Callback... cache key: " + cacheKey);
                if (!_presistCacheObjects.ContainsKey(cacheKey))
                {
                    var cacheObject = arguments.CacheItem.Value as CacheObject;
                    _presistCacheObjects.Add(cacheKey, cacheObject);
                }
                Persist();
            }
        }
        private void Recover()
        {
            lock (this)
            {
                try
                {
                    var filePath = Path.Combine(_persistStoragePath, PersistenceFileName);
                    if (File.Exists(filePath))
                    {
                        using (var fs = new FileStream(filePath, FileMode.Open))
                        {
                            Utility.DebugLog("Start Recover Cache...");
                            var formatter = new BinaryFormatter();
                            var cacheEnum = formatter.Deserialize(fs) as IDictionaryEnumerator;
                            while (cacheEnum.MoveNext())
                            {
                                var cacheKey = cacheEnum.Key as CacheKey;
                                var cacheObject = cacheEnum.Value as CacheObject;
                                Set(cacheKey, cacheObject);
                            } 
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(ex.Message, ex);
                }
            }
        }
        private void Persist()
        {
            lock (this)
            {
                try
                {
                    var filePath = Path.Combine(_persistStoragePath, PersistenceFileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        Utility.DebugLog("Start Persist Cache...");
                        var formatter = new BinaryFormatter();
                        formatter.Serialize(fs, _presistCacheObjects);
                    }
                }
                catch (Exception ex)
                {
                    Utility.ErrorLog(ex.Message, ex);
                }
            }
        }
    }
}
