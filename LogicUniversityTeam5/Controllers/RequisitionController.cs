using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers.Requisition
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageRequisition()
        {
            return View();
        }
    }
}