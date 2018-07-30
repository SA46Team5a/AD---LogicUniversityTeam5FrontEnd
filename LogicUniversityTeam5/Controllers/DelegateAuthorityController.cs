using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.IdentityHelper;
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
            string empId = User.Identity.GetEmployeeId();
            CombinedViewModel combinedView = new CombinedViewModel();
            //To get Department of the employee
            combinedView.DepartmentID = departmentService.getDepartmentID(empId);

            string DeptID = combinedView.DepartmentID;
            combinedView.Employee = departmentService.getEmployeesOfDepartment(combinedView.DepartmentID);

            //AddedText[0] is the delegate, AddedText[0] is start date, AddedText[1] is end date
            combinedView.AddedText = new List<string>(3) { "","","" };
            combinedView.IsSelected = new List<bool>(1) { false };
            combinedView.Authorities = departmentService.getCurrentAuthority(combinedView.DepartmentID);

            return View(combinedView);
        }

        [HttpPost]
        public ActionResult DelegateAuthority(CombinedViewModel model)
        {

            //get user from the login session
            string empId = User.Identity.GetEmployeeId();
            string dateStart = model.AddedText[1];
            string dateEnd = model.AddedText[2];
            bool isRescind = model.IsSelected[0];

            if (model.AddedText[0] != null)
            {
                Employee emp = departmentService.getEmployeeObject(model.AddedText[0]);                  

                if (isRescind)
                {
                    string deptId = departmentService.getDepartmentID(empId);
                    Authority auth = departmentService.getCurrentAuthority(deptId);                   
                    departmentService.rescindAuthority(auth);
                    //To change email method to include the employeeID
                    EmailNotificationController.SendToLostApproveAuthority();

                    return RedirectToAction("DelegateAuthority", "DelegateAuthority", new { isRescind = true });
                }
                else
                {
                    Authority authority = departmentService.getCurrentAuthority(emp.DepartmentID);
                    departmentService.addAuthority(emp, Convert.ToDateTime(dateStart), Convert.ToDateTime(dateEnd));
                    //To change email method to include the employeeID
                    EmailNotificationController.SendEmailToDelegatePerson();

                    return RedirectToAction("DelegateAuthority", "DelegateAuthority", new { isDelegateAuthority = true });
                }
                 
            }
            else
            {
                return RedirectToAction("DelegateAuthority", "DelegateAuthority");
            }                       

        }
    }
}