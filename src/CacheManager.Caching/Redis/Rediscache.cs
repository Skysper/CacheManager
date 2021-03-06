﻿using StackExchange.Redis;
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
                if (!_client.IsConnected) return false;
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
                if (!_client.IsConnected) return "";
                return _client.GetDatabase().StringGet(key);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// query and format the value result by cache key type
        /// Hash、List、Set and String etc.
        /// they all has different format in order to modify by client
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string QueryWithType(string key, CacheKeyType type)
        {
            if (!_client.IsConnected) return string.Empty;
            try
            {
                IDatabase database = _client.GetDatabase();
                IOperator op = null;
                switch (type)
                {
                    case CacheKeyType.None: break;
                    case CacheKeyType.String:
                        op = new RedisString();
                        break;
                    case CacheKeyType.Hash:
                        op = new RedisHash();
                        break;
                    case CacheKeyType.List:
                        op = new RedisList();
                        break;
                    case CacheKeyType.Set:
                        op = new RedisSet();
                        break;
                    case CacheKeyType.SortedSet:
                        op = new RedisSortedSet();
                        break;
                }
                if (op == null) return string.Empty;
                else return op.Format(database, key);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public bool Set(string key, CacheKeyType type, string value, int timeToLive)
        {
            if (!_client.IsConnected) return false;
            IDatabase database = _client.GetDatabase();
            IOperator op = null;
            switch (type)
            {
                case CacheKeyType.None:
                case CacheKeyType.String:
                    op = new RedisString();
                    break;
                case CacheKeyType.Hash:
                    op = new RedisHash();
                    break;
                case CacheKeyType.List:
                    op = new RedisList();
                    break;
                case CacheKeyType.Set:
                    op = new RedisSet();
                    break;
                case CacheKeyType.SortedSet:
                    op = new RedisSortedSet();
                    break;
            }
            if (op == null) { return false; }
            else {
                return op.Set(database, key, value, timeToLive);
            }
        }


        public IEnumerable<string> Query(string[] keys)
        {
            if (!_client.IsConnected)
            {
                return null;
            }

            try
            {
                var count = keys.Length;
                var redisKeys = keys.Select(key => (RedisKey)key).ToArray();
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
                if (!_client.IsConnected)
                {
                    return CacheKeyType.None;
                }
                return (CacheKeyType)((int)_client.GetDatabase().KeyType(key));
            }
            catch (Exception ex)
            {
                return CacheKeyType.None;
            }
        }

        public List<CacheKeyType> Type(string[] keys)
        {
            if (!_client.IsConnected) {
                return null;
            }
            var db = _client.GetDatabase();
            List<CacheKeyType> list = new List<CacheKeyType>();
            foreach (var key in keys)
            {
                var type = (CacheKeyType)((int)db.KeyType(key));
                list.Add(type);
            }
            return list;
        }

        public List<TimeSpan?> Expire(string[] keys)
        {
            if (!_client.IsConnected) {
                return null;
            }
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
            try
            {
                _conn = conn;
                string[] array = _conn.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                ConfigurationOptions options = new ConfigurationOptions();
                options.AbortOnConnectFail = false;
                if (array.Length > 1)
                {
                    _isCluster = true;

                    string[] ipAndPortArray = new string[array.Length * 2];
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
                    
                    options.EndPoints.Add(_conn);
                    _client = ConnectionMultiplexer.Connect(options);
                }
            }
            catch (Exception ex) {
                _client = null;
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

        public TimeSpan? Expire(string key)
        {
            if (_client == null) return null;
            var db = _client.GetDatabase();
            return db.KeyTimeToLive(key, CommandFlags.PreferSlave);
        }
    }
}
