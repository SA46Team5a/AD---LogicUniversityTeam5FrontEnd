using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogicUniversityTeam5.Models;
namespace LogicUniversityTeam5.Models
{
    public class ItemCatalogue
    {
        public List<Items> item { get; set; }


        public List<Stocklevel> stocklevel { get; set; }
    }
}