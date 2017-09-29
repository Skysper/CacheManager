using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    public class CacheFactory
    {
        private CacheFactory()
        {

        }

        public static ICache CreateCache(CacheType type,string conn)
        {
            switch (type)
            {
                case CacheType.Rediscache:
                    return new Rediscache(conn);
                case CacheType.Memcache:
                    return new Memcache(conn);
                default:
                    return null;

            }
        }
    }
}
