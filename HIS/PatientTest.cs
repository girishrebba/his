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
    
    public partial class PatientTest
    {
        public int SNO { get; set; }
        public string ENMRNO { get; set; }
        public int TestID { get; set; }
        public Nullable<System.DateTime> TestDate { get; set; }
        public int PrescribedDoctor { get; set; }
        public Nullable<int> VisitID { get; set; }
        public Nullable<decimal> RecordedValues { get; set; }
        public string TestImpression { get; set; }
        public string ReportPath { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> PaidAmunt { get; set; }
    
        public virtual TestType TestType { get; set; }
        public virtual User User { get; set; }
    }
}
