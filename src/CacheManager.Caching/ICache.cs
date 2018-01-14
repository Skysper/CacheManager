using System;
using System.Collections.Generic;

namespace CacheManager.Caching
{
    public interface ICache
    {
        string Query(string key);

        IEnumerable<string> Query(string[] keys);

        bool Clear(string key);

        string QueryWithType(string key, CacheKeyType type);

        CacheKeyType Type(string key);

        void Close();
        List<CacheKeyType> Type(string[] keys);

        List<TimeSpan?> Expire(string[] keys);

        TimeSpan? Expire(string key);

        bool Set(string key, CacheKeyType type, string value, int timeToLive);
    }
}
