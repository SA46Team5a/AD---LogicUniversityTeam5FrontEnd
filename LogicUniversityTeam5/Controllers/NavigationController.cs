using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Controllers;
using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using Microsoft.AspNet.Identity;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    public class NavigationController : Controller
    {
        IRequisitionService requisitionService;
        IDepartmentService departmentService;
        ChangeRoleController roleController;
        public NavigationController(RequisitionService rs, DepartmentService ds, UserManager<ApplicationUser> userManager)
        {
            requisitionService = rs;
            departmentService = ds;
            roleController = new ChangeRoleController(userManager);
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            if (User.IsInRole("Department Head"))
            {
                List<Requisition> pendingRequisitions = GetPendingRequisitionsForDeptHead();
                CombinedViewModel combinedViewModel = new CombinedViewModel();
                combinedViewModel.AddedNumbers = new List<int>(1) {
                    { pendingRequisitions.Count() }
                };

                return PartialView("_Navbar_DepartmentHead", combinedViewModel);

            }
            if (User.IsInRole("Delegate"))
            {
                string EmpId = User.Identity.GetEmployeeId();
                Employee employee = departmentService.getEmployeeById(EmpId);
                string DeptId = employee.DepartmentID;
                Authority currentAuth = departmentService.getDelegatedAuthority(DeptId);

                if (currentAuth.EmployeeID == EmpId)
                {
                    List<Requisition> pendingRequisitions = GetPendingRequisitionsForDeptHead();
                    CombinedViewModel combinedViewModel = new CombinedViewModel();
                    combinedViewModel.AddedNumbers = new List<int>(1) {
                        { pendingRequisitions.Count() } };
         
                    return PartialView("_Navbar_DepartmentHead", combinedViewModel);
                }
                else
                {
                    roleController.ChangeRoleOfUserToEmployee(EmpId);
                    CombinedViewModel combinedViewModel = new CombinedViewModel();
                    combinedViewModel.Employees = new List<Employee>(1) { { employee } };

                    return PartialView("_Navbar_Employee", combinedViewModel);
                }
                
            }
            if (User.IsInRole("Department Representative"))
            {
                return PartialView("_Navbar_DepartmentRepresentative");

            }
            if (User.IsInRole("Employee"))
            {
                string EmpId = User.Identity.GetEmployeeId();
                Employee employee = departmentService.getEmployeeById(EmpId);
                CombinedViewModel combinedViewModel = new CombinedViewModel();
                combinedViewModel.Employees = new List<Employee>(1) { { employee} };

                return PartialView("_Navbar_Employee", combinedViewModel);

            }
            if (User.IsInRole("Store Clerk"))
            {
                StationeryStoreEntities context = StationeryStoreEntities.Instance;
                
                int count = requisitionService.getCountOfOutstandingRequisitions();
                CombinedViewModel model = new CombinedViewModel();
                model.AddedNumbers = new List<int>(1) {
                    { count }
                };
                return PartialView("_Navbar_StoreClerk",model);

            }
            if (User.IsInRole("Store Manager"))
            {
                return PartialView("_Navbar_StoreManager");

            }
            if (User.IsInRole("Store Supervisor"))
            {
                int count = requisitionService.getCountOfOutstandingRequisitions();
                CombinedViewModel model = new CombinedViewModel();
                model.AddedNumbers = new List<int>(1) {
                    { count }
                };
                return PartialView("_Navbar_StoreSupervisor",model);

            }
            return PartialView("_Navbar_LoggedOut");
        }

        private List<Requisition> GetPendingRequisitionsForDeptHead()
        {
            string EmpId = User.Identity.GetEmployeeId();
            Employee employee = departmentService.getEmployeeById(EmpId);
            string DeptId = employee.DepartmentID;
            return requisitionService.getPendingRequisitionsOfDep(DeptId).ToList();
        }
    }
}