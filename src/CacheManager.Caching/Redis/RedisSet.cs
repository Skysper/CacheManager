using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace CacheManager.Caching.Redis
{
    public class RedisSet : IOperator
    {
        public string Format(IDatabase database, string key)
        {
            RedisValue[] array = database.SetMembers(key);
            return Helper.FormatArray(array);
        }

        public bool Set(IDatabase database, string key, string value, int timeLive = 0)
        {
            if (String.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return false;
            }
            bool isOk = Helper.RemoveExist(database, key);
            if (!isOk)
            {
                return false;
            }

            string[] array = value.Split(CommonConst.REDIS_MULTI_VALUE_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);

            RedisValue[] values = Helper.ConvertTo(array);
            if (values == null) return false;

            isOk = database.SetAdd(key, values, CommandFlags.PreferMaster) == array.Length;
            if (!isOk) return false;

            if (timeLive > 0)
            {
                isOk = database.KeyExpire(key, Helper.SetTimeSpan(timeLive), CommandFlags.PreferMaster);
            }
            return isOk;
        }
    }

}
