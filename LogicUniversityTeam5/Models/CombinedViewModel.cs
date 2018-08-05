using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ServiceLayer;
using ServiceLayer.DataAccess;


namespace LogicUniversityTeam5.Models
{
    public class CombinedViewModel
    {
        public List<Item> Items { get; set; }

        public List<Category> Categories { get; set; }

        public List<int> QtyReceived { get; set; }

        public List<RequisitionDetail> Requisitions { get; set; }

        public List<StockCountItem> StockCountItems { get; set; }

        public List<StockVoucher> StockVouchers { get; set; }


        public List<Order> Orders { get; set; }

        public List<bool> IsSelected { get; set; }

        public bool IsSave { get; set; }

        public string categorySelected { get; set; }
        [Required(ErrorMessage = "OrderID is required.")]
        public List<String> AddedText { get; set; }

        public List<int> AddedNumbers { get; set; }

        public void setIsSelectedSize(int size)
        {
            IsSelected = new List<bool>(size);
        }
        public CombinedViewModel()
        {
            Items = new List<Item>();
            reorderdetail = new List<ReorderDetail>();
        }
        public List<UploadModel> File { get; set; }

        public List<OutstandingRequisitionView> OutstandingReq { get; set; }

        public List<EmailFormModel> EmailForm { get; set; }

        public List<Category> category { get; set; }
        [Required(ErrorMessage = "Employee is required.")]
        public List<Employee> Employee { get; set; }

        public List<SupplierItem> supplierItems { get; set; }

        public List<RequisitionDetail> RequisitionDetails { get; set; }

        public List<Department> Departments { get; set; }

        public List<Supplier> Suppliers { get; set; }

        public List<int> Quantity { get; set; }

        public string trialText { get; set; }

        public List<OrderSupplier> OrderSuppliers { get; set; }

        public List<int> OrderIds { get; set; }

        public List<OrderSupplierDetail> OrderSupplierDetails { get; set; }

        public List<Disbursement> Disbursements { get; set; }

        public List<DisbursementDetail> DisbursementDetails { get; set; }

        public List<Employee> Employees { get; set; }

        public List<int> ReOrderItemQty { get; set; }

        public Dictionary<int, int> SupplierItemIdAndQtyAvail { get; set; }

        public Dictionary<string, int> ItemIdAndQty { get; set; }

        public List<OutstandingRequisitionView> OutstandingRequisitionViews { get; set; }

        public List<SupplierItem> SupplierItem { get; set; }

        public List<RequisitionDetail> Details { get; set; }

        public List<Requisition> Requisition { get; set; }

        public List<Supplier> Supplier { get; set; }

        public List<ReorderDetail> reorderdetail { get; set; }

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
        public List<RadioButtonData> RadioButtonListData { get; set; }
        public List<string> ApprovalStatusNames { get; internal set; }
        public List<OutstandingRequisitionRow> OutstandingRequisitionRows { get; set; }
    }
}