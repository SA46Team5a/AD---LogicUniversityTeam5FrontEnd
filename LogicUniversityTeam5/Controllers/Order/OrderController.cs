using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class OrderController : Controller
    {
        // GET: ItemCatalogue
        public ActionResult ItemCatalogue()
        {
            ItemCatalogue itemcatalogue = new ItemCatalogue();
            itemcatalogue.item = getitem();
            itemcatalogue.category = getcategory();
            itemcatalogue.stocklevel = getstocklevel();
           
            return View(itemcatalogue);
        }

        [HttpPost]
        public ActionResult ItemCatalogue(ItemCatalogue item)
        {
            ItemCatalogue value = new ItemCatalogue();
            value = item;
            return RedirectToAction("OrderQuantity", "OrderQuantity", new { type1 = item });
        }
        public List<Items> getitem()
        {

            List<Items> items = new List<Items>();
            items.Add(
                new Items() { ItemName = "2B Pencil", ItemID="Z123", CartId="A123", UnitOfMeasure="Box" });
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
            stockleve.Add( new Stocklevel() { Reorderlevel=30, Currentstock=38, ReorderQuantity=50});
            stockleve.Add(new Stocklevel() { Reorderlevel = 37, Currentstock = 35, ReorderQuantity = 50 });
            return stockleve;
        }
    }
}