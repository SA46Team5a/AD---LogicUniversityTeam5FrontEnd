using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityTeam5.Models
{
    public class OrderItem
    {
        public string category { get; set; }
        public string description { get; set; }
        public int reorderlevel { get; set; }
        public int currentstock { get; set; }
        public int recorderquantity { get; set; }
       // public List<OrderItem> order { get; set; }


    }
}