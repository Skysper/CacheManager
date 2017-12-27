using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace CacheManager.Caching.Redis
{
    public class RedisHash : IOperator
    {
        public string Format(IDatabase database, string key)
        {
            HashEntry[] entries = database.HashGetAll(key, CommandFlags.PreferSlave);
            return Helper.FormatHash(entries);
        }

        public bool Set(IDatabase database, string key, string value, int timeLive = 0)
        {
            if (String.IsNullOrEmpty(value) || string.IsNullOrEmpty(key))
            {
                return false;
            }

            bool isOk = Helper.RemoveExist(database, key);
            if (!isOk) {
                return false;
            }

            //解析hash字符
            string[] array = value.Split(CommonConst.REDIS_MULTI_VALUE_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);
            List<HashEntry> entries = new List<HashEntry>();
            foreach (var item in array) {
                int index = item.IndexOf(CommonConst.INLINE_SPLIT_CHAR);
                //index不可为0，否则key值为空
                if (index <= 0) {
                    continue;
                }
                
                HashEntry entry = new HashEntry(item.Substring(0, index),
                    item.Substring(index + CommonConst.INLINE_SPLIT_CHAR.Length));
                entries.Add(entry);
            }

            database.HashSet(key, entries.ToArray(), CommandFlags.PreferMaster);
            if (timeLive > 0)
            {
                isOk = database.KeyExpire(key, Helper.SetTimeSpan(timeLive), CommandFlags.PreferMaster);
            }
            return isOk;
        }



    }
}
