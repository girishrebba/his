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
    
    public partial class PatientPrescription
    {
        public int PrescriptionID { get; set; }
        public string ENMRNO { get; set; }
        public int MedInventoryID { get; set; }
        public int Quantity { get; set; }
        public int IntakeFrequencyID { get; set; }
        public string Cooments { get; set; }
    
        public virtual IntakeFrequency IntakeFrequency { get; set; }
        public virtual MedicineInventory MedicineInventory { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
