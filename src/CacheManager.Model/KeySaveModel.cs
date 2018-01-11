using CacheManager.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Model
{
    public class KeySaveModel
    {
        public int AppId { set; get; }
        public String Key { set; get; }
        public String Value { set; get; }
        public int TimeToLive { set; get; }
        public CacheKeyType Type { set; get; }


    }
}
