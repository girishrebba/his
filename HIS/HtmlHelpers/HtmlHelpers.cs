using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Data.Entity;

namespace HIS.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string DateFormat(this HtmlHelper helper, DateTime? date)
        {
            return (date != null && date != DateTime.MinValue) ? date.Value.ToString("dd-MMM-yyyy") : string.Empty;
        }

        public static string DateFormat(DateTime? date)
        {
            return (date != null && date != DateTime.MinValue) ? date.Value.ToString("dd-MMM-yyyy") : string.Empty;
        }

        public static string GetFullName(string firstName, string middleName, string lastName)
        {
            return string.Format("{0} {1} {2}",
                string.IsNullOrEmpty(firstName) ? string.Empty : firstName,
                string.IsNullOrEmpty(middleName) ? string.Empty : middleName,
                string.IsNullOrEmpty(lastName) ? string.Empty : lastName);
        }

        public static string PayTypeDisplay(int type)
        {
            if (type == 1)
                return "Advance";
            else if (type == 2)
                return "Charge";
            else if (type == 3)
                return "Refund";
            else return string.Empty;
        }

        public static string GetMedicineWithDose(string medicineName, string dose)
        {
            return medicineName;
            //return string.Format("{0} - {1}", medicineName, dose);
        }

        public static string GetMedicineWithDoseAvailableQty(string medicineName, string dose, int qty)
        {
            return string.Format("{0} - {1}(Available - {2})", medicineName, dose, qty);
        }

        public static string GetMedicineCategoryWithDoseAvailableQty(string subCategory, string category,
            string medicineName, string dose, int qty)
        {
            return string.Format("{0} -> {1} -> {2} - {3} (Available - {4})", category, !(string.IsNullOrEmpty(subCategory)) ? subCategory : "N/A", medicineName, dose, qty);
        }

        public static string Get_IN_ENMR(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities hs = new HISDBEntities())
            {
                enmrNo = hs.InPatients.Where(op => op.SNO == id).FirstOrDefault().ENMRNO;
            }
            return enmrNo;
        }

        public static string Get_OUT_ENMR(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities hs = new HISDBEntities())
            {
                enmrNo = hs.OutPatients.Where(op => op.SNO == id).FirstOrDefault().ENMRNO;
            }
            return enmrNo;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string GetFullAddress(string address1, string address2, string city, string state, string zip)
        {
            return string.Format("{0} {1} {2} {3} {4}",
                string.IsNullOrEmpty(address1) ? string.Empty : address1 + ",",
                string.IsNullOrEmpty(address2) ? string.Empty : address2+ ",",
                string.IsNullOrEmpty(city) ? string.Empty : city + ",",
                string.IsNullOrEmpty(state) ? string.Empty : state,
                string.IsNullOrEmpty(zip) ? string.Empty : "- " + zip);
        }
        public static string GetGender(int? gender)
        {
            if (gender != null)
            {
                if (gender == 0)
                    return "Male";
                else if (gender == 1)
                    return "Female";
                else return "Others";
            }
            else return string.Empty;
        }

        public static string GetMaritalStatus(int? status)
        {
            if (status != null)
            {
                if (status == 0)
                    return "Single";
                else if (status == 1)
                    return "Married";
                else return "Others";
            }
            else return string.Empty;       
        }

        //Fetch Brands from database
        public static List<Brand> GetBrands()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brands = (from b in dc.Brands
                              select new { b.BrandID, b.BrandName })
                              .OrderBy(b => b.BrandName).AsEnumerable()
                              .Select(x => new Brand { BrandID = x.BrandID, BrandName = x.BrandName }).ToList();
                return brands;
            }
        }

        //Fetch Brand categories from database
        public static List<BrandCategory> GetBrandCategories()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brandCategories = (from bc in dc.BrandCategories
                                       select new { bc.CategoryID, bc.Category, bc.BrandID })
                              .OrderBy(b => b.Category).AsEnumerable()
                              .Select(x => new BrandCategory { CategoryID = x.CategoryID, Category = x.Category, BrandID = x.BrandID }).ToList();
                return brandCategories;
            }
        }

        public static List<User> GetUsers()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var users = (from u in dc.Users
                             select new { u })
                              .OrderBy(b => b.u.UserName).AsEnumerable()
                              .Select(x => new User { UserID = x.u.UserID, NameDisplay = x.u.GetFullName() }).ToList();
                return users;
            }
        }

        public static List<Room> GetAvailableRooms()
        {
            List<Room> roomsList = new List<Room>();
            using (HISDBEntities hs = new HISDBEntities())
            {
                var beds = (from b in hs.Beds
                            join pb in hs.PatientRoomAllocations on b.BedNo equals pb.BedNo
                            where pb.AllocationStatus == true
                            select b.BedNo).ToList();
                var avlbeds = hs.Beds.Select(a => a.BedNo).Except(beds);
                var allbeds = (from b in hs.Beds
                               select new { b }).Where(a => avlbeds.Contains(a.b.BedNo)).AsEnumerable()
                               .Select(x => new Bed { BedNo = x.b.BedNo, RoomNo = x.b.RoomNo }).ToList();

                var bedsList = allbeds.GroupBy(test => test.RoomNo)
                   .Select(grp => grp.First())
                   .ToList();

                if (bedsList.Count() > 0)
                {
                    foreach (var b in bedsList)
                    {
                        var room = hs.Rooms.Where(r => r.RoomNo == b.RoomNo).FirstOrDefault();
                        roomsList.Add(new Room
                        {
                            RoomNo = room.RoomNo,
                            RoomName = room.RoomName
                        });
                    }
                }
                else
                {
                    roomsList = (from u in hs.Rooms
                                       select new { u })
                              .OrderBy(b => b.u.RoomNo).AsEnumerable()
                              .Select(x => new Room { RoomNo = x.u.RoomNo, RoomName = x.u.RoomName }).ToList();
                }
            }
            return roomsList;
        }

