using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CacheManager.Caching
{
    public class KeyManager
    {
        public static List<string> GetPagedKeys(string keyFormat, List<Ranges> list, int pageIndex, int pageSize = 50)
        {
            int total = GetKeysCount(list);
            int skip = (pageIndex - 1) * pageSize;
            List<string> keys = new List<string>();
            //返回空集合
            if (skip >= total) { return keys; }

            int startPosition = skip + 1;
            //todo:算法

            List<int> steps = GetPosition(list, startPosition);
            List<RangeValue> chars = new List<RangeValue>();
            for (int j = 0; j < list.Count; j++)
            {
                var r = list[j];
                chars.Add(list[j].Index(steps[j]));
            }
            keys.Add(string.Format(keyFormat, chars.Cast<Object>().ToArray()));
            int size = Math.Min(total - startPosition + 1, pageSize);
            for (int i = 1; i < size; i++)
            {
                int lastIndex = list.Count - 1;
                chars[lastIndex] = list[lastIndex].Next(chars[lastIndex]);
                bool outBoundary = list[lastIndex].OutBoundary(chars[lastIndex]);
                while (outBoundary)
                {
                    if (lastIndex == 0) break;
                    chars[lastIndex] = list[lastIndex].From;
                    lastIndex--;
                    chars[lastIndex] = list[lastIndex].Next(chars[lastIndex]);
                    outBoundary = list[lastIndex].OutBoundary(chars[lastIndex]);
                }

                keys.Add(string.Format(keyFormat, chars.Cast<Object>().ToArray()));

            }

            return keys;
        }

        private static List<int> GetPosition(List<Ranges> list, int startPosition)
        {
            //因为计数从1开始，则减1处理
            startPosition = startPosition - 1;
            List<int> steps = new List<int>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                int count = list[i].Count();
                int remainder = startPosition % count;
                steps.Add(remainder);

                startPosition = startPosition / count;
            }
            steps.Reverse();
            return steps;
        }

        public static int GetKeysCount(List<Ranges> list)
        {
            int total = 1;
            list.ForEach(r => total *= r.Count());
            return total;
        }
    }
}
