﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HISDBEntities : DbContext
    {
        public HISDBEntities()
            : base("name=HISDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<BrandCategory> BrandCategories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<ConsultationFee> ConsultationFees { get; set; }
        public virtual DbSet<ConsultationType> ConsultationTypes { get; set; }
        public virtual DbSet<InPatientHistory> InPatientHistories { get; set; }
        public virtual DbSet<IntakeFrequency> IntakeFrequencies { get; set; }
        public virtual DbSet<PatientVisitHistory> PatientVisitHistories { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<InPatient> InPatients { get; set; }
        public virtual DbSet<OutPatient> OutPatients { get; set; }
        public virtual DbSet<FeeCollection> FeeCollections { get; set; }
        public virtual DbSet<MedicineInventory> MedicineInventories { get; set; }
        public virtual DbSet<MedicineMaster> MedicineMasters { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<PatientRoomAllocation> PatientRoomAllocations { get; set; }
        public virtual DbSet<PatientPrescription> PatientPrescriptions { get; set; }
        public virtual DbSet<BedType> BedTypes { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<Bed> Beds { get; set; }
    
        public virtual int ConvertOutPatientToInPatient(string eNMRNO)
        {
            var eNMRNOParameter = eNMRNO != null ?
                new ObjectParameter("ENMRNO", eNMRNO) :
                new ObjectParameter("ENMRNO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ConvertOutPatientToInPatient", eNMRNOParameter);
        }
    }
}
