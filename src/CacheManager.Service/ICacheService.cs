using CacheManager.Caching;
using CacheManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Service
{
    public interface ICacheService
    {
        CacheResult Query(App app, string key);

        PagedDataJsonMsg Search(App app, string key, int pageIndex, int pageSize, bool ignoreTTL = false, bool ignoreNull = false);
    }
}
