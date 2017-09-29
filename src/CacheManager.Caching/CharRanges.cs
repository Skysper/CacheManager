using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public class CharRanges : Ranges
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
                return (char)(From - index);
            }
            else
            {
                return (char)(From + index);
            }
        }

        public override RangeValue Next(RangeValue c)
        {
            if (From > To) return (char)(c - 1);
            else
            {
                return (char)(c + 1);
            }
        }

    }

}
