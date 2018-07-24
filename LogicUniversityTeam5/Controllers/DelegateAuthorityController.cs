using System;
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
    public class DelegateAuthorityController : Controller
    {
        IDepartmentService departmentService;
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public DelegateAuthorityController(DepartmentService ds)
        {
            departmentService = ds;
        }
        // GET: DelegateAuthority
        public ActionResult DelegateAuthority(Department dep)
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
                empId = "E008";
            }

            //To get Department of the employee
            //combinedView.DepartmentID = departmentService.getDepartmentID(empId);  

            combinedView.DepartmentID = departmentService.getDepartmentID("E008");
            string DeptID = combinedView.DepartmentID;
            combinedView.Employee = departmentService.getEmployeesOfDepartment(combinedView.DepartmentID);
            combinedView.AddedText = new List<string>(3) { "","","" };
            combinedView.IsSelected = new List<bool>(1) { false };
            

            return View(combinedView);
        }
        [HttpPost]
        public ActionResult DelegateAuthority(CombinedViewModel model)

        {

            //get user from the login session
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindById(User.Identity.GetUserId());
            string dateStart = model.AddedText[1];
            string dateEnd = model.AddedText[2];
            bool isRescind = model.IsSelected[0];

            if (model.AddedText[0] != null)
            {
                Employee emp = departmentService.getEmployeeObject(model.AddedText[0]);                  
               

                // departmentService.updateAuthority(authority);
                if (isRescind)
                {
                    //Hardcoded empId
                    user = new ApplicationUser();
                    user.EmployeeId = "E008";
                    string deptId = departmentService.getDepartmentID(user.EmployeeId);
                    Authority auth = departmentService.getCurrentAuthority(deptId);
                    departmentService.rescindAuthority(auth);
                }
                else
                {
                    Authority authority = departmentService.getCurrentAuthority(emp.DepartmentID);
                    departmentService.addAuthority(emp, Convert.ToDateTime(dateStart), Convert.ToDateTime(dateEnd));
                }
                 
            }


            return RedirectToAction("DelegateAuthority", "DelegateAuthority");
        }
    }
}