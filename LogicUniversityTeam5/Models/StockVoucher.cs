using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityTeam5.Models
{
    public class StockVoucher
    {
        public String discrepancyId { get; set; }
        public int originalcount { get; set; }
        public int actualcount { get; set; }
        public decimal ItemCost { get; set; }
        public string Reason { get; set; }
        public string RaisedBy { get; set; }
        public string RaisedByDate { get; set; }
        public bool isNewlyEnrolled { get; set; }
        public string Password { get; set; }
    }
   
    public class MasterDetails
    {
        public List<StockVoucher> CustPersonal { get; set; }
        public List<Items> CustRegions { get; set; }

    }
}