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
    
    public partial class PatientScan
    {
        public int PSID { get; set; }
        public int STMID { get; set; }
        public int ScanID { get; set; }
        public Nullable<System.DateTime> ScanDate { get; set; }
        public Nullable<decimal> RecordedValues { get; set; }
        public string TestImpression { get; set; }
        public string ReportPath { get; set; }
    
        public virtual TestType TestType { get; set; }
        public virtual ScanTestMaster ScanTestMaster { get; set; }
        public virtual Scan Scan { get; set; }
    }
}
