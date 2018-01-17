using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Model;
using Microsoft.Extensions.Options;
using CacheManager.Repository;
using CacheManager.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CacheManager.Web.Controllers
{
    public class KeyController : Controller
    {
        private readonly AppSettings _settings;
        private IKeyService _keyService;

        public KeyController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _keyService = new KeyService(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            bool isOk = _keyService.Remove(id);
            return Json(new JsonMsg(isOk ? MsgConst.OK : MsgConst.ERROR));
        }

        public IActionResult Save(int id, string name, string description, int appId)
        {
            bool isOk = _keyService.Save(id, name, description, appId);
            return Json(new JsonMsg(isOk ? MsgConst.OK : MsgConst.ERROR));
        }


        public IActionResult Search(string key, int? appId, int? pageIndex, int? pageSize)
        {
            if (!appId.HasValue)
            {
                appId = 0;
            }

            //set pageSize to 1000 first,
            //then we will add the pager function for the page
            if (!pageSize.HasValue)
            {
                pageSize = 1000;
            }
            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            return Json(_keyService.Search(key, appId.Value, pageIndex.Value, pageSize.Value));
        }

        public IActionResult All(string ids)
        {
            var nodes = _keyService.All(ids);
            return Json(nodes);
        }

    }
}
