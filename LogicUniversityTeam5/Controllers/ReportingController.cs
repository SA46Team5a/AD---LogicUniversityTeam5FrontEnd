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
    public class ReportingController : Controller
    {
        private readonly IClassificationService classificationService;
        private readonly IDepartmentService departmentService;
        private readonly IOrderService orderService;

        public ReportingController(IClassificationService classificationService, IDepartmentService departmentService, IOrderService orderService)
        {
            this.classificationService = classificationService;
            this.departmentService = departmentService;
            this.orderService = orderService;
        }

        // GET: Reporting
        public ActionResult ReorderCostAnalysisReport()
        {
            ReportingModel reportingModel = new ReportingModel();
            reportingModel.categories = classificationService.GetCategories();
            reportingModel.suppliers = orderService.getSuppliers();
            return View(reportingModel);
        }

        public ActionResult RequisitionCostAnalysisReport()
        {
            ReportingModel reportingModel = new ReportingModel();
            reportingModel.categories = classificationService.GetCategories();
            reportingModel.departments = departmentService.getDepartments();
            return View(reportingModel);
        }
    }
}