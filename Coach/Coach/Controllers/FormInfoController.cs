using System;
using System.Web.Mvc;
using System.Text;
using System.Web.Script.Serialization;
using Coach.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;
using System.Collections;

namespace Coach.Controllers
{
    public class FormInfoController : Controller
    {
        CheckService _service = new CheckService();
        public ActionResult FormInfo()
        {
            return View("FormInfo");
        }
        //检查用户名和密码
        public JsonResult CheckSession(string userName, string pwd)
        {
            IDictionary<string, string> rsdic = _service.FormInfo(userName, pwd);
            JsonResult json = new JsonResult();
            json.Data = new
            {
                flag = rsdic["flag"],
                userName = rsdic["userName"],
                pwd = rsdic["pwd"]
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        //新入职人员信息文件
        public JsonResult getPersonnelInfo()
        {
            var rsid = _service.getPersonnelInfo();
            return Json(rsid, JsonRequestBehavior.AllowGet);
        }
        //离职人员信息文件
        public JsonResult getResignationInfo()
        {
            object rsid = _service.getResignationInfo();
            return Json(rsid, JsonRequestBehavior.AllowGet);
        }
        //一次性津贴文件
        public JsonResult getOnetimeAllowance()
        {
            object rsid = _service.getOnetimeAllowance();
            return Json(rsid, JsonRequestBehavior.AllowGet);
        }
        //固定津贴信息文件
        public JsonResult getFixedAllowance()
        {
            object rsid = _service.getFixedAllowance();
            return Json(rsid, JsonRequestBehavior.AllowGet);
        }
        //薪资变动文件
        public JsonResult getSalaryChange()
        {
            object rsid = _service.getSalaryChange();
            return Json(rsid, JsonRequestBehavior.AllowGet);
        }
        //邮件重发
        public JsonResult SentMail(string fileName, string dateStartString, string dateEndString)
        {
            string con = Server.MapPath(@"~\Config.xml");
            string mailJudge = _service.SentMail(fileName, con, dateStartString, dateEndString);
            return Json(mailJudge,JsonRequestBehavior.AllowGet);
        }
        //上传文件
        string backMessage;
        [HttpPost]
        public JsonResult uploadFile()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["file"];
                DataTable dtExcel;
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~//upload//" + fileName));
                    string logPath = Path.Combine(HttpContext.Server.MapPath("~//log//"));
                    file.SaveAs(filePath);//将文件保存到路径
                    ArrayList msg = _service.compareExcel(fileName, filePath);
                    if (msg[0].ToString() != "")
                    {
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }else {
                        dtExcel = (DataTable)msg[1];
                    }
                    backMessage = _service.UploadFile(fileName,filePath,logPath,dtExcel);
                }
                return Json(backMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
