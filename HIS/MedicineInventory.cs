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
    
    public partial class MedicineInventory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicineInventory()
        {
            this.PatientPrescriptions = new HashSet<PatientPrescription>();
        }
    
        public int MedInventoryID { get; set; }
        public int BrandID { get; set; }
        public int BrandCategoryID { get; set; }
        public string MedicineName { get; set; }
        public int AvailableQty { get; set; }
        public Nullable<decimal> PricePerItem { get; set; }
        public Nullable<decimal> PricePerSheet { get; set; }
        public string BatchNo { get; set; }
        public string LotNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    
        public virtual BrandCategory BrandCategory { get; set; }
        public virtual Brand Brand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientPrescription> PatientPrescriptions { get; set; }
    }
}
