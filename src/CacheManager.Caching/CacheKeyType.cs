using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Caching
{
    /// <summary>
    /// use CacheKeyType for redis key type and memcache key type
    /// </summary>
    public enum CacheKeyType
    {
        None = 0,
        String = 1,
        List = 2,
        Set = 3,
        SortedSet = 4,
        Hash = 5,
        Unknown = 6
    }
}
