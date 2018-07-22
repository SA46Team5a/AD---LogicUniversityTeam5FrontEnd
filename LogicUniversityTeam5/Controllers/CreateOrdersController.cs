using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using ServiceLayer;


namespace LogicUniversityTeam5.Controllers.Order
{
    public class CreateOrdersController : Controller
    {
        IStockManagementService _stockManagementService;
        public CreateOrdersController(StockManagementService sms)
        {
            _stockManagementService = sms;
        }

        public ActionResult ItemCatalogue()
        {
            ItemCatalogueModel itemcatalogue = new ItemCatalogueModel();
            itemcatalogue.items = _stockManagementService.getAllItems();
            itemcatalogue.categories = getcategory();
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

        public ActionResult PlaceOrder() {

            return View();
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

        public List<Category> getcategory()
        {
            List<Category> category = new List<Category>();
            category.Add(new Category() { CategoryName = "pen", CategoryId = "B123" });
            category.Add(new Category() { CategoryName = "pen", CategoryId = "B123" });
            category.Add(new Category() { });
            return category;
        }
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
