using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;

namespace CacheManager.Caching.Redis
{
    internal class Rediscache : ICache
    {
        private string _conn;
        private ConnectionMultiplexer _client;
        private bool _isCluster;
        public Rediscache(string conn)
        {
            InitClient(conn);
        }

        public bool Clear(string key)
        {
            try
            {
                return _client.GetDatabase().KeyDelete(key);
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
                return _client.GetDatabase().StringGet(key);
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
                IDatabase database = _client.GetDatabase();
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
                if (_isCluster)
                {
                    List<string> list = new List<string>();
                    var db = _client.GetDatabase();
                    foreach (var key in keys)
                    {
                        string value = db.StringGet(key);
                        if (!string.IsNullOrEmpty(value))
                        {
                            list.Add(value);
                        }
                        else
                        {
                            list.Add("");
                        }
                    }
                    return list;
                }
                else
                {
                    var values = _client.GetDatabase().StringGet(redisKeys);
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
                return (CacheKeyType)((int)_client.GetDatabase().KeyType(key));
            }
            catch (Exception ex)
            {
                return CacheKeyType.None;
            }
        }

        public List<CacheKeyType> Type(string[] keys) {
            var db = _client.GetDatabase();
            List<CacheKeyType> list = new List<CacheKeyType>();
            foreach (var key in keys) {
                var type = (CacheKeyType)((int)db.KeyType(key));
                list.Add(type);
            }
            return list;
        }

        public List<TimeSpan?> Expire(string[] keys) {
            var db = _client.GetDatabase();
            List<TimeSpan?> list = new List<TimeSpan?>();
            foreach (var key in keys)
            {
                TimeSpan? span = db.KeyTimeToLive(key, CommandFlags.PreferSlave);
                list.Add(span);
            }
            return list;
        }


        /// <summary>
        /// redis连接客户端
        /// </summary>
        /// <returns></returns>
        private void InitClient(string conn)
        {
            _conn = conn;
            string[] array = _conn.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length > 1)
            {
                _isCluster = true;
                string[] ipAndPortArray = new string[array.Length * 2];
                ConfigurationOptions options = new ConfigurationOptions();
                List<EndPoint> endPoints = new List<EndPoint>();
                for (int i = 0; i < array.Length; i++)
                {
                    options.EndPoints.Add(array[i]);
                }
                options.AllowAdmin = true;
                _client = ConnectionMultiplexer.Connect(options);
            }
            else
            {
                _isCluster = false;
                _client = ConnectionMultiplexer.Connect(_conn);
            }
        }

        public void Close()
        {
            if (_client != null)
            {
                _client.Close();
                _client = null;
            }
        }
    }
}
