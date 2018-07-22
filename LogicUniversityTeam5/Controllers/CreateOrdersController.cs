using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class CreateOrdersController : Controller
    {
        IStockManagementService _stockManagementService;
        //IOrderService orderService;
        StationeryStoreEntities context;

        public CreateOrdersController(StockManagementService sms)
        {
            _stockManagementService = sms;
            //orderService = os;
            context = StationeryStoreEntities.Instance;
        }

        public ActionResult ItemCatalogue()
        {
            ItemCatalogueModel itemcatalogue = new ItemCatalogueModel();
            itemcatalogue.items = _stockManagementService.getAllItems();
            //itemcatalogue.categories = getcategory();
            itemcatalogue.stocklevels = getstocklevel();
            
            return View(itemcatalogue);
        }

        [HttpPost]
        public ActionResult ItemCatalogue(ItemCatalogueModel item)
        {
            ItemCatalogueModel value = new ItemCatalogueModel();
            value = item;
            return RedirectToAction("OrderQuantity", "OrderQuantity", new { type1 = item });
        }

        public ActionResult PlaceOrder()
        {

            CombinedViewModel combinedViewModel = new CombinedViewModel();
            //List<string> itemIds = (List<string>) TempData["itemIds"];
            //combinedViewModel.supplierItems = orderService.getSupplierItemsOfItemIds(itemIds);
            List<string> itemIds = new List<string>()
            {
                "C001","C002","C003"
            };

            combinedViewModel.suppliers = new List<Supplier>();
            combinedViewModel.supplierItems = new List<SupplierItem>()
            {
                new SupplierItem{SupplierItemID=1 , SupplierID="ALPA" , ItemID="C001", Rank=1, Cost=3.50m},
                new SupplierItem{SupplierItemID=2 , SupplierID="ALPA" , ItemID="C002", Rank=1, Cost=4.50m},
                new SupplierItem{SupplierItemID=3 , SupplierID="ALPA" , ItemID="C003", Rank=1, Cost=5.50m}
            };
            foreach (SupplierItem supplierItem in combinedViewModel.supplierItems)
            {
                combinedViewModel.suppliers.Add(context.Suppliers.Where(x => x.SupplierID == supplierItem.SupplierID).First());
            }
            
            return View(combinedViewModel);
        }

        public List<Items> getitem()
        {
            List<Items> items = new List<Items>();
            items.Add(
                new Items() { ItemName = "2B Pencil", ItemID = "Z123", CartId = "A123", UnitOfMeasure = "Box" });
            items.Add(
               new Items() { ItemName = "Blue pen", ItemID = "Z124", CartId = "A124", UnitOfMeasure = "Dozen" });
            return items;
        }

        //public List<Category> getcategory()
        //{
        //    List<Category> category = new List<Category>();
        //    category.Add(new Category() { CategoryName = "pen", CategoryId = "B123" });
        //    category.Add(new Category() { CategoryName = "pen", CategoryId = "B123" });
        //    category.Add(new Category() { });
        //    return category;
        //}
        public List<Stocklevel> getstocklevel()
        {
            List<Stocklevel> stockleve = new List<Stocklevel>();
            stockleve.Add(new Stocklevel() { Reorderlevel = 30, Currentstock = 38, ReorderQuantity = 50 });
            stockleve.Add(new Stocklevel() { Reorderlevel = 37, Currentstock = 35, ReorderQuantity = 50 });
            return stockleve;
        }




        //    public List<OrderItem> getorderitems()
        //    {
        //        List<OrderItem> orderitem = new List<OrderItem>();
        //        {
        //            new OrderItem
        //            {
        //                category = "Puncher",
        //                description = "2 Holes",
        //                reorderlevel = 20,
        //                currentstock = 17,
        //                recorderquantity = 40
        //            };
        //            new OrderItem
        //            {
        //                category = "Ruler",
        //                description = "Ruler 12'",
        //                reorderlevel = 30,
        //                currentstock = 39,
        //                recorderquantity = 50
        //            };
        //            new OrderItem
        //            {
        //                category = "pen",
        //                description = "2B pencil",
        //                reorderlevel = 50,
        //                currentstock = 20,
        //                recorderquantity = 50
        //            };
        //            new OrderItem
        //            {
        //                category = "pen",
        //                description = "Blue pen",
        //                reorderlevel = 50,
        //                currentstock = 40,
        //                recorderquantity = 50
        //            };
        //            new OrderItem
        //            {
        //                category = "Ruler",
        //                description = "Ruler 6'",
        //                reorderlevel = 30,
        //                currentstock = 38,
        //                recorderquantity = 50
        //            };
        //            return orderitem;
        //        }
        //    }

    }
}
