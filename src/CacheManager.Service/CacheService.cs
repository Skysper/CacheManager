﻿using System;
using System.Collections.Generic;
using System.Text;
using CacheManager.Caching;
using CacheManager.Model;
using System.Linq;

namespace CacheManager.Service
{
    public class CacheService : ICacheService
    {
        public bool Clear(App app, string key)
        {
            var cache = Caching.CacheFactory.Create(app.Type, app.ConnectionString);
            bool isOk = cache.Clear(key);

            cache.Close();
            return isOk;
        }

        public bool Clear(App app, List<string> keys) {
            var cache = Caching.CacheFactory.Create(app.Type, app.ConnectionString);
            foreach (var item in keys) {
                cache.Clear(item);
            }
            cache.Close();
            return true;
        }


        public CacheResult Query(App app, string key)
        {
            var cache = Caching.CacheFactory.Create(app.Type, app.ConnectionString);
            CacheKeyType type = cache.Type(key);
            string value = cache.QueryWithType(key, type);
            TimeSpan? expire = cache.Expire(key);

            cache.Close();

            CacheResult result = new CacheResult();
            result.Expire = expire.HasValue ? expire.Value.TotalSeconds.ToString() : "";
            result.Value = value;
            result.Key = key;
            result.Index = 0;
            result.Type = ((int)type).ToString();
            return result;
        }

        /// <summary>
        /// search format key result by params in the cache server
        /// </summary>
        /// <param name="app">app information</param>
        /// <param name="key">the format key to query</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">result count for one page</param>
        /// <param name="ignoreType">whether query with cache type,
        /// true will ignore the type,
        /// false will search key result one by one with key type result
        /// (warning:it may cost more time to finish the query)</param>
        /// <returns></returns>
        public PagedDataJsonMsg Search(App app, string keyFormat, int pageIndex, int pageSize, bool ignoreTTL = false, bool ignoreNull = false)
        {
            var cache = Caching.CacheFactory.Create(Caching.CacheType.Rediscache, app.ConnectionString);

            var ra = new RangeAnalysis(keyFormat);
            string format = ra.Format();
            List<Ranges> ranges = ra.Analysis();
            int keyCount = 0;
            List<string> keys = new List<string>();
            if (ranges.Count > 0)
            {
                keys = KeyManager.GetPagedKeys(format, ranges, pageIndex, pageSize);
                keyCount = KeyManager.GetKeysCount(ranges);
            }
            else
            {
                keys.Add(keyFormat);
                keyCount = 1;
            }



            List<string> keyResult = null;
            List<CacheKeyType> typeResult = cache.Type(keys.ToArray());
            if (typeResult != null)
            {
                keyResult = new List<string>();
                for (int i = 0; i < keys.Count; i++)
                {
                    if (typeResult[i] == CacheKeyType.None) {
                        keyResult.Add("");
                    }
                    else
                    {
                        keyResult.Add(cache.QueryWithType(keys[i], typeResult[i]));
                    }
                }
            }

            List<TimeSpan?> expireResult = null;
            if(!ignoreTTL)
            {
                expireResult = cache.Expire(keys.ToArray());
            }

            cache.Close();

            List<CacheResult> list = null;
            if (keyResult != null)
            {
                list = new List<CacheResult>();
                var resultList = keyResult.ToList();
                for (int i = 0; i < resultList.Count; i++)
                {
                    Caching.CacheResult cr = new Caching.CacheResult();
                    cr.Index = i + 1;
                    cr.Key = keys[i];
                    cr.Expire = !ignoreTTL && expireResult[i].HasValue ? (expireResult[i].Value.TotalSeconds + "s") : "";
                    cr.Type = typeResult[i].ToString();
                    cr.Value = resultList[i];
                    if (ignoreNull && cr.Type == CacheKeyType.None.ToString())
                    {
                        continue;
                    }
                    list.Add(cr);
                }
            }
            //if the cache connection string is error
            //the cache result list will return null
            //then reset the key count to 0
            if (list == null)
            {
                keyCount = 0;
            }

            PagedDataJsonMsg msg = new PagedDataJsonMsg(MsgConst.OK, "", list, keyCount);
            return msg;
        }

    }
}
