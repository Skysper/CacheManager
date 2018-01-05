using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching.Redis
{
    public interface IOperator
    {
        string Format(IDatabase database, string key);

        /// <summary>
        /// set key value.
        /// if key is empty or value is empty, return false.
        /// 
        /// because we have checked the key exists and make sure to delete it first.
        /// then we can add it use any type we want
        /// so we can set the timeout because it haven't got any timeout info
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeLive">key timeout</param>
        /// <returns></returns>
        bool Set(IDatabase database, string key, string value, int timeLive = 0);
    }
}
