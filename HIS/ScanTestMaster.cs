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
    
    public partial class ScanTestMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScanTestMaster()
        {
            this.PatientScans = new HashSet<PatientScan>();
        }
    
        public int STMID { get; set; }
        public string ENMRNO { get; set; }
        public int PrescribedBy { get; set; }
        public Nullable<System.DateTime> DatePrescribed { get; set; }
        public Nullable<int> VisitID { get; set; }
        public Nullable<bool> IsBillPaid { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<bool> ISIP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientScan> PatientScans { get; set; }
        public virtual User User { get; set; }
    }
}
