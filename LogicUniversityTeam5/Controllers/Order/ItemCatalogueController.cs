using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class ItemCatalogueController : Controller
    {
        // GET: ItemCatalogue
        public ActionResult ItemCatalogue()
        {
            ordertest combine = new ordertest();
            combine.order = getorderitems();

            
           
            return View(combine);
        }
         public List<OrderItem> getorderitems()
        {
            List<OrderItem> orderitem = new List<OrderItem>();
            {
                new OrderItem
                {
                    category = "Puncher",
                    description = "2 Holes",
                    reorderlevel = 20,
                    currentstock = 17,
                    recorderquantity = 40
                };
                new OrderItem
                {
                    category = "Ruler",
                    description = "Ruler 12'",
                    reorderlevel = 30,
                    currentstock = 39,
                    recorderquantity = 50
                };
                new OrderItem
                {
                    category = "pen",
                    description = "2B pencil",
                    reorderlevel = 50,
                    currentstock = 20,
                    recorderquantity = 50
                };
                new OrderItem
                {
                    category = "pen",
                    description = "Blue pen",
                    reorderlevel = 50,
                    currentstock = 40,
                    recorderquantity = 50
                };
                new OrderItem
                {
                    category = "Ruler",
                    description = "Ruler 6'",
                    reorderlevel = 30,
                    currentstock = 38,
                    recorderquantity = 50
                };
                return orderitem;
            }
        }
   
}