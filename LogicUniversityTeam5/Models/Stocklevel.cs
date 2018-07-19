using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityTeam5.Models
{
    public class Stocklevel
    {
        public int Reorderlevel { get; set; }
        public int Currentstock { get; set; }
        public int ReorderQuantity { get; set; }
    }
}