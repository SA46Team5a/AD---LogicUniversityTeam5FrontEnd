using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Models
{
    public class ItemCatalogueModel
    {
        public List<Item> items { get; set; }

        public List<Category> categories { get; set; }

        public List<Stocklevel> stocklevels { get; set; }
    }
}