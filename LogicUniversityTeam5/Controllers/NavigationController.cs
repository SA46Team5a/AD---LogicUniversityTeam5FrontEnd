using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.IdentityHelper;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    public class NavigationController : Controller
    {
        IRequisitionService requisitionService;
        public NavigationController(RequisitionService rs)
        {
            requisitionService = rs;
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            if (User.IsInRole("Department Head"))
            {
                StationeryStoreEntities context = StationeryStoreEntities.Instance;

                string EmpId = User.Identity.GetEmployeeId();
                Employee employee = context.Employees.First(x => x.EmployeeID == EmpId);
                string DeptId = employee.DepartmentID;
                List<ServiceLayer.DataAccess.Requisition> pendingRequisitions =
                    requisitionService.getPendingRequisitionsOfDep(DeptId).ToList();
                CombinedViewModel combinedViewModel = new CombinedViewModel();
                combinedViewModel.AddedNumbers = new List<int>(1) {
                    { pendingRequisitions.Count() }
                };
                
                return PartialView("_Navbar_DepartmentHead",combinedViewModel);

            }
            if (User.IsInRole("Department Representative"))
            {
                return PartialView("_Navbar_DepartmentRepresentative");

            }
            if (User.IsInRole("Employee"))
            {
                return PartialView("_Navbar_Employee");

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
    }
}