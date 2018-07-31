//using LogicUniversityTeam5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.IdentityHelper;
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

            if(User.IsInRole("Store Manager"))
            {
                combinedView.StockVouchers = stockManagementService.getOpenVouchers(true);
            }
            else if(User.IsInRole("Store Supervisor"))
            {
                combinedView.StockVouchers = stockManagementService.getOpenVouchers(false);
            }

            combinedView.StockVouchers = stockManagementService.getOpenVouchers(true);
            combinedView.IsSelected = new List<bool>();
            foreach (StockVoucher voucher in combinedView.StockVouchers)

            combinedView.IsSelected = new List<bool>(combinedView.StockVouchers.Count);
            combinedView.StockVouchers.ForEach(sv => combinedView.IsSelected.Add(false));
            return View(combinedView);
        }

        [HttpPost]
        public ActionResult ManageMonthlyStockDiscrepancy(CombinedViewModel model)
        {
            string approverEmpId = User.Identity.GetEmployeeId();

            List<StockVoucher> openVouchers = model.StockVouchers;
            List<bool> isSelected = model.IsSelected;
       
            for(int i =0; i<openVouchers.Count; i++)
            {
                if (isSelected[i] == true)
                {
                    int stockVoucherId = openVouchers[i].DiscrepancyID;
                    string discrepancyReason = openVouchers[i].Reason;
                    stockManagementService.closeVoucher(stockVoucherId, approverEmpId, discrepancyReason); 
                }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}