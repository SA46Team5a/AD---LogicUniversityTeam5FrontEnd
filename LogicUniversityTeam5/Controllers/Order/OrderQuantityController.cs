using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class OrderQuantityController : Controller
    {
        // GET: OrderQuantity
        public ActionResult OrderQuantity(ItemCatalogue passeditem)
        {
            
            return View();
        }
        public ActionResult RequestNPD(string type)
        {
            string q = Request.QueryString["type1"];
            ViewBag.test = q;


            return View();

        }
    }
}