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
        IAppService _appService;

        public HomeController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _appService = new AppService(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        private App _app;

        private IActionResult ValidateKeyAndAppInfo(string key, int? appId)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Json(new JsonMsg(MsgConst.ERROR, "键值为空"));
            }

            if (!appId.HasValue)
            {
                return Json(new JsonMsg(MsgConst.ERROR, "未获取到appId"));
            }
            _app = _appService.Find(appId.Value);

            if (_app == null)
            {
                return Json(new JsonMsg(MsgConst.ERROR, "未获取到app信息"));
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
            return Json(new JsonMsg(isOk ? MsgConst.OK : MsgConst.ERROR));

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
            return Json(new DataJsonMsg(MsgConst.OK, "", cacheResult));
        }

        public IActionResult Save(Model.KeySaveModel model)
        {
            if (model.AppId <= 0)
            {
                return Json(new JsonMsg(MsgConst.ERROR));
            }

            Model.App app = _appService.Find(model.AppId);
            if (app == null)
            {
                return Json(new JsonMsg(MsgConst.ERROR));
            }

            var cache = Caching.CacheFactory.Create(Caching.CacheType.Rediscache, app.ConnectionString);
            bool isOk = cache.Set(model.Key, model.Type, model.Value, model.TimeToLive);


            return Json(new JsonMsg(MsgConst.OK, ""));
        }


        public IActionResult Search(string key, int? pageIndex, int? pageSize, int? appId, bool? ignoreType,bool? ignoreNull)
        {
            if (!appId.HasValue)
            {
                return Content("[]");
            }

            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = 50;
            }
            if (!ignoreType.HasValue)
            {
                ignoreType = false;
            }

            if (!ignoreNull.HasValue) {
                ignoreNull = false;
            }

            Model.App app = _appService.Find(appId.Value);
            if (app == null)
            {
                return Content("[]");
            }

            ICacheService service = new CacheService();
            PagedDataJsonMsg result = service.Search(app, key, pageIndex.Value, pageSize.Value, ignoreType.Value, ignoreNull.Value);
            return Json(result);
        }

    }
}
