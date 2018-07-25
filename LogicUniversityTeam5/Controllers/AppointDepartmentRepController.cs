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
    public class AppointDepartmentRepController : Controller
    {
        IDepartmentService departmentService;       
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public AppointDepartmentRepController(DepartmentService ds)
        {
            departmentService = ds;         
        }

        // GET: AppointDepartmentRep
        public ActionResult AppointDepartmentRep(Department dep)
        {
            CombinedViewModel combinedView = new CombinedViewModel();
           
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

            string empId = "";

            if (user != null)
            {
                empId = user.EmployeeId;
            }
            else
            {
                //To change to error page after implementing login
                empId = "E009";
            }

            //To get Department of the employee
            //combinedView.DepartmentID = departmentService.getDepartmentID(empId);  
            
            combinedView.DepartmentID = departmentService.getDepartmentID("E009");
            combinedView.Employee = departmentService.getEmployeesOfDepartment(combinedView.DepartmentID);
            combinedView.AddedText = new List<string>(1) { "" };

            return View(combinedView);
        }

        [HttpPost]
        public ActionResult AppointDepartmentRep(CombinedViewModel model)
        {
           
            //get user from the login session
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());

                if (model.AddedText[0] != null)
                {
                Employee emp = departmentService.getEmployeeObject(model.AddedText[0]);
                DepartmentRepresentative departmentRepresentative = departmentService.getCurrentDepartmentRepresentative(emp.DepartmentID);             
                    departmentService.updateDepartmentRepresentative(departmentRepresentative.DeptRepID, emp.EmployeeID);
                }
           
            return RedirectToAction("AppointDepartmentRep", "AppointDepartmentRep", new { isAppointDepartmentRep = true });
        }
    }
}