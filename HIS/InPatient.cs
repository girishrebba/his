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
    
    public partial class InPatient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InPatient()
        {
            this.InPatientHistories = new HashSet<InPatientHistory>();
            this.PatientVisitHistories = new HashSet<PatientVisitHistory>();
            this.FeeCollections = new HashSet<FeeCollection>();
            this.PatientRoomAllocations = new HashSet<PatientRoomAllocation>();
        }
    
        public int SNO { get; set; }
        public string ENMRNO { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string BirthPlace { get; set; }
        public string Profession { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int BloodGroupID { get; set; }
        public Nullable<int> MaritalStatus { get; set; }
        public string ReferredBy { get; set; }
        public string RefPhone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PinCode { get; set; }
        public Nullable<System.DateTime> Enrolled { get; set; }
        public string Purpose { get; set; }
        public int DoctorID { get; set; }
        public Nullable<bool> Mask { get; set; }
        public string PatientHistory { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<decimal> BMI { get; set; }
        public Nullable<int> HeartBeat { get; set; }
        public Nullable<decimal> BP { get; set; }
        public Nullable<decimal> Temperature { get; set; }
    
        public virtual BloodGroup BloodGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InPatientHistory> InPatientHistories { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientVisitHistory> PatientVisitHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeeCollection> FeeCollections { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientRoomAllocation> PatientRoomAllocations { get; set; }
    }
}
