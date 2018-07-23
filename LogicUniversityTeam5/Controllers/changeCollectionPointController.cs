using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceLayer;
using ServiceLayer.DataAccess;


namespace LogicUniversityTeam5.Controllers
{
    public class ChangeCollectionPointController : Controller
    {
        IDepartmentService departmentService;
        IClassificationService classificationService;
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ChangeCollectionPointController(DepartmentService ds,ClassificationService cs)
        {
            departmentService = ds;
            classificationService = cs;
        }
        // GET: CollectionPoint
        public ActionResult changeCollectionPoint(Department dep)
        {

            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.CollectionPoint = classificationService.GetCollectionPoints();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

            string empId = "";

            if(user!= null)
            {
                empId = user.EmployeeId;
            }
            else
            {
                //To change to error page after implementing login
                empId = "E017";
            }
           
            combinedView.DepartmentRepresentative = new List<DepartmentRepresentative>();
            Employee employee = context.Employees.Where(e => e.EmployeeID.Equals(empId)).First();
            DepartmentRepresentative departmentRepresentative = context.DepartmentRepresentatives
                                        .Where(e => e.EmployeeID == employee.EmployeeID).First();
            combinedView.DepartmentRepresentative.Add(departmentRepresentative);
            combinedView.AddedText = new List<string>(1) { "" };

            return View(combinedView);
        }


        [HttpPost]
        public ActionResult changeCollectionPoint(CombinedViewModel model)
        {
            //User user = session.getUser();
            List<CollectionPoint> collectionPoint = model.CollectionPoint;
            List<bool> isSelected = model.IsSelected;            
            
            //get user from the login session
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

            //get the empId from user
            //string deptID=departmentService.getDepartmentID(user.EmployeeId);
            string deptID = departmentService.getDepartmentID("E017");

            for (int i=0;i<model.AddedText.Count;i++)
            {
                if(model.AddedText[0] != null)
                {
                    departmentService.updateCollectionPoint(deptID, Convert.ToInt32(model.AddedText[0]));

                }
            }
            //ViewData["Sucess"] = "Changed collectionPoint successfully";
            return RedirectToAction("changeCollectionPoint", "ChangeCollectionPoint");
            ViewData["Sucess"] = "Changed collectionPoint successfully";
        }
    }
}