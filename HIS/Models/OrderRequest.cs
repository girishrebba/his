using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public partial class OrderRequest
    {
        public string OrderNo { get; set; }
        public string MedicineWithDose { get; set; }
        public string OrderDate { get; set; }
    }
}