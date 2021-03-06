﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceLayer.DataAccess;
using LogicUniversityTeam5.Models;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class ItemCatalogueController : Controller
    {
        // GET: ItemCatalogue
        public ActionResult ItemCatalogue()
        {
            ItemCatalogueModel itemcatalogue = new ItemCatalogueModel();
            //itemcatalogue.items = getitem();
            //itemcatalogue.categories = getcategory();
            //itemcatalogue.stocklevels = getstocklevel();
           
            return View(itemcatalogue);
        }

        [HttpPost]
        public ActionResult ItemCatalogue(ItemCatalogueModel item)
        {
            ItemCatalogueModel value = new ItemCatalogueModel();
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
            category.Add(new Category() { CategoryName = "pen", CategoryID = 123 });
            category.Add(new Category() { CategoryName = "pen", CategoryID = 123 });
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
