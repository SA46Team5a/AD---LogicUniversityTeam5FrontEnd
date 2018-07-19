using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers.Requisition
{
    public class RequisitionController : Controller
    {
        IStockManagementService iStockService;
        IClassificationService iClassService;
        public RequisitionController(StockManagementService sms, ClassificationService cs)
        {
            iStockService = sms;
            iClassService = cs;
        }

       
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageRequisition()
        {
            return View();
        }

        public ActionResult ViewStationeryCatalogue()
        {
            List<StockCountItem> itemList = iStockService.getAllStockCountItem();            
            List<Category> catList = iClassService.GetCategories();            
            ViewBag.dropdowncat = new SelectList(catList, "categoryId", "categoryName");
            return View(itemList);
        }
    }
}