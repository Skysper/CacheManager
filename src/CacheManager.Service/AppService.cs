using CacheManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheManager.Service
{
    public class AppService : IAppService
    {
        CacheManager.Repository.AppRepository _repos;

        public AppService(string connectionString)
        {
            _repos = new Repository.AppRepository(connectionString);
        }

        public List<App> Search(string key, int pageIndex, int pageSize)
        {
            var result = _repos.FindByPage(key, pageIndex, pageSize);
            return result.ToList();
        }

        public bool Save(App app)
        {
            if (string.IsNullOrEmpty(app.Identity))
            {
                app.Identity = Guid.NewGuid().ToString();
            }
            try
            {
                if (app.Id == 0)
                {
                    _repos.Add(app);
                }
                else
                {
                    _repos.Update(app);
                }
                return true;
            }
            catch
            {
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
            catch
            {
                return false;
            }
        }

        public App Find(int id)
        {
            try
            {
                return _repos.FindById(id);
            }
            catch
            {
                return null;
            }
        }

    }
}
