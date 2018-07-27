using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5
{
    public class StockVoucherPayload
    {
        public string ItemID { get; set; }
        public int ActualCount { get; set; }
        public string EmployeeID { get; set; }
        public string Reason { get; set; }
    }
}