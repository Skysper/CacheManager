using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Repository;
using CacheManager.Model;
using Microsoft.Extensions.Configuration;
using Microsoft​.Extensions​.Options;
using CacheManager.Service;
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

        private AppInfo _app;

        private IActionResult ValidateKeyAndAppInfo(string key, int? appId)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Json(new { Ok = 0, Msg = "键值为空" });
            }



            if (!appId.HasValue)
            {
                return Json(new { Ok = 0, Msg = "未获取到appId" });
            }
            _app = _repository.FindById(appId.Value);

            if (_app == null)
            {
                return Json(new { Ok = 0, Msg = "未获取到app信息" });
            }
            return null;
        }

        public IActionResult ClearCache(string key, int? appId)
        {
            IActionResult result = ValidateKeyAndAppInfo(key, appId);
            if (result != null)
            {
                return result;
            }

            var cache = Caching.CacheFactory.Create(Caching.CacheType.Rediscache, _app.ConnectionString);

            bool isOk = cache.Clear(key);

            cache.Close();
            return Json(new { Ok = isOk ? 1 : 0, Msg = "" });

        }

        public IActionResult More(string key, int? appId)
        {
            IActionResult result = ValidateKeyAndAppInfo(key, appId);
            if (result != null)
            {
                return result;
            }

            ICacheService service = new CacheService();
            Caching.CacheResult cacheResult = service.Query(_app, key);
            return Json(new { Ok = 1, Data = cacheResult, Msg = "" });
        }

        public IActionResult Save(Model.KeySaveModel model)
        {
            //
            if (model.AppId <= 0)
            {
                return Json(new JsonMsg() { Ok = 0, Msg = "" });
            }

            Model.AppInfo app = _repository.FindById(model.AppId);
            if (app == null)
            {
                return Json(MsgConst.GetErrorMsg());
            }

            var cache = Caching.CacheFactory.Create(Caching.CacheType.Rediscache, app.ConnectionString);
            bool isOk = cache.Set(model.Key, model.Type, model.Value, model.TimeToLive);


            return Json(MsgConst.GetOkMsg());
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

            var cache = Caching.CacheFactory.Create(Caching.CacheType.Rediscache, app.ConnectionString);

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

            var keyArray = keys.ToArray();
            var keyResult = cache.Query(keys.ToArray());
            var expireResult = cache.Expire(keys.ToArray());
            var typeResult = cache.Type(keys.ToArray());

            cache.Close();

            List<CacheResult> list = new List<CacheResult>();
            if (keyResult != null)
            {
                var resultList = keyResult.ToList();
                for (int i = 0; i < resultList.Count; i++)
                {
                    Caching.CacheResult cr = new Caching.CacheResult();
                    cr.Index = i + 1;
                    cr.Key = keys[i];
                    cr.Expire = expireResult[i].HasValue ? (expireResult[i].Value.TotalSeconds + "s") : "";
                    cr.Type = typeResult[i].ToString();
                    cr.Value = resultList[i];
                    list.Add(cr);
                }
            }

            //if the cache connection string is error
            //the cache result list will return null
            //then reset the key count to 0
            if (list == null || list.Count == 0)
            {
                keyCount = 0;
            }
            return Json(new { Count = keyCount, Data = list });
        }

    }
}
