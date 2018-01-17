using CacheManager.Model;
using System;
using System.Collections.Generic;

namespace CacheManager.Service
{
    public interface IAppService
    {
        List<App> Search(string key, int pageIndex, int pageSize);
        bool Save(App app);

        bool Remove(int id);

        App Find(int id);
    }
}
