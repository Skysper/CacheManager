using CacheManager.Caching;
using CacheManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Service
{
    public interface ICacheService
    {
        bool Clear(App app, string key);
        bool Clear(App app, List<string> keys);
        CacheResult Query(App app, string key);

        PagedDataJsonMsg Search(App app, string key, int pageIndex, int pageSize, bool ignoreTTL = false, bool ignoreNull = false);
    }
}
