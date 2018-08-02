﻿using System;
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
    [Authorize(Roles = "Department Head")]
    public class DelegateAuthorityController : Controller
    {
        IDepartmentService departmentService;
        ChangeRoleController roleController;
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public DelegateAuthorityController(DepartmentService ds, UserManager<ApplicationUser> userManager)
        {
            departmentService = ds;
            roleController = new ChangeRoleController(userManager);
        }
        // GET: DelegateAuthority
        public ActionResult DelegateAuthority(Department dep)
        {
            string empId = User.Identity.GetEmployeeId();
            CombinedViewModel combinedView = new CombinedViewModel();
            //To get Department of the employee
            combinedView.DepartmentID = departmentService.getDepartmentID(empId);

            string DeptID = combinedView.DepartmentID;
            combinedView.Employee = departmentService.getEligibleDelegatedAuthority(combinedView.DepartmentID);
            combinedView.Employee.Remove(departmentService.getCurrentDepartmentRepresentative(combinedView.DepartmentID).Employee);
            //AddedText[0] is the delegate, AddedText[0] is start date, AddedText[1] is end date
            combinedView.AddedText = new List<string>(3) { "","","" };
            combinedView.IsSelected = new List<bool>(1) { false };
            combinedView.Authorities = departmentService.getDelegatedAuthority(combinedView.DepartmentID);
            //combinedView.AddedText[1] = combinedView.Authorities.StartDate.ToString();
            //combinedView.AddedText[2] = combinedView.Authorities.EndDate.ToString();
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
                    departmentService.rescindAuthority(empId);
                    //To change email method to include the employeeID
                    EmailNotificationController.SendToLostApproveAuthority();
                    roleController.ChangeRoleOfUserToEmployee(emp.EmployeeID);

                    return RedirectToAction("DelegateAuthority", "DelegateAuthority", new { isRescind = true });
                }
                else
                {
                    Authority authority = departmentService.getDelegatedAuthority(emp.DepartmentID);
                    departmentService.addAuthority(emp, Convert.ToDateTime(dateStart), Convert.ToDateTime(dateEnd));
                    //To change email method to include the employeeID
                    EmailNotificationController.SendEmailToDelegatePerson();
                    roleController.ChangeRoleOfUserToDelegate(emp.EmployeeID);

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