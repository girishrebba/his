using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public class RoomBilling
    {
        public string ENMRNO { get; set; }
        public string RoomName { get; set; }
        public string BedName { get; set; }
        public int OccupiedDays { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal Total { get; set; }
    }
}