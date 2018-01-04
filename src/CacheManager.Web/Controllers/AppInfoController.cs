using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Model;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CacheManager.Web.Controllers
{
    public class AppInfoController : Controller
    {
        private readonly AppSettings _settings;
        CacheManager.Repository.AppInfoRepository _repos;

        private const int UNLIMIT_PAGESIZE = 1000;

        public AppInfoController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;

            _repos = new Repository.AppInfoRepository(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            var result = SearchApps(string.Empty, 1, UNLIMIT_PAGESIZE);
            return View(result);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _repos.Remove(id);
                return Json(new { Ok = 1, Msg = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Ok = 0, Msg = ex.Message });
            }
        }

        public IActionResult Save(int? id, string name, string des, int type, string constr)
        {
            try
            {
                if (!id.HasValue)
                {
                    id = 0;
                }
                Model.AppInfo app = new AppInfo();
                app.Id = id.Value;
                app.Name = name;
                app.Description = des;
                app.Type = type;
                app.ConnectionString = constr;
                if (string.IsNullOrEmpty(app.Identity))
                {
                    app.Identity = Guid.NewGuid().ToString();
                }

                if (app.Id == 0)
                {
                    _repos.Add(app);
                }
                else
                {
                    _repos.Update(app);
                }

                return Json(new { Ok = 1, Msg = "" });

            }
            catch (Exception ex)
            {
                return Json(new { Ok = 0, Msg = ex.Message });
            }
        }

        public IActionResult All(string key, int? pageIndex, int? pageSize)
        {
            var result = _repos.FindAll().ToList();
            return Json(result);
        }

        public IActionResult Search(string key, int? pageIndex, int? pageSize)
        {
            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = UNLIMIT_PAGESIZE;
            }
            var result = SearchApps(string.Empty, pageIndex.Value, pageSize.Value);
            return Json(result);
        }

        private List<AppInfo> SearchApps(string key, int pageIndex, int pageSize)
        {
            var result = _repos.FindByPage(key, pageIndex, pageSize);
            return result.ToList();
        }


    }
}
