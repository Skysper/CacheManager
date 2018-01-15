using CacheManager.Caching;
using CacheManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Service
{
    public interface ICacheService
    {
        CacheResult Query(AppInfo app, string key);

        PagedDataJsonMsg Search(AppInfo app, string key, int pageIndex, int pageSize, bool ignoreType = true);
    }
}
