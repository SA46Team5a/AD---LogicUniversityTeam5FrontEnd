using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceLayer;
using ServiceLayer.DataAccess;

namespace LogicUniversityTeam5.Models
{
    public class ReportingModel
    {
        public List<Category> categories { get; set; }
        public List<Department> departments { get; set; }
    }
}