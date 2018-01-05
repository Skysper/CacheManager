using CacheManager.Caching.Redis;
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

        public static ICache Create(CacheType type,string conn)
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

        public static ICache Create(int type, string conn) {
            CacheType cacheType = (CacheType)type;
            return Create(cacheType, conn); 
        }

    }
}
