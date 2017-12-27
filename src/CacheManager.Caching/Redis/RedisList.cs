using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace CacheManager.Caching.Redis
{
    public class RedisList : IOperator
    {
        public string Format(IDatabase database, string key)
        {
            RedisValue[] values = database.ListRange(key, 0, -1, CommandFlags.PreferSlave);
            return Helper.FormatArray(values);
        }

        public bool Set(IDatabase database, string key, string value, int timeLive = 0)
        {
            if (String.IsNullOrEmpty(value) || string.IsNullOrEmpty(key)) {
                return false;
            }
            string[] array = value.Split(CommonConst.REDIS_MULTI_VALUE_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);
            RedisValue[] values = new RedisValue[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                values[i] = array[i];
            }
            bool isOk = database.ListRightPush(key, values, CommandFlags.PreferMaster) == values.Length;
            if (!isOk) return false;

            if (timeLive > 0)
            {
                isOk = database.KeyExpire(key, Helper.SetTimeSpan(timeLive), CommandFlags.PreferMaster);
            }
            return isOk;
        }
    }
}
