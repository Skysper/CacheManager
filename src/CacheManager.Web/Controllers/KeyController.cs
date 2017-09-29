using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CacheManager.Model;
using Microsoft.Extensions.Options;
using CacheManager.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CacheManager.Web.Controllers
{
    public class KeyController : Controller
    {
        private readonly AppSettings _settings;
        private CacheKeyRepository _repository;

        public KeyController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _repository = new CacheKeyRepository(_settings.ConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Remove(id);
                return Json(new { Ok = 1, Msg = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Ok = 0, Msg = ex.Message });
            }

        }

        public IActionResult Save(int id, string name, string descrption, int appId)
        {
            Model.CacheKey key = new CacheKey();
            key.AppId = appId;
            key.Id = id;
            key.Name = name;
            key.Description = descrption;

            try
            {
                if (key.Id <= 0)
                {
                    _repository.Add(key);
                }
                else
                {
                    _repository.Update(key);
                }
                return Json(new { Ok = 1, Msg = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Ok = 0, Msg = ex.Message });
            }
        }


        public IActionResult Search(string inputKey, int? appId, int? pageIndex, int? pageSize)
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
            return Json(_repository.FindByPage(appId.Value, inputKey, pageIndex.Value, pageSize.Value));
        }


        public IActionResult All(string ids)
        {
            List<Model.CacheKey> list = new List<CacheKey>();

            if (string.IsNullOrEmpty(ids))
            {
                return Json(list);
            }
            var array = ids.Split(',').Select(int.Parse).ToList();

            var nodes = _repository.FindByIds(array);
            return Json(nodes);
        }




    }
}
