using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CacheManager.Caching
{
    internal class Rediscache : ICache
    {
        private readonly string _conn;
        public Rediscache(string conn)
        {
            _conn = conn;
        }

        public bool Clear(string key)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    return client.GetDatabase().KeyDelete(key);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Query(string key)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    return client.GetDatabase().StringGet(key);
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public IEnumerable<string> Query(string[] keys)
        {
            var count = keys.Length;
            var redisKeys = keys.Select(key => (RedisKey)key).ToArray();

            try
            {
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    var values = client.GetDatabase().StringGet(redisKeys);
                    return values.Select(val => (string)val);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
