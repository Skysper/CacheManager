using System;
using System.Collections.Generic;
using System.Text;
using CacheManager.Caching;
using CacheManager.Model;

namespace CacheManager.Service
{
    public class CacheService : ICacheService
    {
        public CacheResult Query(AppInfo app, string key)
        {
            var cache = Caching.CacheFactory.Create(app.Type, app.ConnectionString);
            CacheKeyType type = cache.Type(key);
            string value = cache.QueryWithType(key, type);
            TimeSpan? expire = cache.Expire(key);

            CacheResult result = new CacheResult();
            result.Expire = expire.HasValue ? expire.Value.TotalSeconds.ToString() : "";
            result.Value = value;
            result.Key = key;
            result.Index = 0;
            result.Type = ((int)type).ToString();
            return result;
        }
    }
}
