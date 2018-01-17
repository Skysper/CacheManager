using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Model;
using Microsoft.Extensions.Options;
using CacheManager.Service;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CacheManager.Web.Controllers
{
    public class AppController : Controller
    {
        private readonly AppSettings _settings;

        IAppService _appService;

        private const int UNLIMIT_PAGESIZE = 1000;

        public AppController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _appService = new AppService(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            var result = _appService.Search(string.Empty, 1, UNLIMIT_PAGESIZE);
            return View(result);
        }

        public IActionResult Delete(int id)
        {

            bool isOk = _appService.Remove(id);

            return Json(new JsonMsg(isOk ? MsgConst.OK : MsgConst.ERROR));
        }

        public IActionResult Save(int? id, string name, string des, int type, string constr)
        {

            if (!id.HasValue)
            {
                id = 0;
            }
            Model.App app = new App(id.Value, name, des, "", type, constr);
            bool isOk = _appService.Save(app);
            if (isOk)
            {
                return Json(new JsonMsg(MsgConst.OK));
            }
            else
            {
                return Json(new JsonMsg(MsgConst.ERROR));
            }
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
            var result = _appService.Search(key, pageIndex.Value, pageSize.Value);
            return Json(result);
        }


    }
}
