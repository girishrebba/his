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
    
    public partial class IpRevenueReport_Result
    {
        public string ENMRNO { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientLasttName { get; set; }
        public int DoctorID { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorLasttName { get; set; }
        public Nullable<decimal> consultationFee { get; set; }
        public Nullable<decimal> feeDiscount { get; set; }
        public Nullable<decimal> LabTotalAmount { get; set; }
        public Nullable<decimal> LabTestPaidAmount { get; set; }
        public Nullable<decimal> LabTestDiscount { get; set; }
        public Nullable<decimal> ScanTotalAmount { get; set; }
        public Nullable<decimal> ScanTestPaidAmount { get; set; }
        public Nullable<decimal> ScanTestDiscount { get; set; }
    }
}
