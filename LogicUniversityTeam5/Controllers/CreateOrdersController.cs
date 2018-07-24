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
              
            //itemcatalogue.items = stockManagementService.getAllItems();

            //model.Items = stockManagementService.getAllItems();
            CombinedViewModel model = new CombinedViewModel();
            model.reorderdetail = context.ReorderDetails.ToList();

            model.IsSelected = new List<string>();

            model.Items = new List<Item>();
            for (int i = 0; i < model.reorderdetail.Count ; i++)
            {
                string itemId = model.reorderdetail[i].ItemID;
                model.Items.Add(context.Items.First(m => m.ItemID == itemId));
            }
            //TempData["passmodel"] = model;
           
            return View(model);

        }
       
        [HttpPost]
        public ActionResult ItemCatalogue(CombinedViewModel model)//int i=0       
        {
            CombinedViewModel passModel = new CombinedViewModel();
            passModel.Quantity = new List<int>();
            passModel.IsSelected = new List<string>();
            passModel.Items = new List<Item>();
            passModel.reorderdetail = new List<ReorderDetail>();
            for (int i = 0; i < model.reorderdetail.Count; i++)
            {
                int value = model.Quantity[i];
                if (value.ToString()!=null&& value!=0)
                {

                    passModel.Items.Add(model.Items[i]);
                    passModel.reorderdetail.Add(model.reorderdetail[i]);
                    passModel.Quantity.Add(model.Quantity[i]);

                }
            }
            TempData["passmodel"] = passModel;
           

            return RedirectToAction("OrderQuantity");
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
        public ActionResult SubmitInvoice(CombinedViewModel model, HttpPostedFileBase file)
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
            //client.UseDefaultCredentials = true;
            MailMessage mm = new MailMessage("meitingtonia@gmail.com", "meitingtonia@gmail.com");
            mm.Subject = "test subject";
            mm.Body = "test body";
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(file.InputStream, file.FileName); //ERROR

            mm.Attachments.Add(attachment);
            client.Send(mm);          
            return View(model1);
            
        }
        public ActionResult Sent()
        {
            return View();
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