public static List<User> GetDoctors()
        {
            string[] fliter = new string[] {"Doctor","Admin" };
            using (HISDBEntities dc = new HISDBEntities())
            {
                var doctors = (from u in dc.Users
                               join ut in dc.UserTypes on u.UserTypeID equals ut.UserTypeID
                               where ut.UserTypeName.Contains("Doctor")
                               select new { u })
                               .OrderBy(b => b.u.UserName).AsEnumerable()
                               .Select(x => new User { UserID = x.u.UserID,
                                   NameDisplay = GetFullName(x.u.FirstName,x.u.MiddleName,x.u.LastName)
                               ,
                                   UserStatus = x.u.UserStatus.HasValue ? x.u.UserStatus.Value : false
                               }).ToList();
                return doctors.Where(x => x.UserStatus == true).ToList();
            }
        }
        public static List<User> GetNurses()
        {
            string[] fliter = new string[] { "Nurse"};
            using (HISDBEntities dc = new HISDBEntities())
            {
                var doctors = (from u in dc.Users
                               join ut in dc.UserTypes on u.UserTypeID equals ut.UserTypeID
                               where ut.UserTypeName.Contains("Nurse")
                               select new { u })
                               .OrderBy(b => b.u.UserName).AsEnumerable()
                               .Select(x => new User { UserID = x.u.UserID, NameDisplay = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                               UserStatus = x.u.UserStatus.HasValue ? x.u.UserStatus.Value : false}).ToList();
                return doctors.Where(x=>x.UserStatus == true).ToList();
            }
        }
        //Fetch Blood Groups from database
        public static List<BloodGroup> GetBloodGroups()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var bloodGroups = (from bg in dc.BloodGroups
                                   select new { bg.GroupID, bg.GroupName })
                              .OrderBy(b => b.GroupName).AsEnumerable()
                              .Select(x => new BloodGroup { GroupID = x.GroupID, GroupName = x.GroupName }).ToList();
                return bloodGroups;
            }
        }

        //Fetch Blood Groups from database
        public static List<InsuranceProvider> GetInsuranceProviders()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var InsuranceProvider = (from bg in dc.InsuranceProviders
                                   select new { bg.ProviderID, bg.ProviderName })
                              .OrderBy(b => b.ProviderName).AsEnumerable()
                              .Select(x => new InsuranceProvider { ProviderID = x.ProviderID, ProviderName = x.ProviderName }).ToList();
                return InsuranceProvider;
            }
        }

        public static List<ConsultationType> GetConsultationTypes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var consultTypes = (from ct in dc.ConsultationTypes
                             select new {ct.ConsultTypeID, ct.ConsultType }).AsEnumerable()
                             .Select(x => new ConsultationType { ConsultTypeID = x.ConsultTypeID, ConsultType = x.ConsultType }).ToList();
                return consultTypes;
            }
        }

        public static List<TestType> GetTestTypes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var testTypes = (from tt in dc.TestTypes
                                    select new { tt.TestID, tt.TestName,tt.IsKit }).AsEnumerable()
                             .Select(x => new TestType {
                                 TestID = x.TestID,
                                 TestName = x.TestName,
                                 IsKit = x.IsKit
                             }).ToList();
                return testTypes;
            }
        }

        public static List<PaymentMode> GetPaymentModes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var payModes = (from pt in dc.PaymentModes
                                 select new { pt.ModeID, pt.Mode }).AsEnumerable()
                             .Select(x => new PaymentMode { ModeID = x.ModeID, Mode = x.Mode }).ToList();
                return payModes;
            }
        }

        public static List<Purpose> GetPurposes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var purposes = (from p in dc.Purposes
                                select new { p.PurposeID, p.PurposeName }).AsEnumerable()
                             .Select(x => new Purpose { PurposeID = x.PurposeID, PurposeName = x.PurposeName }).ToList();
                return purposes;
            }
        }

        public static List<Supplier> GetSuppliers()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var suppliers = (from s in dc.Suppliers
                                select new { s.SupplierID, s.SupplierName }).AsEnumerable()
                             .Select(x => new Supplier { SupplierID = x.SupplierID, SupplierName = x.SupplierName }).ToList();
                return suppliers;
            }
        }

        public static List<BrandSubCategory> GetSubCategories()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var subCats = (from bsc in dc.BrandSubCategories
                                 select new { bsc.SubCategoryID, bsc.SubCategory }).AsEnumerable()
                             .Select(x => new BrandSubCategory { SubCategoryID = x.SubCategoryID, SubCategory = x.SubCategory }).ToList();
                return subCats;
            }
        }

        public static List<IntakeFrequency> GetIntakes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var intakes = (from ifs in dc.IntakeFrequencies
                                    select new { ifs.FrequencyID, ifs.Frequency }).AsEnumerable()
                             .Select(x => new IntakeFrequency { FrequencyID = x.FrequencyID,
                                 Frequency = x.Frequency }).ToList();
                return intakes;
            }
        }

        public static List<ConsultationFee> GetConsultationFees()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var consultationFees = (from ct in dc.ConsultationFees
                                    select new { ct }).AsEnumerable()
                             .Select(x => new ConsultationFee {
                                ConsultationID  = x.ct.ConsultationID,
                                 DoctorID = x.ct.DoctorID,
                                 ConsultTypeID = x.ct.ConsultTypeID,
                                 Fee = x.ct.Fee
                             }).ToList();
                return consultationFees;
            }
        }

        public static List<Scan> GetScans()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var scans = (from s in dc.Scans
                              select new { s.ScanID, s.ScanName, s.ScanCost })
                              .OrderBy(b => b.ScanName).AsEnumerable()
                              .Select(x => new Scan { ScanID = x.ScanID, ScanName = x.ScanName, ScanCost = x.ScanCost }).ToList();
                return scans;
            }
        }

        public static List<PharmaKit> GetPharmaKits()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var pkits = (from pk in dc.PharmaKits
                             select new { pk.PKitID, pk.PKitName, pk.PKitCost })
                              .OrderBy(b => b.PKitName).AsEnumerable()
                              .Select(x => new PharmaKit
                              {
                                  PKitID = x.PKitID,
                                  PKitName = x.PKitName,
                                  PackDisplay = string.Format("{0}- {1}", x.PKitName, x.PKitCost.HasValue ? x.PKitCost.Value.ToString("F") : string.Empty)
                              }).ToList();
                return pkits;
            }
        }

        public static List<LabKit> GetLabKits()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var lkits = (from lk in dc.LabKits
                             select new { lk.LKitID, lk.LKitName })
                              .OrderBy(b => b.LKitName).AsEnumerable()
                              .Select(x => new LabKit { LKitID = x.LKitID,
                                  LKitName = x.LKitName }).ToList();
                return lkits;
            }
        }

        public static List<Specialization> GetSpecializations()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var specializations = (from s in dc.Specializations
                                       select new { s.SpecializationID, s.DoctorType })
                              .OrderBy(b => b.DoctorType).AsEnumerable()
                              .Select(x => new Specialization { SpecializationID = x.SpecializationID, DoctorType = x.DoctorType }).ToList();
                return specializations;
            }
        }

        public static List<MedicineMaster> GetMedicinesWithDose()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var lkits = (from mm in dc.MedicineMasters
                             select new { mm })
                              .OrderBy(b => b.mm.MedicineName).AsEnumerable()
                              .Select(x => new MedicineMaster
                              {
                                  MMID = x.mm.MMID,
                                  MedicineDisplay = GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose)
                              }).ToList();
                return lkits;
            }
        }

        public static string GetSequencedEnmrNo()
        {
            string initial = ConfigurationManager.AppSettings["EnmrNoStartsWith"];
            int ipCount = 0, opCount = 0;
            //string ipEnmr = "0", opEnmr = "0";
            //InPatient in_Patient;
            //OutPatient out_Patient;
            //using (HISDBEntities db = new HISDBEntities())
            //{
            //    in_Patient = db.InPatients.OrderByDescending(ip => ip.SNO).FirstOrDefault();
            //    out_Patient = db.OutPatients.OrderByDescending(ip => ip.SNO).FirstOrDefault();
            //}

            //if (in_Patient != null) ipEnmr = in_Patient.ENMRNO;
            //if (out_Patient != null) opEnmr = out_Patient.ENMRNO;
            using (HISDBEntities db = new HISDBEntities())
            {
                ipCount = (from ip in db.InPatients
                           where !db.OutPatients.Any(o => o.ENMRNO == ip.ENMRNO)
                           select ip.ENMRNO).Count();


                opCount = db.OutPatients.Count();
            }
            return string.Format("{0}-{1}", initial, (ipCount+opCount+1).ToString().PadLeft(5,'0'));            
        }

        public static int GetNextEnmrNo(string ipEnmr, string opEnmr)
        {
            int enmrno = Math.Max(Convert.ToInt32(ipEnmr), Convert.ToInt32(opEnmr));
            return enmrno+1;
        }

        public static void CreatePatientDirectory(string patientType, string enmrNo)
        {
            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/PatientRecords/"), enmrNo);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static List<PatientVisitHistory> GetOutPatientVisits(string enmrNo)
        {
            var patientVisits = new List<PatientVisitHistory>();
            using (HISDBEntities hs = new HISDBEntities())
            {
                if (hs.OutPatients.Where(op => op.ENMRNO == enmrNo).Any())
                {
                    patientVisits = (from pv in hs.PatientVisitHistories
                                         join op in hs.OutPatients on pv.ENMRNO equals op.ENMRNO
                                         join ct in hs.ConsultationTypes on pv.ConsultTypeID equals ct.ConsultTypeID
                                         join u in hs.Users on pv.DoctorID equals u.UserID
                                         join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                         where pv.ENMRNO.Equals(enmrNo)
                                         select new
                                         {
                                             pv,
                                             u,
                                             ct.ConsultType
                                         })
                                      .OrderByDescending(b => b.pv.DateOfVisit)
                                      .AsEnumerable()
                                     .Select(x => new PatientVisitHistory
                                     {
                                         DOVDisplay = DateFormat(x.pv.DateOfVisit),
                                         ENMRNO = x.pv.ENMRNO,
                                         ConsultType = x.ConsultType,
                                         Fee = x.pv.Fee,
                                         Discount = x.pv.Discount,
                                         DoctorID = x.pv.DoctorID,
                                         ConsultTypeID = x.pv.ConsultTypeID,
                                         DateOfVisit = x.pv.DateOfVisit,
                                         DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                         Purpose = x.pv.Purpose
                                     }).ToList();
                }

                // Bind the Valid till date
                if (patientVisits != null && patientVisits.Count() > 0)
                {
                    var purposeList = GetPurposes();
                    foreach (PatientVisitHistory pv in patientVisits)
                    {
                        var cfee = hs.ConsultationFees.Where(cf => cf.DoctorID == pv.DoctorID && cf.ConsultTypeID == pv.ConsultTypeID).FirstOrDefault();
                        if (cfee != null)
                        {
                            int days = cfee.ValidDays.HasValue ? cfee.ValidDays.Value : 0;
                            pv.ValidDate = DateFormat(pv.DateOfVisit.AddDays(days));
                        }
                        else
                        {
                            pv.ValidDate = pv.DOVDisplay;
                        }
                        pv.Purpose = GetPurpose(pv.Purpose, purposeList);
                    }
                }

                return patientVisits;
            }
        }

        public static List<PatientVisitHistory> GetOutPatientVisit(string enmrNo,int visitid)
        {
            var patientVisits = new List<PatientVisitHistory>();
            using (HISDBEntities hs = new HISDBEntities())
            {
                if (hs.OutPatients.Where(op => op.ENMRNO == enmrNo).Any())
                {
                    patientVisits = (from pv in hs.PatientVisitHistories
                                     join op in hs.OutPatients on pv.ENMRNO equals op.ENMRNO
                                     join ct in hs.ConsultationTypes on pv.ConsultTypeID equals ct.ConsultTypeID
                                     join u in hs.Users on pv.DoctorID equals u.UserID
                                     join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                     where pv.ENMRNO.Equals(enmrNo) && pv.SNO==visitid
                                     select new
                                     {
                                         pv,
                                         u,
                                         ct.ConsultType
                                     })
                                      .OrderByDescending(b => b.pv.DateOfVisit)
                                      .AsEnumerable()
                                     .Select(x => new PatientVisitHistory
                                     {
                                         DOVDisplay = DateFormat(x.pv.DateOfVisit),
                                         ENMRNO = x.pv.ENMRNO,
                                         ConsultType = x.ConsultType,
                                         Fee = x.pv.Fee,
                                         Discount = x.pv.Discount,
                                         DoctorID = x.pv.DoctorID,
                                         ConsultTypeID = x.pv.ConsultTypeID,
                                         DateOfVisit = x.pv.DateOfVisit,
                                         DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                         Purpose = x.pv.Purpose
                                     }).ToList();
                }

                // Bind the Valid till date
                if (patientVisits != null && patientVisits.Count() > 0)
                {
                    var purposeList = GetPurposes();
                    foreach (PatientVisitHistory pv in patientVisits)
                    {
                        var cfee = hs.ConsultationFees.Where(cf => cf.DoctorID == pv.DoctorID && cf.ConsultTypeID == pv.ConsultTypeID).FirstOrDefault();
                        if (cfee != null)
                        {
                            int days = cfee.ValidDays.HasValue ? cfee.ValidDays.Value : 0;
                            pv.ValidDate = DateFormat(pv.DateOfVisit.AddDays(days));
                        }
                        else
                        {
                            pv.ValidDate = pv.DOVDisplay;
                        }
                        pv.Purpose = GetPurpose(pv.Purpose, purposeList);
                    }
                }

                return patientVisits;
            }
        }


        public static string GetPurpose(string purposes, List<Purpose> purposeList)
        {

            if (!string.IsNullOrEmpty(purposes) && purposeList.Count() > 0)
            {
                List<string> purposeNames = new List<string>();

                foreach (var id in purposes.Split(',').ToArray())
                {
                    purposeNames.Add(purposeList.Where(p => p.PurposeID == Convert.ToInt32
                    (id)).FirstOrDefault().PurposeName);

                }
                return string.Join(",", purposeNames);
            }
            else return string.Empty;
        }

        private static int ValidDays(int? days)
        {
            if (days.HasValue) return days.Value;
            return 0;
        }

        public static List<PatientPrescription> GetPatientPrescriptions(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID
                                            join op in hs.OutPatients on pm.ENMRNO equals op.ENMRNO
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            join pv in hs.PatientVisitHistories on pm.VisitID equals pv.SNO
                                            join ct in hs.ConsultationTypes on pv.ConsultTypeID equals ct.ConsultTypeID
                                            where pm.ENMRNO.Equals(enmrNo)
                                            select new
                                            {
                                                pp,
                                                pm,
                                                u,
                                                mm,
                                                ifs.Frequency,
                                                ct.ConsultType
                                            })
                                  .OrderByDescending(b => b.pm.DatePrescribed)
                                  .AsEnumerable()
                                 .Select(x => new PatientPrescription
                                 {
                                     DateDisplay = DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID,
                                     VisitName = x.ConsultType
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        public static List<PatientPrescription> GetPatientPrescriptionbyVisit(string enmrNo, int visitid)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID 
                                            join op in hs.OutPatients on pm.ENMRNO equals op.ENMRNO
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            join pv in hs.PatientVisitHistories on pm.VisitID equals pv.SNO
                                            join ct in hs.ConsultationTypes on pv.ConsultTypeID equals ct.ConsultTypeID
                                            where  pm.VisitID == visitid && pm.ENMRNO.Equals(enmrNo) 
                                            select new
                                            {
                                                pp,
                                                pm,
                                                u,
                                                mm,
                                                ifs.Frequency,
                                                ct.ConsultType
                                            })
                                  .OrderByDescending(b => b.pm.DatePrescribed)
                                  .AsEnumerable()
                                 .Select(x => new PatientPrescription
                                 {
                                     DateDisplay = DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID,
                                     VisitName = x.ConsultType
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        public static List<PatientPrescription> InPatientPrescriptions(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == 0 && pm.ISIP == true && pm.IsDelivered == true
                                            select new
                                            {
                                                pp,
                                                pm,
                                                u,
                                                mm,
                                                ifs.Frequency
                                            })
                                  .OrderByDescending(b => b.pm.DatePrescribed)
                                  .AsEnumerable()
                                 .Select(x => new PatientPrescription
                                 {
                                     DateDisplay = DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        public static List<PatientTest> GetPatientTests(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join vh in hs.PatientVisitHistories on ltm.VisitID equals vh.SNO
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo 
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = enmrNo,
                                     TestName = x.tt.TestName,
                                     DateDisplay = DateFormat(x.pt.TestDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     TestCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientTests;
            }
        }

        public static List<PatientTest> GetPatientTestbyVisit(string enmrNo, int visitid)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join vh in hs.PatientVisitHistories on ltm.VisitID equals vh.SNO
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID==visitid
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = enmrNo,
                                     TestName = x.tt.TestName,
                                     DateDisplay = DateFormat(x.pt.TestDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     TestCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientTests;
            }
        }


        public static List<PatientScan> GetPatientScans(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = enmrNo,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.pt.ScanDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     ScanCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientScans;
            }
        }

        public static List<PatientScan> GetPatientScanbyVisit(string enmrNo, int visitid)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitid
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = enmrNo,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.pt.ScanDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     ScanCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientScans;
            }
        }
        public static string GetVisitName(int consultTypeID)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == consultTypeID).FirstOrDefault().ConsultType;

                return consultName;
            }
        }


        public static List<PatientTest> GetInPatientTests(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID==0
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = enmrNo,
                                     TestName = x.tt.TestName,
                                     DateDisplay = DateFormat(x.pt.TestDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     TestCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientTests;
            }
        }

        public static List<PatientScan> GetInPatientScans(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == 0
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = enmrNo,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.pt.ScanDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     ScanCost = x.ltm.PaidAmount != null ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientScans;
            }
        }

        public static List<PatientScan> GetPatientVisitScansBillPay(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID && ltm.IsBillPaid == false
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.ltm.DatePrescribed),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     STMID = x.pt.STMID,
                                     ScanID = x.pt.ScanID,
                                     ScanCost = x.tt.ScanCost.HasValue ? x.tt.ScanCost.Value : 0
                                 }).ToList();

                return patientScans;
            }
        }

        public static List<PatientScan> GetPatientVisitScansBillPayPrint(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.ltm.DatePrescribed),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     STMID = x.pt.STMID,
                                     ScanID = x.pt.ScanID,
                                     ScanCost = x.tt.ScanCost.HasValue ? x.tt.ScanCost.Value : 0,
                                     TotalAmount = x.ltm.TotalAmount.HasValue ? x.ltm.TotalAmount.Value : 0,
                                     Discount = x.ltm.Discount.HasValue ? x.ltm.Discount.Value : 0,
                                     PaidAmount = x.ltm.PaidAmount.HasValue ? x.ltm.PaidAmount.Value : 0
                                 }).ToList();

                return patientScans;
            }
        }


        public static List<PatientScan> GetOpPatientScans(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientScans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID
                                    //&& ltm.IsDelivered == true
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt,
                                        ltm
                                    })
                                  .OrderByDescending(b => b.pt.PSID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = enmrNo,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = DateFormat(x.pt.ScanDate),
                                     DoctorName = GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     IsBillPaid = x.ltm.IsBillPaid.HasValue ? x.ltm.IsBillPaid.Value : false
                                 }).ToList();

                return patientScans;
            }
        }

        public static bool ISPatientDischrged(string enmrNo)
        {
            using (var db = new HISDBEntities())
            {
                var flag = db.InPatients.Where(ip => ip.ENMRNO == enmrNo).FirstOrDefault().IsDischarged;
                return flag.HasValue ? flag.Value : false;
            }
        }

        public static decimal InsuranceScantionedAmount(string enmrNo)
        {
            using (var db = new HISDBEntities())
            {
                var flag = db.InPatients.Where(ip => ip.ENMRNO == enmrNo).FirstOrDefault().InsuranceRecievedAmt;
                return flag != null ? flag.Value : 0;
            }
        }

        public static decimal PharmaPackAmount(string enmrNo)
        {
            using (var db = new HISDBEntities())
            {
                var flag = db.InPatients.Where(ip => ip.ENMRNO == enmrNo).FirstOrDefault().PkitID;
                if (flag != null && flag >= 1)
                {
                    return db.PharmaKits.Where(pk => pk.PKitID == flag).FirstOrDefault().PKitCost.Value;
                }
                else return 0;
            }
        }

        public static RoomBilling GetRoomBilling(string enmrNo)
        {
            using (var db = new HISDBEntities())
            {
                var roomBilling = (from pra in db.PatientRoomAllocations
                                      join r in db.Rooms on pra.RoomNo equals r.RoomNo
                                      join b in db.Beds on pra.BedNo equals b.BedNo
                                      join rt in db.RoomTypes on r.RoomTypeID equals rt.RoomTypeID
                                      where pra.ENMRNO.Equals(enmrNo)
                                      select new { pra, r, rt, b }).AsEnumerable()
                                      .Select(x => new RoomBilling {
                                          ENMRNO = x.pra.ENMRNO,
                                          RoomName = string.Format("{0}-{1}", x.pra.RoomNo, x.rt.RoomType1),
                                          BedName = x.b.BedName,
                                          CostPerDay = x.r.CostPerDay,
                                          OccupiedDays = DateTime.Today > x.pra.FromDate ?  DateTime.Today.Subtract(x.pra.FromDate).Days : 1,
                                      }).FirstOrDefault();
                return roomBilling;
            }
        }

        public static bool IsOPPrescriptionFormEnable(string enmrNo)
        {
            bool result = true;
            bool hasPrescriptionForLastVisit = false;
            bool hasTestForLastVisit = false;
            bool hasScanForLastVisit = false;
            using (var hs = new HISDBEntities())
            {
                var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                if(latestVisit != null)
                {
                    hasPrescriptionForLastVisit = hs.PrescriptionMasters.Where(p => p.VisitID == latestVisit.SNO).Any();

                    hasTestForLastVisit = hs.LabTestMasters.Where(p => p.VisitID == latestVisit.SNO).Any();

                    hasScanForLastVisit = hs.ScanTestMasters.Where(p => p.VisitID == latestVisit.SNO).Any();

                    if (hasPrescriptionForLastVisit 
                        && hasTestForLastVisit 
                        && hasScanForLastVisit)
                    {
                        result = false;
                    }
                }
                else
                {
                    result = false;
                }
                
            }
            return result;
        }
       
        public static void CreateOrderRequest(PatientPrescription prescription)
        {
            string orderNo = "0";
            int omid = 0;
            using (var db = new HISDBEntities())
            {
                if (prescription != null && prescription.RequestQty > 0)
                {
                    var order = db.OrderMasters.OrderByDescending(o => o.OrderNO).FirstOrDefault();
                    if (order != null && order.Status == false)
                    {
                        omid = order.OMID;
                    }
                    else
                    {
                        orderNo = GeneratePONumber();
                        System.Data.Entity.Core.Objects.ObjectParameter omidOut = new System.Data.Entity.Core.Objects.ObjectParameter("OMID", typeof(Int32));
                        db.CreateMasterOrder(orderNo, omidOut);
                        omid = Convert.ToInt32(omidOut.Value);
                    }
                    var requestWithOrder = db.OrderRequests.Where(or => or.OMID == omid && or.MedicineID == prescription.MedicineID).FirstOrDefault();
                    if (requestWithOrder != null)
                    {
                        requestWithOrder.Quantity = requestWithOrder.Quantity + prescription.RequestQty;
                        db.Entry(requestWithOrder).State = EntityState.Modified;
                    }
                    else {
                        OrderRequest oReq = new OrderRequest();
                        oReq.OMID = omid;
                        oReq.MedicineID = prescription.MedicineID;
                        oReq.Quantity = prescription.RequestQty;
                        db.OrderRequests.Add(oReq);
                    }
                    db.SaveChanges();
                }     
            }
        }

        public static string GeneratePONumber()
        {
            string orderNo = "0";
            using (var db = new HISDBEntities())
            {
                var lastOrder = db.PurchaseOrders.OrderByDescending(o => o.OrderID).FirstOrDefault();
                if (lastOrder != null)
                {
                    orderNo = (Convert.ToInt32(lastOrder.PONumber) + 1).ToString().PadLeft(5, '0');
                }
                else
                {
                    orderNo = (Convert.ToInt32(orderNo) + 1).ToString().PadLeft(5, '0');
                }
            }
            return orderNo;
        }

        public static string LoginUserName()
        {
            using (var db = new HISDBEntities())
            {
                int loginUser = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                var user = db.Users.Where(u => u.UserID == loginUser).FirstOrDefault();
                if(user != null)
                {
                    return GetFullName(user.FirstName,user.MiddleName,user.LastName);
                }
            }
            return string.Empty;
        }
    }
}