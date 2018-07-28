using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Controllers;

namespace LogicUniversityTeam5.Controllers
{
    public class TestEmailController : Controller
    {
        // GET: TestEmail
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Index(int i=0)
        {
            EmailNotificationController emailNotificationController = new EmailNotificationController();
            
            
            emailNotificationController.SendtoConfirmDisbursement_Mobile("Meiting");
            return View();
        }
    }
}