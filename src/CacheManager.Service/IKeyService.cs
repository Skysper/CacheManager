using System;
using System.Collections.Generic;
using System.Text;

namespace CacheManager.Service
{
    public interface IKeyService
    {
        List<Model.CacheKey> All(string ids);
        List<Model.CacheKey> Search(string key, int appId, int pageIndex, int pageSize);
        bool Save(int id, string name, string description, int appId);
        bool Remove(int id);
        bool Save(Model.CacheKey key);

    }
}
