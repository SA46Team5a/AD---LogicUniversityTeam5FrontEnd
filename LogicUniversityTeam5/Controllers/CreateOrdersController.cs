using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Models;
using ServiceLayer;
using ServiceLayer.DataAccess;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Resources;

namespace LogicUniversityTeam5.Controllers.Order
{
    // Author: Meiting
    public class CreateOrdersController : Controller
    {

        IStockManagementService stockManagementService;
        IOrderService orderService;
        StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public CreateOrdersController(StockManagementService service, OrderService orderservice)
        {
            this.stockManagementService = service;
            this.orderService = orderservice;
        }

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
                CombinedViewModel passModel = new CombinedViewModel();
                passModel.Quantity = new List<int>();
                passModel.Items = new List<Item>();
                passModel.reorderdetail = new List<ReorderDetail>();
                for (int i = 0; i < model.reorderdetail.Count; i++)
                {
                    CombinedViewModel searchmodel = new CombinedViewModel();
                    string selectcategory = model.AddedText[0];
                    searchmodel.Items = new List<Item>();
                    searchmodel.reorderdetail = new List<ReorderDetail>();
                    searchmodel.Items = context.Items.Where(m => m.Category.CategoryName == selectcategory).ToList();
                    List<string> itemid = new List<string>();
                    for (int m = 0; m < searchmodel.Items.Count; m++)
                    {
                        itemid.Add(searchmodel.Items[m].ItemID);
                    }
                    for (int m = 0; m < itemid.Count; m++)
                    {
                        var value = itemid[m];
                        ReorderDetail detail = context.ReorderDetails.First(x => x.ItemID == value);
                        searchmodel.reorderdetail.Add(detail);
                    }
                    return View(searchmodel);
                }
            }
           
            if (Next != null)
            {
                CombinedViewModel passModel = new CombinedViewModel();
                passModel.Quantity = new List<int>();
                passModel.Items = new List<Item>();
                passModel.reorderdetail = new List<ReorderDetail>();
                for (int i = 0; i < model.reorderdetail.Count; i++)
                {
                    int value = model.Quantity[i];
                    if (value.ToString() != null && value != 0)
                    {

                        passModel.Items.Add(model.Items[i]);
                        passModel.reorderdetail.Add(model.reorderdetail[i]);
                        passModel.Quantity.Add(model.Quantity[i]);

                    }
                }
                TempData["passmodel"] = passModel;

                return RedirectToAction("OrderQuantity");
            }
            else
            {
                return View(model);
            }
            
          
        }
       
        public ActionResult OrderQuantity()
        {
            CombinedViewModel model = new CombinedViewModel();
            model=TempData["passmodel"] as CombinedViewModel;          
            return View(model);
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
                passModel.Items.Add(model.Items[i]);
                passModel.reorderdetail.Add(model.reorderdetail[i]);
                passModel.Quantity.Add(model.Quantity[i]);
              
            }
            TempData["passmodel"] = passModel;
            return RedirectToAction("PlaceOrder");
        }
        public ActionResult PlaceOrder_test()
        {
            return View();
        }
        public ActionResult SubmitInvoice()
        {

            // CombinedViewModel model = new CombinedViewModel();
            CombinedViewModel model = getmodel();

            return View(model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitInvoice(CombinedViewModel model, HttpPostedFileBase file, string Sent, string Search)
        {
            if (Search != null)
            {
                CombinedViewModel searchmodel = new CombinedViewModel();
                int selectorderid = Int32.Parse(model.AddedText[0]);
                OrderSupplier orderSupplier = context.OrderSuppliers.First(m => m.OrderID == selectorderid);
                searchmodel.Supplier = new List<Supplier>();
                searchmodel.Supplier = context.Suppliers.Where(x => x.SupplierID == orderSupplier.SupplierID).ToList();
                searchmodel.OrderSuppliers = context.OrderSuppliers.Where(x => x.InvoiceUploadStatus.InvoiceUploadStatusID == 2).ToList();
                return View(searchmodel);
            }
            if (Sent != null)
            {
                int selectedOrderId = Convert.ToInt32(model.AddedText[0]);
                string selectedSupplierName = model.AddedText[1];

                List<OrderSupplier> orderSuppliers =
                        orderService.getOrderSuppliersOfOrder(selectedOrderId);
                OrderSupplier selectedOrderSupplier = orderSuppliers
                    .Where(x => x.Supplier.SupplierName.Equals(selectedSupplierName)).First();

                orderService.confirmInvoiceUploadStatus(selectedOrderSupplier.OrderSupplierID);

                CombinedViewModel model1 = getmodel();

                string path = System.IO.Path.Combine(Server.MapPath("~/UploadedInvoices"),
                    System.IO.Path.GetFileName(file.FileName));

                file.SaveAs(path);
                model1.EmailForm = new List<EmailFormModel>();
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new System.Net.NetworkCredential("meitingtonia@gmail.com", "GMTtonia1995");
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
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
            model.OrderSuppliers = context.OrderSuppliers.Where(x => x.InvoiceUploadStatus.InvoiceUploadStatusID == 2).ToList();
            model.Supplier = context.Suppliers.ToList();
            model.AddedText = new List<string>(2) { "", "" };
            return model;
        }
        
    }

    
}

