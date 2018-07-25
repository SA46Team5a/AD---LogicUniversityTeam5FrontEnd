using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using Rotativa;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Controllers.Order
{
    public class CreateOrdersController : Controller
    {
        IStockManagementService stockManagementService;
        IOrderService orderService;
        StationeryStoreEntities context;

        public CreateOrdersController(StockManagementService sms, OrderService os)
        {
            stockManagementService = sms;
            orderService = os;
            context = StationeryStoreEntities.Instance;
        }

        public ActionResult ItemCatalogue()
        {
            ItemCatalogueModel itemcatalogue = new ItemCatalogueModel();
            itemcatalogue.items = stockManagementService.getAllItems();
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
            combinedViewModel.Suppliers = new List<Supplier>();
            combinedViewModel.AddedNumbers = new List<int>();
            combinedViewModel.Items = new List<Item>();
            combinedViewModel.ReOrderItemQty = new List<int>();
            //combinedViewModel.ReOrderItemQty = (List<int>)TempData["itemQty"];
            //List<string> itemIds = (List<string>) TempData["itemIds"];
            //List<int> itemQty = (List<int>) TempData["itemQty"];

            
            //Hardcoded: To be removed
            List<int> itemQty = new List<int>() { 50, 60, 80 };

            //Hardcoded: To be removed
            TempData["itemIds"] = new List<string>()
            {
                "C001","C002","C003"
            };
            TempData["itemQty"] = new List<int>() { 50, 60, 80 };

            //}

            TempData.Keep("itemQty");
            TempData.Keep("itemIds");

            //Hardcoded: To be removed
            List<string> itemIds = new List<string>()
            {
                "C001","C002","C003"
            };

            Dictionary<string, int> itemIdsAndItemQty = new Dictionary<string, int>();

            for(int i= 0; i<itemIds.Count;i++)
            {
                itemIdsAndItemQty.Add(itemIds[i], itemQty[i]);
            }
            TempData["itemIdsAndItemQty"] = itemIdsAndItemQty;
            TempData.Keep("itemIdsAndItemQty");

            combinedViewModel.supplierItems = orderService.getSupplierItemsOfItemIds(itemIds);
            

            foreach (SupplierItem supplierItem in combinedViewModel.supplierItems)
            {
                //Adding Suppliers in combinedview
                if (!combinedViewModel.Suppliers.Contains(supplierItem.Supplier))
                {
                    combinedViewModel.Suppliers.Add(context.Suppliers.Where(x => x.SupplierID == supplierItem.SupplierID).First());
                }

                //Adding Items in combinedview
                combinedViewModel.Items.Add(context.Items.Where(x => x.ItemID.Equals(supplierItem.ItemID)).First());
                foreach(KeyValuePair<string,int> itemIdAndQty in itemIdsAndItemQty)
                {
                    if(itemIdAndQty.Key.Equals(supplierItem.ItemID))
                        combinedViewModel.ReOrderItemQty.Add(itemIdAndQty.Value);
                }
                combinedViewModel.AddedNumbers.Add(0);
            }
            
            return View(combinedViewModel);
        }

        [HttpPost]
        public ActionResult PlaceOrder(CombinedViewModel model)
        {
            //creating Dictionary<int, int> supplierItemsAndQty
            Dictionary<int, int> supplierItemsAndQty = new Dictionary<int, int>();
            for(int i =0; i < model.AddedNumbers.Count; i++)
            {
                supplierItemsAndQty.Add(model.supplierItems[i].SupplierItemID, model.AddedNumbers[i]);
            }

            //creating Dictionary<string, int> itemAndQty
            Dictionary<string, int> itemAndQty = new Dictionary<string, int>();
            List<string> itemIds = (List<string>) TempData["itemIds"];
            List<int> itemQty = (List<int>) TempData["itemQty"];
            for (int i = 0; i < itemIds.Count; i++)
            {
                itemAndQty.Add(itemIds[i],itemQty[i]);
            }

            TempData.Keep("itemIdsAndItemQty");
            //creating new OrderId
            //int newOrderId = orderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);
            //Hardcoded: to be removed
            int newOrderId = 1;

            return RedirectToAction("OrderSummary",new { id = newOrderId });
        }


        [Route("CreateOrders/OrderSummary/{id}")]
        public ActionResult OrderSummary(int id)
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();

            combinedViewModel.OrderSupplierDetails =
                context.OrderSupplierDetails.Where(x => x.OrderSupplier.OrderID == id).ToList();

            //Adding Items and Required Quantity
            Dictionary<string, int> itemIdsAndItemQty = (Dictionary<string, int>)TempData["itemIdsAndItemQty"];
            if(itemIdsAndItemQty==null)
                itemIdsAndItemQty = (Dictionary<string, int>)Session["itemIdsAndItemQty"];
            combinedViewModel.Items = new List<Item>();
            combinedViewModel.ReOrderItemQty = new List<int>();
            foreach(OrderSupplierDetail osd in combinedViewModel.OrderSupplierDetails)
            {
                combinedViewModel.Items.Add(
                    context.Items.First(x => x.ItemID.Equals(osd.ItemID)));
                if (itemIdsAndItemQty.Keys.Contains(osd.ItemID)) {
                    combinedViewModel.ReOrderItemQty.Add(
                        itemIdsAndItemQty.First(x => x.Key.Contains(osd.ItemID)).Value);
                }
                else
                {
                    combinedViewModel.ReOrderItemQty.Add(0);
                }
            }
            TempData["orderId"] = id;
            TempData.Keep("itemIdsAndItemQty");

            return View(combinedViewModel);
        }

 
        public ActionResult PrintOrderSummary(CombinedViewModel model)
        {
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            int id = (int)TempData["orderId"];
            combinedViewModel.OrderSupplierDetails =
                context.OrderSupplierDetails.Where(x => x.OrderSupplier.OrderID == id).ToList();

            //Adding Items and Required Quantity
            Dictionary<string, int> itemIdsAndItemQty = (Dictionary<string, int>)TempData["itemIdsAndItemQty"];
            if (itemIdsAndItemQty == null)
                itemIdsAndItemQty = (Dictionary<string, int>)Session["itemIdsAndItemQty"];
            combinedViewModel.Items = new List<Item>();
            combinedViewModel.ReOrderItemQty = new List<int>();
            foreach (OrderSupplierDetail osd in combinedViewModel.OrderSupplierDetails)
            {
                combinedViewModel.Items.Add(
                    context.Items.First(x => x.ItemID.Equals(osd.ItemID)));
                if (itemIdsAndItemQty.Keys.Contains(osd.ItemID))
                {
                    combinedViewModel.ReOrderItemQty.Add(
                        itemIdsAndItemQty.First(x => x.Key.Contains(osd.ItemID)).Value);
                }
                else
                {
                    combinedViewModel.ReOrderItemQty.Add(0);
                }
            }
            return new ViewAsPdf("PrintOrderSummary", combinedViewModel);

        }

        public ActionResult OrderSummary(CombinedViewModel model)
        {
            TempData["itemIdsAndItemQty"] = model.ItemIdAndQty;
            int id = model.AddedNumbers[0];
            return OrderSummary(id);
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
