using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Models
{
    public class CombinedViewModel
    {
        public List<Item> Items { get; set; }

        public List<StockVoucher> StockVouchers { get; set; }

        public List<SupplierItem> supplierItems { get; set; }

        public List<Requisition> Requisitions { get; set; }

        public List<Department> Departments { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public List<Order> Orders { get; set; }

        public List<OrderSupplier> OrderSuppliers { get; set; }

        public List<OrderSupplierDetail> OrderSupplierDetails { get; set; }

        public List<int> ReOrderItemQty { get; set; }

        public Dictionary<int,int> SupplierItemIdAndQtyAvail { get; set; }

        public Dictionary<string, int> ItemIdAndQty { get; set; }

        public List<OutstandingRequisitionView> OutstandingRequisitionViews { get; set; }

        public List<bool> IsSelected { get; set; }

        public List<String> AddedText { get; set; }

        public List<int> AddedNumbers { get; set; }


    }
}