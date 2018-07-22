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

            //StockVoucher v = context.StockVouchers.Where(x => x.DiscrepancyID == 11).First();
            //combinedView.StockVouchers = new List<StockVoucher>()
            //{
            //    new StockVoucher{ DiscrepancyID = 1, ItemID="C001",OriginalCount=10,ActualCount=5,ItemCost=4,Reason="Damaged",RaisedBy="E017",ApprovedBy="E008",RaisedByDate=Convert.ToDateTime("19/7/2018"),ApprovedDate=Convert.ToDateTime("23/7/2018") },
            //    new StockVoucher{ DiscrepancyID = 1, ItemID="C001",OriginalCount=10,ActualCount=8,ItemCost=4,Reason="Damaged",RaisedBy="E017",ApprovedBy="E008",RaisedByDate=Convert.ToDateTime("19/7/2018"),ApprovedDate=Convert.ToDateTime("23/7/2018")},
            //    v
            //};
            
            combinedView.IsSelected = new List<bool>();
            foreach (StockVoucher voucher in combinedView.StockVouchers)
            {
                combinedView.IsSelected.Add(false);
            }

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
                    stockManagementService.closeVoucher(id,"E012");
                }
            }

            //StockVoucher newVoucher = context.StockVouchers.Where(x => x.DiscrepancyID == 10).First();
            //string emp = newVoucher.ApprovedBy;
            //decimal d = newVoucher.ItemCost;
            //    CombinedViewModel combinedView = new CombinedViewModel();
            //    if (combinedView.IsSelected== true)
            //    {
            //        ViewBag.Message = "Selected";
            //        return View();
            //    }
            //    else (FlatFile == false)
            //{
            //        ViewBag.Message = "Not selected";
            //        return View();
            //    }
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