using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.DataAccess;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers
{
    public class TrackOutstandingRequisitionController : Controller
    {
        StationeryStoreEntities context = StationeryStoreEntities.Instance;
        IDepartmentService departmentService;
        public TrackOutstandingRequisitionController(DepartmentService ds)
        {
            departmentService = ds;
        }
        // GET: TrackOutstandingRequisition
        public ActionResult TrackOutsandingRequisition()
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.OutstandingRequisitionViews = context.OutstandingRequisitionViews.ToList();
            combinedViewModel.Departments = departmentService.getDepartments();
            combinedViewModel.Requisitions = new List<ServiceLayer.DataAccess.Requisition>();


            return View(combinedViewModel);
        }
    }
}