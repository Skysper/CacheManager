using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public class CacheResult
    {
        //索引
        public int Index { set; get; }
        //缓存键
        public string Key { set; get; }
        //缓存值
        public string Value { set; get; }

        public string Expire { set; get; }

        public string Type { set; get; }
    }
}
