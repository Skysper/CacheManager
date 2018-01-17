using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheManager.Service
{
    public class KeyService : IKeyService
    {
        CacheManager.Repository.CacheKeyRepository _repos;

        public KeyService(string connectionString)
        {
            _repos = new Repository.CacheKeyRepository(connectionString);
        }

        public List<Model.CacheKey> All(string ids)
        {
            List<Model.CacheKey> list = null;
            if (string.IsNullOrEmpty(ids))
            {
                list = new List<Model.CacheKey>();
                return list;
            }
            else
            {
                var array = ids.Split(',').Select(int.Parse).ToList();
                var nodes = _repos.FindByAppIds(array);
                return nodes;
            }
        }

        public List<Model.CacheKey> Search(string key, int appId, int pageIndex, int pageSize)
        {
            return _repos.FindByPage(appId, key, pageIndex, pageSize);
        }

        public bool Save(int id, string name, string description, int appId)
        {
            Model.CacheKey key = new Model.CacheKey(id, name, description, appId);
            return Save(key);
        }

        public bool Save(Model.CacheKey key)
        {
            if (key == null || string.IsNullOrEmpty(key.Name) || key.AppId == 0)
            {
                return false;
            }
            try
            {
                if (key.Id <= 0)
                {
                    _repos.Add(key);
                }
                else
                {
                    _repos.Update(key);
                }
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Remove(int id)
        {
            try
            {
                _repos.Remove(id);
                return true;
            }
            catch {
                return false;
            }
        }

    }
}
