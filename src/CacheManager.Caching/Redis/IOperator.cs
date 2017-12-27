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
        /// <param name="database">数据库操作对象</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeLive">存活时间</param>
        /// <returns></returns>
        bool Set(IDatabase database, string key, string value, int timeLive = 0);
    }
}
