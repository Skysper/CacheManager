using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching.Redis
{
    /// <summary>
    /// Redis字符相关操作
    /// </summary>
    public class RedisString : IOperator
    {
        public string Format(IDatabase database, string key)
        {
            return database.StringGet(key, CommandFlags.PreferSlave);
        }

        public bool Set(IDatabase database, string key, string value, int timeLive = 0)
        {
            if (String.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return false;
            }
            //do not have to delete the key before, it can overwrite all the keys,regardless of its type
            return database.StringSet(key, value, Helper.SetTimeSpan(timeLive), When.Always, CommandFlags.PreferMaster);
        }

    }
}
