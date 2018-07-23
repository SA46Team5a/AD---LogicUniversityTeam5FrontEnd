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

        public List<Category> Categories { get; set; }

        public RequisitionDetail Requisitions { get; set; }

        public List<StockCountItem> StockCountItems { get; set; }

        public List<StockVoucher> StockVouchers { get; set; }

        public List<Order> Orders { get; set; }

        public List<bool> IsSelected { get; set; }

        public bool IsSave { get; set; }

        public List<String> AddedText { get; set; }

        public List<int> AddedNumbers { get; set; }

        public void setIsSelectedSize(int size)
        {
            IsSelected = new List<bool>(size);
        }

        public void setAddedTextSize(int size)
        {
            AddedText = new List<String>(size);
        }

        public void setAddedNumbersSize(int size)
        {
            AddedNumbers = new List<int>(size);
        }

    }
}