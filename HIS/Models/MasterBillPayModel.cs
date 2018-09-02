using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public partial class MasterBillPayModel
    {
        public int ID { get; set; }
        public string ENMRNO { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
    }
}