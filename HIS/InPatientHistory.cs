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
    
    public partial class InPatientHistory
    {
        public int SNO { get; set; }
        public string ENMRNO { get; set; }
        public string Observations { get; set; }
        public Nullable<System.DateTime> ObservationDate { get; set; }
        public int DoctorID { get; set; }
    
        public virtual User User { get; set; }
        public virtual InPatient InPatient { get; set; }
    }
}
