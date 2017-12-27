using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CacheManager.Caching.Redis
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

        public string QueryWithType(string key, CacheKeyType type)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    IDatabase database = client.GetDatabase();
                    IOperator operate = null;
                    switch (type)
                    {
                        case CacheKeyType.None: break;
                        case CacheKeyType.String:
                            operate = new RedisString();
                            break;
                        case CacheKeyType.Hash:
                            operate = new RedisHash();
                            break;
                        case CacheKeyType.List:
                            operate = new RedisList();
                            break;
                        case CacheKeyType.Set:
                            operate = new RedisSet();
                            break;
                        case CacheKeyType.SortedSet:
                            operate = new RedisSortedSet();
                            break;
                    }
                    if (operate == null) return string.Empty;
                    else return operate.Format(database, key);
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

        public CacheKeyType Type(string key)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(_conn))
                {
                    return  (CacheKeyType)((int)client.GetDatabase().KeyType(key));
                }
            }
            catch (Exception ex)
            {
                return CacheKeyType.None;
            }
        }
    }
}
