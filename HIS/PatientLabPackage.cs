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
    
    public partial class PatientLabPackage
    {
        public int PLabPackID { get; set; }
        public string ENMRNO { get; set; }
        public int LkitID { get; set; }
        public Nullable<bool> IsOP { get; set; }
    
        public virtual LabKit LabKit { get; set; }
    }
}
