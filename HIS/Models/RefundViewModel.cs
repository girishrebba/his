using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public class RefundViewModel
    {
        public int RefundID { get; set; }
        public int VisitID { get; set; }
        public string ENMRNO { get; set; }
        public string VisitName { get; set; }
        public decimal BillAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal RefundAmount { get; set; }
        public string RefundReason { get; set; }
        public decimal PrevRefAmount { get; set; }
    }
}