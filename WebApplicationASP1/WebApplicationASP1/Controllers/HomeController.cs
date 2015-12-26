using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationASP1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return View();
            return RedirectToAction("Index", "drinksUser");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Per Jensen's company description page.";
            

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Per Jensen's contact page.";

            return View();
        }
    }
}