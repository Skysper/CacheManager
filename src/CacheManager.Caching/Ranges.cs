using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public abstract class Ranges
    {
        public RangeValue From { set; get; }
        public RangeValue To { set; get; }

        public int Count()
        {
            return Math.Abs(To - From) + 1;
        }

        public bool OutBoundary(RangeValue value)
        {
            if (From > To)
            {
                if (value < To) { return true; }
                else return false;
            }
            else
            {
                if (value > To) { return true; }
                else return false;
            }
        }

        public abstract RangeValue Index(int index);
        public abstract RangeValue Next(RangeValue value);
    }

}
