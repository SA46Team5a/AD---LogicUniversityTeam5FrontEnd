using LogicUniversityTeam5.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogicUniversityTeam5.Controllers
{


    public class StockAdjustmentController : Controller
    {
        //IRetrieveStockManagementService stockRetrieveService;
        //IUpdateStockManagementService stockUpdateService;
        public ActionResult ManageMonthlyStockDiscrepancy()
        {
            //stockService =  new RetrieveStockManagementService();
            //List<StockVoucher> vouchers = stockService.getOpenVouchers();
            //combine.item = db.Items.ToList;
            //foreach(StockVoucher v in vouchers)
            //{ ;
            //  combine.isSelected.Add(new Dictionary<int,bool>{
            //     {v.ItemId, false }
            //  }); }
            ItemAndVoucher combine = new ItemAndVoucher();
            combine.item = getitems();
            combine.stockVoucher = getstockvoucher();
            return View(combine);
        }

        [HttpPost]
        public ActionResult ManageMonthlyStockDiscrepancy(ItemAndVoucher model)
        {
            //stockUpdateService =  new UpdateStockManagementService();
            //List<StockVoucher> vouchers = stockService.getOpenVouchers();
            //foreach(StockVoucher v in vouchers)
            //{ combine.item.Add(db.Items.ItemName)}
            ItemAndVoucher combine = new ItemAndVoucher();
            combine.item = getitems();
            combine.stockVoucher = getstockvoucher();
            return View("Home");
        }

        // Retrieving Mock Data from here
        public List<Items> getitems()
        {
            List <Items> items = new List<Items>();
            items.Add(
                new Items() { ItemName = "2B Pencil" });
            return items;
        }

        public List<StockVoucher> getstockvoucher()
        {
            List<StockVoucher> stock = new List<StockVoucher>();
            stock.Add(new StockVoucher() { discrepancyId = "Z123", actualcount = 95, originalcount = 100,
                ItemCost = 25, Reason = "damaged", RaisedBy = "Tonia", isNewlyEnrolled = true, RaisedByDate = "18-07-2018",
            Password = "password"
            });
            return stock;
        }
    }  
}