using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ServiceLayer;
using ServiceLayer.DataAccess;
using System.Net;
using System.Net.Mail;



namespace LogicUniversityTeam5.Controllers
{
    [Authorize(Roles = "Department Head,Delegate")]
    public class AppointDepartmentRepController : Controller
    {
        IDepartmentService departmentService;       
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;
        private readonly UserManager<ApplicationUser> _userManager;
        ChangeRoleController roleController;

        public AppointDepartmentRepController(DepartmentService ds, UserManager<ApplicationUser> userManager)
        {
            departmentService = ds;
            _userManager = userManager;
            roleController = new ChangeRoleController(userManager);
        }

        // GET: AppointDepartmentRep
        public ActionResult AppointDepartmentRep(Department dep)
        {
            CombinedViewModel combinedView = new CombinedViewModel();

            string empId = User.Identity.GetEmployeeId();

            //To get Department of the employee
            combinedView.DepartmentID = departmentService.getDepartmentID(empId);
            combinedView.Employee = departmentService.getEligibleDepartmentRepresentatives(combinedView.DepartmentID);
            combinedView.DepartmentRepresentative = new List<DepartmentRepresentative>(1)
            {
                {departmentService.getCurrentDepartmentRepresentative(combinedView.DepartmentID)}
            };
            combinedView.AddedText = new List<string>(1) { "" };

            return View(combinedView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AppointDepartmentRep(CombinedViewModel model, string confirm)
        {
           
            //get user from the login session
            string empId = User.Identity.GetEmployeeId();

            if (confirm != null)
            {
                Employee neweDepRepEmployee = departmentService.getEmployeeObject(confirm);
                DepartmentRepresentative departmentRepresentative = departmentService.getCurrentDepartmentRepresentative(neweDepRepEmployee.DepartmentID);
                string oldDepRepEmployeeId = departmentRepresentative.EmployeeID;
                departmentService.updateDepartmentRepresentative(departmentRepresentative.DeptRepID, neweDepRepEmployee.EmployeeID);
                //EmailNotificationController.SendEmailToAppointingDepRep(departmentRepresentative.Passcode);
                
                roleController.ChangeRoleOfUserToDepartmentRep(neweDepRepEmployee.EmployeeID);
                roleController.ChangeRoleOfUserToEmployee(oldDepRepEmployeeId);

            }
            return RedirectToAction("AppointDepartmentRep", "AppointDepartmentRep", new { isAppointDepartmentRep = true });
        }

    }
}