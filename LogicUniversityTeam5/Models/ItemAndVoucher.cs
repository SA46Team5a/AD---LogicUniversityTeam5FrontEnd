﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LogicUniversityTeam5.Models;
namespace LogicUniversityTeam5.Models
{
    public class ItemAndVoucher
    {
        public List<Items> item { get; set; }
        public List<StockVoucher> stockVoucher { get; set; }
        public List<Dictionary<int,bool>> isSelected { get; set; }

    }
}