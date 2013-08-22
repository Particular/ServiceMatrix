using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace $safeprojectname$.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Home/SendMessage

        public ActionResult SendMessage()
        {
            // TODO: Insert your send message code here!

            ViewBag.Message = "Message Sent!";

            return View("Index");
        }

    }
}
