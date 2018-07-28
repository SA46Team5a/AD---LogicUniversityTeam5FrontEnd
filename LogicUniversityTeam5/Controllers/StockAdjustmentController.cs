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

        public StockAdjustmentController(IStockManagementService sms)
        {
            stockManagementService = sms;
        }

        public ActionResult ManageMonthlyStockDiscrepancy()
        {
            CombinedViewModel combinedView = new CombinedViewModel();
            // TODO: get role of user
            combinedView.StockVouchers = stockManagementService.getOpenVouchers(true);
            combinedView.IsSelected = new List<bool>();
            foreach (StockVoucher voucher in combinedView.StockVouchers)
            {
                combinedView.IsSelected.Add(false);
            }

            return View(combinedView);
        }



        [HttpPost]
        public ActionResult ManageMonthlyStockDiscrepancy(LogicUniversityTeam5.Models.ItemAndVoucher model)
        {
            //stockUpdateService =  new UpdateStockManagementService();
            //foreach(KeyValuePair<int, false> entry in model.isSelected)
            //{ 
            //      if (entry.value == true){
            //      StockVoucher sv = db.StockVouchers.Where(x=>x.ItemId == entry.key);
            //      stockUpdateService.closeVoucher(sv);
            //}
            return RedirectToAction("Index", "Home");
        }

    }
}