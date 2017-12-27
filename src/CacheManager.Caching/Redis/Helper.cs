using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching.Redis
{
    public class Helper
    {
        public static TimeSpan? SetTimeSpan(int timeLive)
        {
            TimeSpan? timeSpan = null;
            if (timeLive > 0)
            {
                timeSpan = new TimeSpan(0, 0, timeLive);
            }
            return timeSpan;
        }

        public static String FormatArray(RedisValue[] values)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in values)
            {
                stringBuilder.Append(item);
                stringBuilder.Append(CommonConst.NEW_LINE);
            }
            return stringBuilder.ToString();
        }
        public static String FormatHash(HashEntry[] entries)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var entry in entries)
            {
                stringBuilder.Append(entry.Name);
                stringBuilder.Append("$$");
                stringBuilder.Append(entry.Value);
                stringBuilder.Append("\r\n");
            }
            return stringBuilder.ToString();
        }

        public static String FormatSortedSetEntry(IEnumerable<SortedSetEntry> entries)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var entry in entries)
            {
                stringBuilder.Append(entry.Element);
                stringBuilder.Append("$$");
                stringBuilder.Append(entry.Score);
                stringBuilder.Append("\r\n");
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 移除存在的键值
        /// </summary>
        /// <param name="database"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveExist(IDatabase database, string key)
        {
            if (database.KeyExists(key))
            {
                return database.KeyDelete(key, CommandFlags.PreferMaster);
            }
            return true;
        }

        /// <summary>
        /// 将字符数据转换为RedisValue数组
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static RedisValue[] ConvertTo(string[] array)
        {
            if (array == null || array.Length == 0) return null;
            RedisValue[] values = new RedisValue[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                values[i] = array[i];
            }
            return values;
        }

    }
}
