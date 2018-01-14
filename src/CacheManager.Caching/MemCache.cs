using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    internal class Memcache : ICache
    {
        private readonly string _conn;
        public Memcache(string conn)
        {
            _conn = conn;
        }

        public bool Clear(string key)
        {
            throw new NotImplementedException();
        }

        public string Query(string key)
        {
            return string.Empty;
        }

        public IEnumerable<string> Query(string[] keys)
        {
            return null;
        }

        /// <summary>
        /// memcache need not to implement this function.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string QueryWithType(string key, CacheKeyType type)
        {
            throw new NotImplementedException();
        }

        public CacheKeyType Type(string key)
        {
            return CacheKeyType.String;
        }

        public void Close()
        {
            return;
        }

        public List<CacheKeyType> Type(string[] keys)
        {
            throw new NotImplementedException();
        }

        public List<TimeSpan?> Expire(string[] keys)
        {
            throw new NotImplementedException();
        }

        public TimeSpan? Expire(string key)
        {
            throw new NotImplementedException();
        }

        public bool Set(string key, CacheKeyType type, string value, int timeToLive)
        {
            throw new NotImplementedException();
        }
    }
}
