using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Repository;
using CacheManager.Model;
using Microsoft.Extensions.Configuration;
using Microsoft​.Extensions​.Options;
using CacheManager.Caching;

namespace CacheManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _settings;
        CacheManager.Repository.AppInfoRepository _repository;

        public HomeController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _repository = new AppInfoRepository(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ClearCache(string key, int? appId)
        {
            if (!appId.HasValue || string.IsNullOrEmpty(key))
            {
                return Json(new { Ok = 0, Msg = "未获取appId" });
            }

            Model.AppInfo app = _repository.FindById(appId.Value);
            if (app == null)
            {
                return Json(new { Ok = 0, Msg = "未获取到app信息" });
            }

            var cache = Caching.CacheFactory.CreateCache(Caching.CacheType.Rediscache, app.ConnectionString);

            bool isOk = cache.Clear(key);
            return Json(new { Ok = isOk ? 1 : 0, Msg = "" });

        }


        public IActionResult Search(string key, int? pageIndex, int? pageSize, int? appId)
        {
            if (!appId.HasValue)
            {
                return Content("[]");
            }

            Model.AppInfo app = _repository.FindById(appId.Value);
            if (app == null)
            {
                return Content("[]");
            }

            var cache = Caching.CacheFactory.CreateCache(Caching.CacheType.Rediscache, app.ConnectionString);

            var ra = new RangeAnalysis(key);
            string format = ra.Format();
            List<Ranges> ranges = ra.Analysis();

            int keyCount = 0;
            List<string> keys = new List<string>();
            if (ranges.Count > 0)
            {
                if (!pageIndex.HasValue)
                {
                    pageIndex = 1;
                }
                if (!pageSize.HasValue)
                {
                    pageSize = 50;
                }
                keys = KeyManager.GetPagedKeys(format, ranges, pageIndex.Value, pageSize.Value);
                keyCount = KeyManager.GetKeysCount(ranges);
            }
            else
            {
                keys.Add(key);
                keyCount = 1;
            }

            var queryResult = cache.Query(keys.ToArray());
            List<CacheResult> list = new List<CacheResult>();
            if (queryResult != null)
            {
                var resultList = queryResult.ToList();
                for (int i = 0; i < resultList.Count; i++)
                {
                    Caching.CacheResult cr = new Caching.CacheResult();
                    cr.Index = i + 1;
                    cr.Key = keys[i];
                    cr.Value = resultList[i];
                    list.Add(cr);
                }
            }
            //

            return Json(new { Count = keyCount, Data = list });
        }

    }
}
