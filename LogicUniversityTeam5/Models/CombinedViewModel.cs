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
        public CombinedViewModel()
        {
            //IsSelected = new List<string>();
            Items = new List<Item>();
            reorderdetail= new List<ReorderDetail>();
        }
        public List<UploadModel> File { get; set; }
        public List<EmailFormModel> EmailForm { get; set; }
        public List<Item> Items { get; set; }
        public List<Employee> Employee { get; set; }
        public List<StockVoucher> StockVouchers { get; set; }
        public List<int> Quantity { get; set; }
        public List<Order> Orders { get; set; }
        public string trialText { get; set; }

        public List<OrderSupplier> OrderSuppliers { get; set; }
        public List<RequisitionDetail> Details { get; set; }
        public List<Supplier> Supplier { get; set; }
        public List<String> AddedText { get; set; }
        public List<int> AddedNumbers { get; set; }
        public List<ReorderDetail> reorderdetail {get;set;}
        public List<bool> IsSelected { get; set; }

        public void setAddedTextSize(int size)
        {
            AddedText = new List<String>(size);
        }

        public void setAddedNumbersSize(int size)
        {
            AddedNumbers = new List<int>(size);
        }
        public List<CollectionPoint> CollectionPoint { get; set; }


        public List<DepartmentRepresentative> DepartmentRepresentative { get; set; }

        public List<Department> Department { get; set; }

        public string DepartmentID { get; set; }
        public List<Authority> Authority { get; set; }

        public Authority Authorities { get; set; }

        //public List<CollectionPoint> RadioButtonList { get; set; }
        // public string SelectedRadioButton { get; set; }
    }
}