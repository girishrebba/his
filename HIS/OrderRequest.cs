//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HIS
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderRequest
    {
        public int ORID { get; set; }
        public int OMID { get; set; }
        public int MedicineID { get; set; }
        public int Quantity { get; set; }
        public Nullable<int> PlacedQty { get; set; }
    
        public virtual MedicineMaster MedicineMaster { get; set; }
        public virtual OrderMaster OrderMaster { get; set; }
    }
}
