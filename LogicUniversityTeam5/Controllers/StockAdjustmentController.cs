//using LogicUniversityTeam5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ServiceLayer;
//using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers
{
    // Author: Tan Khim Yang, Gong Meiting
    //public class StockAdjustmentController : Controller
    //{
    //    IStockManagementService stockManagementService;
    //    public StockAdjustmentController(IStockManagementService sms)
    //    {
    //        stockManagementService = sms;
    //    }

    //    // Path for testing connection to database. To be deleted.
    //    public ActionResult Test()
    //    {
    //        ViewBag.test = stockManagementService.getStockCountOfItem("C001");
    //        return View();
    //    }
        
    //    //IRetrieveStockManagementService stockRetrieveService;
    //    //IUpdateStockManagementService stockUpdateService;
    //    public ActionResult ManageMonthlyStockDiscrepancy()
    //    {
    //        //stockService =  new RetrieveStockManagementService();
    //        //List<StockVoucher> vouchers = stockService.getOpenVouchers();
    //        //combine.item = db.Items.ToList;
    //        //foreach(StockVoucher v in vouchers)
    //        //{ 
    //        //  combine.isSelected.Add(v.ItemId, false); }
    //        List<StockVoucher> vouchers = stockManagementService.getOpenVouchers();
    //        //ItemAndVoucher combine = new ItemAndVoucher();
    //        //combine.item = getitems();
    //        //combine.stockVoucher = getstockvoucher();
    //        //combine.isSelected = getIsSelected();
    //        return View(vouchers);
    //    }



    //    [HttpPost]
    //    public ActionResult ManageMonthlyStockDiscrepancy(LogicUniversityTeam5.Models.ItemAndVoucher model)
    //    {
    //        //stockUpdateService =  new UpdateStockManagementService();
    //        //foreach(KeyValuePair<int, false> entry in model.isSelected)
    //        //{ 
    //        //      if (entry.value == true){
    //        //      StockVoucher sv = db.StockVouchers.Where(x=>x.ItemId == entry.key);
    //        //      stockUpdateService.closeVoucher(sv);
    //        //}
    //        return RedirectToAction("Index","Home");
    //    }

    //    // Retrieving Mock Data from here
    //    public List<Items> getitems()
    //    {
    //        List<Item> items = new List<Item>();
    //        items.Add(
    //            new Item() { ItemName = "2B Pencil" });
    //        return items;
    //    }

    //    public List<StockVoucher> getstockvoucher()
    //    {
    //        List<StockVoucher> stock = new List<StockVoucher>();
    //        stock.Add(new StockVoucher()
    //        {
    //            ItemCost = 25,
    //            Reason = "damaged",
    //            RaisedBy = "Tonia",
    //        });
    //        return stock;
    //    }

    //    private Dictionary<int, bool> getIsSelected()
    //    {
    //        return new Dictionary<int, bool>() { { 1, false } };
    //    }
    //}  
}