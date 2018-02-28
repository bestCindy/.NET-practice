using System;
using System.Web.Mvc;
using System.Text;
using System.Web.Script.Serialization;
using Coach.Services;
using System.Collections.Generic;

namespace Coach.Controllers
{
    public class IndexController : Controller
    {
        CheckService _service = new CheckService();
        public ActionResult Index()
        {
            return View("Index");
        }

        public JsonResult Login(string userName, string pwd)
        {
            IDictionary<string,string> rsdic = _service.indexMain(userName, pwd);
            JsonResult json = new JsonResult();
            json.Data = new
            {
                flag = rsdic["flag"],
                userName = rsdic["userName"],
                pwd = rsdic["pwd"]
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
