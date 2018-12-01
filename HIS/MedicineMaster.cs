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
    
    public partial class MedicineMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicineMaster()
        {
            this.MedicineInventories = new HashSet<MedicineInventory>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
            this.PatientPrescriptions = new HashSet<PatientPrescription>();
            this.OrderRequests = new HashSet<OrderRequest>();
            this.PharmaKitItems = new HashSet<PharmaKitItem>();
        }
    
        public int MMID { get; set; }
        public int BrandID { get; set; }
        public int BrandCategoryID { get; set; }
        public string MedicineName { get; set; }
        public string MedDose { get; set; }
        public Nullable<int> TriggerQty { get; set; }
        public int SubCategoryID { get; set; }
        public int SupplierID { get; set; }
    
        public virtual BrandCategory BrandCategory { get; set; }
        public virtual Brand Brand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicineInventory> MedicineInventories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientPrescription> PatientPrescriptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderRequest> OrderRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PharmaKitItem> PharmaKitItems { get; set; }
    }
}
