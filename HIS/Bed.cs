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
    
    public partial class Bed
    {
        public int BedNo { get; set; }
        public string BedName { get; set; }
        public string Description { get; set; }
        public Nullable<int> BedStatus { get; set; }
        public int BedTypeID { get; set; }
        public Nullable<int> RoomNo { get; set; }
        public Nullable<System.DateTime> NextAvailbility { get; set; }
    
        public virtual Room Room { get; set; }
    }
}
