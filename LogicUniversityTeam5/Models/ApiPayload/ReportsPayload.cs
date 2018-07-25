using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogicUniversityTeam5
{
    public class ReorderCostReportPayload
    {
        public int category { get; set; }
        public List<string> department { get; set; }
        public int duration { get; set; }
        public List<int> option { get; set; }
    }
}