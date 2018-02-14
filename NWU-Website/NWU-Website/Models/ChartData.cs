using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWU_Website.Models
{
    public class ChartData
    {
        public List<int> CurrentNumber { get; set; }
        public List<int> MaxNumber { get; set; }
        public List<string> AreaNames { get; set; }

        public ChartData()
        {
            CurrentNumber = new List<int>();
            MaxNumber = new List<int>();
            AreaNames = new List<string>();
        }
    }
}