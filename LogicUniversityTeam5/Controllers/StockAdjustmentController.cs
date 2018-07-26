//using LogicUniversityTeam5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers
{
    // Author: Tan Khim Yang, Gong Meiting
    public class StockAdjustmentController : Controller
    {
        IStockManagementService stockManagementService;
        static StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public StockAdjustmentController(StockManagementService sms)
        {
            stockManagementService = sms;
        }

        public ActionResult ManageMonthlyStockDiscrepancy()
        {

            CombinedViewModel combinedView = new CombinedViewModel();
            combinedView.StockVouchers = stockManagementService.getOpenVouchers();
            combinedView.IsSelected = new List<bool>();
            return View(combinedView);
        }

        [HttpPost]
        public ActionResult ManageMonthlyStockDiscrepancy(CombinedViewModel model)
        {
            //User user = session.getUser();

            List<StockVoucher> openVouchers = model.StockVouchers;
            List<bool> isSelected = model.IsSelected;
       
            for(int i =0; i<openVouchers.Count; i++)
            {
                if (isSelected[i] == true)
                {
                    //string userId = User.getId();
                    int id = openVouchers[i].DiscrepancyID;
                    stockManagementService.closeVoucher(id,"E012","damaged");
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}