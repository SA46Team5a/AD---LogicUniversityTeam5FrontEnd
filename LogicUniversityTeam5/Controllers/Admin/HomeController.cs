using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TempData["FirstValue"] = "Hello World";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is About Page";
            if (TempData["FirstValue"] != null)
            {
                TempData.Keep();
                return RedirectToAction("Contact");
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}