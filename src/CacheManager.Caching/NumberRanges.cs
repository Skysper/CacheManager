using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public class NumberRanges : Ranges
    {
        /// <summary>
        /// 索引位置，从0开始计算
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override RangeValue Index(int index)
        {
            if (From > To)
            {
                return (From - index);
            }
            else
            {
                return (From + index);
            }
        }

        public override RangeValue Next(RangeValue c)
        {
            if (From > To) return (c - 1);
            else
            {
                return (c + 1);
            }
        }
    }
}
