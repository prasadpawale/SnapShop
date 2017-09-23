using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace SnapShop.Design.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TopDeals()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            foreach (string file1 in Request.Files)
            {
                var postedFile = Request.Files[file1];
                string fileUploadedPath = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));
                //Run(fileUploadedPath).Wait();

            }



            //if (file != null && file.ContentLength > 0) try
            //    {
            //        string path = Path.Combine(Server.MapPath("~/Files"),
            //        Path.GetFileName(file.FileName));
            //        file.SaveAs(path);
            //        return Json(new
            //        {
            //            Error = false,
            //            Message = "File Uploaded Successfully..!!!",
            //            FilePath = file.FileName
            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new
            //        {
            //            Error = false,
            //            Message = "File Not Uploaded"
            //        }, JsonRequestBehavior.AllowGet);
            //    }
            //else
            //{
            //    ViewBag.Message = "You have not specified a file.";
            //}
            return View();
        }
    }
}