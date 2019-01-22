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
    
        public virtual DbSet<Bed> Beds { get; set; }
        public virtual DbSet<BedType> BedTypes { get; set; }
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<BrandCategory> BrandCategories { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<ConsultationFee> ConsultationFees { get; set; }
        public virtual DbSet<ConsultationType> ConsultationTypes { get; set; }
        public virtual DbSet<InPatientHistory> InPatientHistories { get; set; }
        public virtual DbSet<IntakeFrequency> IntakeFrequencies { get; set; }
        public virtual DbSet<MedicineInventory> MedicineInventories { get; set; }
        public virtual DbSet<MedicineMaster> MedicineMasters { get; set; }
        public virtual DbSet<PatientPrescription> PatientPrescriptions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PrescriptionMaster> PrescriptionMasters { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<TestType> TestTypes { get; set; }
        public virtual DbSet<UserPermission> UserPermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<LabTestMaster> LabTestMasters { get; set; }
        public virtual DbSet<PatientTest> PatientTests { get; set; }
        public virtual DbSet<PatientRoomAllocation> PatientRoomAllocations { get; set; }
        public virtual DbSet<PaymentMode> PaymentModes { get; set; }
        public virtual DbSet<OrderMaster> OrderMasters { get; set; }
        public virtual DbSet<OrderRequest> OrderRequests { get; set; }
        public virtual DbSet<ScanCategory> ScanCategories { get; set; }
        public virtual DbSet<PatientScan> PatientScans { get; set; }
        public virtual DbSet<ScanTestMaster> ScanTestMasters { get; set; }
        public virtual DbSet<Consultant> Consultants { get; set; }
        public virtual DbSet<InsuranceProvider> InsuranceProviders { get; set; }
        public virtual DbSet<LabKit> LabKits { get; set; }
        public virtual DbSet<PatientInsurance> PatientInsurances { get; set; }
        public virtual DbSet<PatientPurpose> PatientPurposes { get; set; }
        public virtual DbSet<PharmaKitItem> PharmaKitItems { get; set; }
        public virtual DbSet<PharmaKit> PharmaKits { get; set; }
        public virtual DbSet<Scan> Scans { get; set; }
        public virtual DbSet<ConsultantVisit> ConsultantVisits { get; set; }
        public virtual DbSet<LabKitItem> LabKitItems { get; set; }
        public virtual DbSet<Purpose> Purposes { get; set; }
        public virtual DbSet<BrandSubCategory> BrandSubCategories { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<PatientVisitHistory> PatientVisitHistories { get; set; }
        public virtual DbSet<FeeCollection> FeeCollections { get; set; }
        public virtual DbSet<InPatient> InPatients { get; set; }
        public virtual DbSet<OutPatient> OutPatients { get; set; }
    
        public virtual int ConvertOutPatientToInPatient(string eNMRNO, Nullable<decimal> estAmount, Nullable<decimal> advAmount)
        {
            var eNMRNOParameter = eNMRNO != null ?
                new ObjectParameter("ENMRNO", eNMRNO) :
                new ObjectParameter("ENMRNO", typeof(string));
    
            var estAmountParameter = estAmount.HasValue ?
                new ObjectParameter("EstAmount", estAmount) :
                new ObjectParameter("EstAmount", typeof(decimal));
    
            var advAmountParameter = advAmount.HasValue ?
                new ObjectParameter("AdvAmount", advAmount) :
                new ObjectParameter("AdvAmount", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ConvertOutPatientToInPatient", eNMRNOParameter, estAmountParameter, advAmountParameter);
        }
    
        public virtual int CreateMasterPrescription(string eNMRNO, Nullable<int> doctorID, Nullable<int> visitID, Nullable<bool> iSIP, ObjectParameter pMID)
        {
            var eNMRNOParameter = eNMRNO != null ?
                new ObjectParameter("ENMRNO", eNMRNO) :
                new ObjectParameter("ENMRNO", typeof(string));
    
            var doctorIDParameter = doctorID.HasValue ?
                new ObjectParameter("DoctorID", doctorID) :
                new ObjectParameter("DoctorID", typeof(int));
    
            var visitIDParameter = visitID.HasValue ?
                new ObjectParameter("VisitID", visitID) :
                new ObjectParameter("VisitID", typeof(int));
    
            var iSIPParameter = iSIP.HasValue ?
                new ObjectParameter("ISIP", iSIP) :
                new ObjectParameter("ISIP", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterPrescription", eNMRNOParameter, doctorIDParameter, visitIDParameter, iSIPParameter, pMID);
        }
    
        public virtual int CreateMasterLabTest(string eNMRNO, Nullable<int> doctorID, Nullable<int> visitID, Nullable<bool> iSIP, ObjectParameter lTMID)
        {
            var eNMRNOParameter = eNMRNO != null ?
                new ObjectParameter("ENMRNO", eNMRNO) :
                new ObjectParameter("ENMRNO", typeof(string));
    
            var doctorIDParameter = doctorID.HasValue ?
                new ObjectParameter("DoctorID", doctorID) :
                new ObjectParameter("DoctorID", typeof(int));
    
            var visitIDParameter = visitID.HasValue ?
                new ObjectParameter("VisitID", visitID) :
                new ObjectParameter("VisitID", typeof(int));
    
            var iSIPParameter = iSIP.HasValue ?
                new ObjectParameter("ISIP", iSIP) :
                new ObjectParameter("ISIP", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterLabTest", eNMRNOParameter, doctorIDParameter, visitIDParameter, iSIPParameter, lTMID);
        }
    
        [DbFunction("HISDBEntities", "CSVToTable")]
        public virtual IQueryable<Nullable<int>> CSVToTable(string inStr)
        {
            var inStrParameter = inStr != null ?
                new ObjectParameter("InStr", inStr) :
                new ObjectParameter("InStr", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<Nullable<int>>("[HISDBEntities].[CSVToTable](@InStr)", inStrParameter);
        }
    
        public virtual ObjectResult<InventoryReport_Result> InventoryReport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<InventoryReport_Result>("InventoryReport");
        }
    
        public virtual int CreateMasterScanTest(string eNMRNO, Nullable<int> doctorID, Nullable<int> visitID, Nullable<bool> iSIP, ObjectParameter sTMID)
        {
            var eNMRNOParameter = eNMRNO != null ?
                new ObjectParameter("ENMRNO", eNMRNO) :
                new ObjectParameter("ENMRNO", typeof(string));
    
            var doctorIDParameter = doctorID.HasValue ?
                new ObjectParameter("DoctorID", doctorID) :
                new ObjectParameter("DoctorID", typeof(int));
    
            var visitIDParameter = visitID.HasValue ?
                new ObjectParameter("VisitID", visitID) :
                new ObjectParameter("VisitID", typeof(int));
    
            var iSIPParameter = iSIP.HasValue ?
                new ObjectParameter("ISIP", iSIP) :
                new ObjectParameter("ISIP", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterScanTest", eNMRNOParameter, doctorIDParameter, visitIDParameter, iSIPParameter, sTMID);
        }
    
        public virtual int CreateMasterLabKit(string lKitName, Nullable<decimal> lKitCost, ObjectParameter lKitID)
        {
            var lKitNameParameter = lKitName != null ?
                new ObjectParameter("LKitName", lKitName) :
                new ObjectParameter("LKitName", typeof(string));
    
            var lKitCostParameter = lKitCost.HasValue ?
                new ObjectParameter("LKitCost", lKitCost) :
                new ObjectParameter("LKitCost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterLabKit", lKitNameParameter, lKitCostParameter, lKitID);
        }
    
        public virtual int CreateMasterPharmaKit(string pKitName, Nullable<decimal> pKitCost, ObjectParameter pKitID)
        {
            var pKitNameParameter = pKitName != null ?
                new ObjectParameter("PKitName", pKitName) :
                new ObjectParameter("PKitName", typeof(string));
    
            var pKitCostParameter = pKitCost.HasValue ?
                new ObjectParameter("PKitCost", pKitCost) :
                new ObjectParameter("PKitCost", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterPharmaKit", pKitNameParameter, pKitCostParameter, pKitID);
        }
    
        public virtual ObjectResult<ConsutantPayReport_Result> ConsutantPayReport(string doctors, Nullable<System.DateTime> start_Time, Nullable<System.DateTime> end_Time)
        {
            var doctorsParameter = doctors != null ?
                new ObjectParameter("Doctors", doctors) :
                new ObjectParameter("Doctors", typeof(string));
    
            var start_TimeParameter = start_Time.HasValue ?
                new ObjectParameter("Start_Time", start_Time) :
                new ObjectParameter("Start_Time", typeof(System.DateTime));
    
            var end_TimeParameter = end_Time.HasValue ?
                new ObjectParameter("End_Time", end_Time) :
                new ObjectParameter("End_Time", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ConsutantPayReport_Result>("ConsutantPayReport", doctorsParameter, start_TimeParameter, end_TimeParameter);
        }
    
        public virtual int CreateMasterOrder(string orderNo, ObjectParameter oMID)
        {
            var orderNoParameter = orderNo != null ?
                new ObjectParameter("OrderNo", orderNo) :
                new ObjectParameter("OrderNo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CreateMasterOrder", orderNoParameter, oMID);
        }
    
        public virtual ObjectResult<BedReport_Result> BedReport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BedReport_Result>("BedReport");
        }
    
        public virtual int AddSubCategory(string name, ObjectParameter subCategoryID)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddSubCategory", nameParameter, subCategoryID);
        }
    
        public virtual int AddSupplier(string name, ObjectParameter supplierID)
        {
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddSupplier", nameParameter, supplierID);
        }
    
        public virtual ObjectResult<RevenueReport_Result> RevenueReport(string doctors, Nullable<System.DateTime> start_Time, Nullable<System.DateTime> end_Time)
        {
            var doctorsParameter = doctors != null ?
                new ObjectParameter("Doctors", doctors) :
                new ObjectParameter("Doctors", typeof(string));
    
            var start_TimeParameter = start_Time.HasValue ?
                new ObjectParameter("Start_Time", start_Time) :
                new ObjectParameter("Start_Time", typeof(System.DateTime));
    
            var end_TimeParameter = end_Time.HasValue ?
                new ObjectParameter("End_Time", end_Time) :
                new ObjectParameter("End_Time", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<RevenueReport_Result>("RevenueReport", doctorsParameter, start_TimeParameter, end_TimeParameter);
        }
    
        public virtual ObjectResult<IpRevenueReport_Result> IpRevenueReport(string doctors, Nullable<System.DateTime> start_Time, Nullable<System.DateTime> end_Time)
        {
            var doctorsParameter = doctors != null ?
                new ObjectParameter("Doctors", doctors) :
                new ObjectParameter("Doctors", typeof(string));
    
            var start_TimeParameter = start_Time.HasValue ?
                new ObjectParameter("Start_Time", start_Time) :
                new ObjectParameter("Start_Time", typeof(System.DateTime));
    
            var end_TimeParameter = end_Time.HasValue ?
                new ObjectParameter("End_Time", end_Time) :
                new ObjectParameter("End_Time", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<IpRevenueReport_Result>("IpRevenueReport", doctorsParameter, start_TimeParameter, end_TimeParameter);
        }
    }
}
