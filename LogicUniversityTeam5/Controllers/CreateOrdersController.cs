﻿using System;
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
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Resources;

namespace LogicUniversityTeam5.Controllers.Order
{
    // Author: Meiting && Khim Yang
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
        
        [Authorize(Roles = "Store Supervisor,Store Clerk")]
        public ActionResult ItemCatalogue()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.reorderdetail = orderService.getReorderDetails();

            model.Items = new List<Item>();
            for (int i = 0; i < model.reorderdetail.Count ; i++)
            {
                string itemId = model.reorderdetail[i].ItemID;
                model.Items.Add(stockManagementService.getItemById(itemId));
            }
            return View(model);
        }
       
        [HttpPost]
        public ActionResult ItemCatalogue(string Next, CombinedViewModel model,string Search)      
        {
            
            if (Search != null)
            {
                for (int i = 0; i < model.reorderdetail.Count; i++)
                {
                    CombinedViewModel searchmodel = new CombinedViewModel();
                    string selectCategory = model.AddedText[0];
                    searchmodel.reorderdetail = new List<ReorderDetail>();
                    
                    //refactor to ServiceLayer getReorderDetailsByCategoryName()
                    Category category = context.Categories.First(m => m.CategoryName == selectCategory);
                    searchmodel.Items = stockManagementService.getItemsOfCategory(category.CategoryID);
                    
                    List<string> itemIds = searchmodel.Items.Select(it => it.ItemID).ToList();

                    foreach(string id in itemIds)
                    {
                        ReorderDetail detail = context.ReorderDetails.First(x => x.ItemID == id);
                        searchmodel.reorderdetail.Add(detail);
                    }
                    //

                    return View(searchmodel);
                }
            }
            
            if (Next != null && model.AddedText[0]==null)
            {
                CombinedViewModel passModel = new CombinedViewModel();
                passModel.Quantity = new List<int>();
                passModel.Items = new List<Item>();
                passModel.reorderdetail = new List<ReorderDetail>();
                for (int i = 0; i < model.reorderdetail.Count; i++)
                {
                    int value = model.Quantity[i];
                    string selectcategory = model.AddedText[0];
                       
                        if (value.ToString() != null && value != 0)
                        {

                            passModel.Items.Add(model.Items[i]);
                            passModel.reorderdetail.Add(model.reorderdetail[i]);
                            passModel.Quantity.Add(model.Quantity[i]);
                            TempData["passmodel"] = passModel;
                        }
                      
                }
                //TempData["passmodel"] = passModel;
                return RedirectToAction("OrderQuantity");
            }
            else
            {
                return View(model);
            }


        }
        [Authorize(Roles = "Store Clerk")]
        public ActionResult OrderQuantity()
        {
            CombinedViewModel model = new CombinedViewModel();
            model=TempData["passmodel"] as CombinedViewModel;

            TempData["passmodel"] = model;
            TempData.Keep("passmodel");
            return View(model);
        }
        [Authorize(Roles = "Store Clerk")]
        public ActionResult PlaceOrder()
        {

            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.Suppliers = new List<Supplier>();
            combinedViewModel.AddedNumbers = new List<int>();
            combinedViewModel.Items = new List<Item>();
            combinedViewModel.ReOrderItemQty = new List<int>();

            CombinedViewModel passedModel = (CombinedViewModel) TempData["passmodel"];

            List<int> itemQty = passedModel.Quantity;
            List<string> itemIds = passedModel.Items.Select(x => x.ItemID).ToList();
            Dictionary<string, int> itemIdsAndItemQty = new Dictionary<string, int>();

            for(int i= 0; i<itemIds.Count;i++)
            {
                itemIdsAndItemQty.Add(itemIds[i], itemQty[i]);
            }
            TempData["itemIdsAndItemQty"] = itemIdsAndItemQty;
            TempData.Keep("itemIdsAndItemQty");

            //getting supplierItems
            combinedViewModel.supplierItems = orderService.getSupplierItemsOfItemIds(itemIds);            

            foreach (SupplierItem supplierItem in combinedViewModel.supplierItems)
            {
                //Adding Suppliers in combinedview
                if (!combinedViewModel.Suppliers.Contains(supplierItem.Supplier))
                {
                    //to refactor to service method getSupplierBySupplierID
                    combinedViewModel.Suppliers.Add(context.Suppliers.Where(x => x.SupplierID == supplierItem.SupplierID).First());
                }

                //Adding Items in combinedview
                combinedViewModel.Items.Add(context.Items.Where(x => x.ItemID.Equals(supplierItem.ItemID)).First());
                foreach(KeyValuePair<string,int> itemIdAndQty in itemIdsAndItemQty)
                {
                    if(itemIdAndQty.Key.Equals(supplierItem.ItemID))
                        combinedViewModel.ReOrderItemQty.Add(itemIdAndQty.Value);
                }

                //initial value for txtboxes
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
            Dictionary<string, int> itemAndQty = (Dictionary<string, int>) TempData["itemIdsAndItemQty"];

            TempData["itemIdsAndItemQty"] = itemAndQty;
            TempData.Keep("itemIdsAndItemQty");

            //creating new OrderId
            int newOrderId = orderService.createOrderAndGetOrderId(itemAndQty, supplierItemsAndQty);

            return RedirectToAction("OrderSummary",new { id = newOrderId });
        }

        [Authorize(Roles = "Store Clerk")]
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
        [Authorize(Roles = "Store Clerk")]
        public ActionResult PrintOrderSummary(CombinedViewModel model)
        {
            //Harcoded: to be removed
            CombinedViewModel combinedViewModel = new CombinedViewModel();
            combinedViewModel.OrderSuppliers = context.OrderSuppliers.Where(x => x.OrderSupplierID == 1).ToList();
            combinedViewModel.OrderSupplierDetails = context.OrderSupplierDetails.Where(x => x.OrderSupplierID == 1).ToList();

            return new ViewAsPdf("PrintOrderSummary", combinedViewModel);

        }
        [Authorize(Roles = "Store Clerk")]
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
        [Authorize(Roles = "Store Clerk")]
        public ActionResult OrderSummary(CombinedViewModel model)
        {
            TempData["itemIdsAndItemQty"] = model.ItemIdAndQty;
            int id = model.AddedNumbers[0];
            return OrderSummary(id);
        }

        [HttpPost]
        public ActionResult OrderQuantity(CombinedViewModel model)
        {
            CombinedViewModel passModel = new CombinedViewModel();
            passModel.Quantity = new List<int>();
            passModel.Items = new List<Item>();
            passModel.reorderdetail = new List<ReorderDetail>();
            for (int i = 0; i < model.reorderdetail.Count; i++)
            {
                int value = model.Quantity[i];
                passModel.Items.Add(stockManagementService.getItemById(model.Items[i].ItemID));
                passModel.reorderdetail.Add(model.reorderdetail[i]);
                passModel.Quantity.Add(model.Quantity[i]);
              
            }
            TempData["passmodel"] = passModel;
            return RedirectToAction("PlaceOrder");
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
        [Authorize(Roles = "Store Clerk")]
        public ActionResult SubmitInvoice()
        {

            CombinedViewModel model = new CombinedViewModel();
            model.Suppliers = new List<Supplier>();
            model.OrderSuppliers = context.OrderSuppliers.Where(x => x.InvoiceUploadStatus.InvoiceUploadStatusID == 2).ToList();
            model.Suppliers = orderService.getSuppliers();
            model.AddedText = new List<string>(2) { "", "" };
            model.RadioButtonListData = new List<RadioButtonData>(); 
            for(int i = 0; i < model.Suppliers.Count; i++)
            {
                int ID = i + 1;
                model.RadioButtonListData.Add(new RadioButtonData { Id = ID });

            }
           
            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitInvoice(CombinedViewModel model, HttpPostedFileBase file, string Sent, string Search, string radiobutton)
        {


            if (Search != null)
            {
                CombinedViewModel searchmodel = new CombinedViewModel();
                int selectorderid = Int32.Parse(model.AddedText[0]);
                searchmodel.OrderSuppliers = new List<OrderSupplier>();
                searchmodel.OrderSuppliers = orderService.getOrderSuppliersOfOrder(selectorderid);       
                searchmodel.Suppliers = new List<Supplier>();
                searchmodel.Suppliers = orderService.getSuppliersOfOrderIdWithOutstandingInvoice(selectorderid);
                int size = searchmodel.Suppliers.Count;
                searchmodel.RadioButtonListData = new List<RadioButtonData>(size);
                for(int i = 0; i < size; i++)
                {
                    int radioid = i + 1;
                    searchmodel.RadioButtonListData.Add(new RadioButtonData { Id = radioid });
                }
                return View(searchmodel);
            }
            if (Sent != null)
            {
                int selectedOrderId = Convert.ToInt32(model.AddedText[0]);
                CombinedViewModel model1 = new CombinedViewModel();
                model1.OrderSuppliers =
                        orderService.getOrderSuppliersOfOrder(selectedOrderId);
                List<string> supplierid = new List<string>();
                for (int i = 0; i < model1.OrderSuppliers.Count; i++)
                {
       
                    supplierid.Add(model1.OrderSuppliers[i].SupplierID);
                    
                }
                model1.Suppliers = new List<Supplier>();
                List<Supplier> sup = new List<Supplier>();
                for (int i = 0; i < supplierid.Count; i++)
                {
                    string value = supplierid[i];
                    Supplier supplier = context.Suppliers.First(m => m.SupplierID == value);
                    sup.Add(supplier);
                }
                for(int i = 0; i < sup.Count(); i++)
                {
                    string name = sup[i].SupplierName;
                    var output = sup[i].SupplierID;
                    List<string> supplierlist = new List<string>();
                    if (radiobutton == name)
                    {
                        OrderSupplier selectedOrderSupplier = model1.OrderSuppliers
                       .Where(x => x.SupplierID==output).First();
                        orderService.confirmInvoiceUploadStatus(selectedOrderSupplier.OrderSupplierID);
                        model1.Suppliers = orderService.getSuppliersOfOrderIdWithOutstandingInvoice(selectedOrderId);
                        int size = model1.Suppliers.Count;
                        model1.RadioButtonListData = new List<RadioButtonData>(size);
                        for (int m = 0; m < size; m++)
                        {
                            int radioid = m + 1;
                            model1.RadioButtonListData.Add(new RadioButtonData { Id = radioid });
                        }
                    }
                }
               
               
                string path = System.IO.Path.Combine(Server.MapPath("~/UploadedInvoices"),
                    System.IO.Path.GetFileName(file.FileName));

                file.SaveAs(path);
                model1.EmailForm = new List<EmailFormModel>();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new System.Net.NetworkCredential("LogicstationeryTeam5@gmail.com", "logicteam5@");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mm = new MailMessage("LogicstationeryTeam5@gmail.com", " LogicfinanceTeam5@gmail.com");
                mm.Subject = "  Submit Invoice";
                mm.Body = "Dear financial department: attached is the invoice for supplier, please go and check";
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(file.InputStream, file.FileName);

                mm.Attachments.Add(attachment);
                client.Send(mm);
                //EmailNotificationController emailNotificationController = new EmailNotificationController();
                //emailNotificationController.Index(file);
                return View(model1);
            }
            else
            {
                return View(model);
            }

            
        }
       
        public CombinedViewModel getmodel()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.Suppliers = new List<Supplier>();
            model.OrderSuppliers = new List<OrderSupplier>();
            model.OrderSuppliers = context.OrderSuppliers.Where(x => x.InvoiceUploadStatus.InvoiceUploadStatusID == 2).ToList();
            model.Suppliers = orderService.getSuppliers();
            model.AddedText = new List<string>(2) { "", "" };
            return model;
        }
        
    }

    
}

