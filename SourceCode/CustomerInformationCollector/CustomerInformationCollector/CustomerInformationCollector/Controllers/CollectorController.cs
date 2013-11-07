using CustomerInformationCollector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace CustomerInformationCollector.Controllers
{
    public class CollectorController : Controller
    {
        // GET
        // /Collector/Stats
        [HttpGet]
        public ActionResult Stats()
        {
            var directoryInfo = new DirectoryInfo(this.HttpContext.Server.MapPath("/Files"));
            return View(new StatsModel()
            {
                FilesCount = directoryInfo.GetFiles().Count(),
                LastUpload = (directoryInfo.GetFiles().Any()) ? ((DateTime?)directoryInfo.GetFiles().Max(x => x.CreationTime)) : null
            });
        }
        
        
        // POST
        // /Collector/Upload
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            file.SaveAs(Path.Combine (this.HttpContext.Server.MapPath("/Files"), file.FileName + "." + Guid.NewGuid()));
            return Json("ok");
        }
    }
}
