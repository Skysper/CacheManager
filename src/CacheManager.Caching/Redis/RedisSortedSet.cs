using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace CacheManager.Caching.Redis
{
    public class RedisSortedSet : IOperator
    {
        public string Format(IDatabase database, string key)
        {
            IEnumerable<SortedSetEntry> entries = database.SortedSetScan(key);
            return Helper.FormatSortedSetEntry(entries);
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
            List<SortedSetEntry> list = new List<SortedSetEntry>();
            foreach (var item in array)
            {
                int index = item.IndexOf(CommonConst.INLINE_SPLIT_CHAR);
                if (index <= 0) continue;
                string itemValue = item.Substring(0, index);
                string itemScore = item.Substring(index + CommonConst.INLINE_SPLIT_CHAR.Length);
                double score = 0d;
                double.TryParse(itemValue, out score);
                SortedSetEntry entry = new SortedSetEntry(itemScore, score);
                list.Add(entry);
            }

            isOk = database.SortedSetAdd(key, list.ToArray(), CommandFlags.PreferMaster) == list.Count;
            if (!isOk)
            {
                return false;
            }
            else {
                if (timeLive > 0)
                {
                    isOk = database.KeyExpire(key, Helper.SetTimeSpan(timeLive), CommandFlags.PreferMaster);
                }
                return isOk;
            }
        }
    }
}
