﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityTeam5.Controllers;
using LogicUniversityTeam5.Models;

using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    public class ViewReceivedOrdersListController : Controller
    {
        IOrderService orderService;
        StationeryStoreEntities context = StationeryStoreEntities.Instance;

        public ViewReceivedOrdersListController()
        {
            this.orderService = orderService;
            
        }
        public ActionResult ReceivedPurchaseOrdersList()
        {
            CombinedViewModel model = new CombinedViewModel();
            model.Orders=context.Orders.ToList();
            
            return View(model);
        }
        public ActionResult PurchaseOrderSummary(int id)
        {
            CombinedViewModel model = new CombinedViewModel();
            List<int> ordersupplierid = new List<int>();
            List<int> supplierdetailsid = new List<int>();
            List<string> itemid = new List<string>();
            model.OrderSupplierDetails = new List<OrderSupplierDetail>();
            model.Items = new List<Item>();
            model.SupplierItem = new List<SupplierItem>();
            List<string> supplierid = new List<string>();
            model.Supplier = new List<Supplier>();          
            model.OrderSuppliers = context.OrderSuppliers.Where(x => x.OrderID == id).ToList();
            for(int i = 0; i < model.OrderSuppliers.Count; i++)
            {
                ordersupplierid.Add(model.OrderSuppliers[i].OrderSupplierID);
               
            }
      
            
            
            for(int i = 0; i < model.OrderSuppliers.Count; i++)
            {
                CombinedViewModel tempmodel = new CombinedViewModel();
                var osSupplierid = ordersupplierid[i];
                tempmodel.OrderSupplierDetails = context.OrderSupplierDetails.
                    Where(x => x.OrderSupplierID == osSupplierid).ToList();
                for (int m = 0; m < tempmodel.OrderSupplierDetails.Count; m++)
                {
                   
                    supplierdetailsid.Add(tempmodel.OrderSupplierDetails[m].OrderSupplierDetailsID);
                    itemid.Add(tempmodel.OrderSupplierDetails[m].ItemID);
                    
                }
                
            }
            
            for (int i = 0; i < itemid.Count; i++)
            {
                var itemid1=itemid[i];
                Item item = context.Items.First(m => m.ItemID == itemid1);
                SupplierItem supplierItem = context.SupplierItems.First(m => m.ItemID == itemid1);
                model.SupplierItem.Add(supplierItem);
                model.Items.Add(item);
            }
            
            for(int i = 0; i < itemid.Count; i++)
            {                
                supplierid.Add(model.SupplierItem[i].SupplierID);
            }
            
            for (int i = 0; i <supplierid.Count; i++)
            {
                var supplierid1 = supplierid[i];
                Supplier supplier = context.Suppliers.First(m => m.SupplierID == supplierid1);
                model.Supplier.Add(supplier);
            }
            for(int i = 0; i < supplierdetailsid.Count; i++)
            {
                int passid = supplierdetailsid[i];
                OrderSupplierDetail detail = context.OrderSupplierDetails.First(m=>m.OrderSupplierDetailsID==passid);
                model.OrderSupplierDetails.Add(detail);
            }

           

            return View(model);
        }
    }
}