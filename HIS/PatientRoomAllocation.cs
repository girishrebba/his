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
    
    public partial class PatientRoomAllocation
    {
        public int AllocationID { get; set; }
        public string ENMRNO { get; set; }
        public int RoomNo { get; set; }
        public int BedNo { get; set; }
        public System.DateTime FromDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> AllocationStatus { get; set; }
    
        public virtual InPatient InPatient { get; set; }
        public virtual Room Room { get; set; }
    }
}
