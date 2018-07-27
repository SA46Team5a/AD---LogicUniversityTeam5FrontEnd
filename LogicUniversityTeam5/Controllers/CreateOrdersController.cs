using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            //Harcoded: to be removed
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.OrderSuppliers = context.OrderSuppliers.Where(x => x.OrderSupplierID == 1).ToList();
            combinedViewModel.OrderSupplierDetails = context.OrderSupplierDetails.Where(x => x.OrderSupplierID == 1).ToList();

            return new ViewAsPdf("PrintOrderSummary", combinedViewModel);

        }

        private CombinedViewModel RetrieveDataForOrderSummary()
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

            return combinedViewModel;
        }

        public ActionResult OrderSummary(CombinedViewModel model)
        {
            TempData["itemIdsAndItemQty"] = model.ItemIdAndQty;
            int id = model.AddedNumbers[0];
            return OrderSummary(id);
        }

        public List<Stocklevel> getstocklevel()
        {
            List<Stocklevel> stockleve = new List<Stocklevel>();
            stockleve.Add(new Stocklevel() { Reorderlevel = 30, Currentstock = 38, ReorderQuantity = 50 });
            stockleve.Add(new Stocklevel() { Reorderlevel = 37, Currentstock = 35, ReorderQuantity = 50 });
            return stockleve;
        }

        //To be implemented later
        [HttpPost]
        public FileResult DownloadPurchaseOrders(List<string> files)
        {
            var archive = Server.MapPath("~/archive.zip");
            var temp = Server.MapPath("~/temp");

            // clear any existing archive
            if (System.IO.File.Exists(archive))
            {
                System.IO.File.Delete(archive);
            }
            // empty the temp folder
            Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

            // copy the selected files to the temp folder

            files.ForEach(f => System.IO.File.Copy(f, Path.Combine(temp, Path.GetFileName(f))));

            // create a new archive
            ZipFile.CreateFromDirectory(temp, archive);

            return File(archive, "application/zip", "archive.zip");
        }



    }
}
