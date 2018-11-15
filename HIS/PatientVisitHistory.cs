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
    
    public partial class PatientVisitHistory
    {
        public int SNO { get; set; }
        public System.DateTime DateOfVisit { get; set; }
        public int ConsultTypeID { get; set; }
        public string ENMRNO { get; set; }
        public decimal Fee { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public int DoctorID { get; set; }
        public string Purpose { get; set; }
        public Nullable<int> HeartBeat { get; set; }
        public string BMI { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string BP { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<int> NurseID { get; set; }
    
        public virtual ConsultationType ConsultationType { get; set; }
        public virtual User User { get; set; }
        public virtual OutPatient OutPatient { get; set; }
    }
}
