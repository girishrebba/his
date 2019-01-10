using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using HIS.Action_Filters;
using System.ComponentModel;
using System.Globalization;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class OutPatientController : Controller
    {
        // GET: OutPatient
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOutPatients()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var outPatients = (from op in hs.OutPatients
                                  join user in hs.Users on op.DoctorID equals user.UserID where (op.Status == null || op.Status == false) 
                                  select new
                                  {
                                      op,
                                      user
                                  })
                                  .OrderByDescending(b => b.op.Enrolled)
                                  .AsEnumerable()
                                 .Select(x => new OutPatient
                                 {
                                     SNO = x.op.SNO,
                                     ENMRNO = x.op.ENMRNO,
                                     Name = x.op.GetFullName(),
                                     DOBDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.op.DOB),
                                     Address = HtmlHelpers.HtmlHelpers.GetFullAddress(
                                         x.op.Address1, 
                                         x.op.Address2,
                                         x.op.City,
                                         x.op.State,
                                         x.op.PinCode),
                                     GenderDisplay = HtmlHelpers.HtmlHelpers.GetGender(x.op.Gender),
                                     Phone = x.op.Phone,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(
                                         x.user.FirstName,
                                         x.user.MiddleName,
                                         x.user.LastName),
                                     Purpose = x.op.Purpose,
                                     EnrolledDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.op.Enrolled),
                                     PrevENMR = x.op.PrevENMR
                                 }).ToList();


                //Bind the Valid till date
                //if (outPatients != null && outPatients.Count() > 0)
                //{
                //    int validDays = 0;
                //    foreach (OutPatient op in outPatients)
                //    {
                //        var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == op.ENMRNO).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                //        if (latestVisit != null)
                //        {
                //            validDays = hs.ConsultationFees.Where(cf => cf.DoctorID == latestVisit.DoctorID && cf.ConsultTypeID == latestVisit.ConsultTypeID).First().ValidDays.Value;
                //            if (latestVisit.DateOfVisit.AddDays(validDays) > DateTime.Today)
                //            {
                //                op.ValidDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(latestVisit.DateOfVisit.AddDays(validDays));
                //            }
                //        }

                //    }
                //}
                // Get patients who has visits today.
                return Json(new { data = outPatients }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetErolledConsultations()
        {
            DateTime startDateTime = DateTime.Today; 
            DateTime endDateTime = DateTime.Today.AddDays(1).AddHours(12.5).AddTicks(-1);
            string today = DateTime.Today.AddHours(12.5).Date.ToString("dd/MM/yyyy");
            int sno = 0;
            using (HISDBEntities hs = new HISDBEntities())
            {
                var outPatients = (from op in hs.OutPatients
                                   join user in hs.Users on op.DoctorID equals user.UserID
                                   join pv in hs.PatientVisitHistories on op.ENMRNO equals pv.ENMRNO
                                   where (op.Status == null || op.Status == false) &&
                                   (pv.DateOfVisit >= startDateTime && pv.DateOfVisit <= endDateTime)
                                   select new
                                   {
                                       op,
                                       user,
                                       pv
                                   })
                                  .OrderBy(b => b.pv.DateOfVisit)
                                  .AsEnumerable()
                                 .Select(x => new OutPatient
                                 {
                                     RowNum = ++sno,
                                     SNO = x.op.SNO,
                                     ENMRNO = x.op.ENMRNO,
                                     Name = x.op.GetFullName(),
                                     DOBDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.op.DOB),
                                     Address = HtmlHelpers.HtmlHelpers.GetFullAddress(
                                         x.op.Address1,
                                         x.op.Address2,
                                         x.op.City,
                                         x.op.State,
                                         x.op.PinCode),
                                     GenderDisplay = HtmlHelpers.HtmlHelpers.GetGender(x.op.Gender),
                                     Phone = x.op.Phone,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(
                                         x.user.FirstName,
                                         x.user.MiddleName,
                                         x.user.LastName),
                                     Purpose = x.op.Purpose,
                                     EnrolledDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.op.Enrolled),
                                     VisitDate= x.pv.DateOfVisit.ToString("dd/MM/yyyy"),
                                     PrevENMR = x.op.PrevENMR
                                 }).ToList();
                outPatients = outPatients.Where(a => a.VisitDate == today).ToList();
                return Json(new { data = outPatients  }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewPatient(string enmrNo)
        {
            return View(GetPatientDetails(enmrNo));
        }

        [HttpGet]
        public ActionResult DeliverPrescription(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var prescriptions = new List<PatientPrescription>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                prescriptions = GetPatientNotDeliverPrescriptions(enmrNo, latestVisit.SNO);
                string visitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                if(prescriptions.Count() > 0)
                {
                    foreach(var pp in prescriptions)
                    {
                        var itemCost = hs.MedicineInventories.Where(mi => mi.MedicineID == pp.MedicineID).First().PricePerItem.Value;
                        pp.ItemCost = itemCost;
                        pp.TotalCost = pp.Quantity * itemCost;
                        pp.VisitName = visitName;
                        pp.DeliverQty = pp.Quantity;
                        pp.RequestQty = 0;
                    }
                }
                //ViewBag.History = PatientPrescriptionHistory(enmrNo);
            }
            return View(prescriptions); 
        }

        [HttpGet]
        public ActionResult DeliverPrescriptionPrint(string enmrNo, int visitid)
        {
            var latestVisit = new PatientVisitHistory();
            var prescriptions = new List<PatientPrescription>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo && pvh.SNO == visitid).FirstOrDefault();

                prescriptions = GetPatientPrintPrescriptions(enmrNo, latestVisit.SNO);
                string visitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                if (prescriptions.Count() > 0)
                {
                    foreach (var pp in prescriptions)
                    {
                        var itemCost = hs.MedicineInventories.Where(mi => mi.MedicineID == pp.MedicineID).First().PricePerItem.Value;
                        pp.ItemCost = itemCost;
                        pp.TotalCost = pp.Quantity * itemCost;
                        pp.VisitName = visitName;
                        //pp.DeliverQty = pp.Quantity;
                        //pp.RequestQty = 0;
                    }
                }
                //ViewBag.History = PatientPrescriptionHistory(enmrNo);
            }
            return View(prescriptions);
        }

        [HttpPost]
        public ActionResult DeliverPrescription(IList<PatientPrescription> prescriptions)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (prescriptions != null && prescriptions.Count() > 0)
                {
                    int PMID = prescriptions[0].PMID;
                    foreach (PatientPrescription pp in prescriptions)
                    {
                        var prescription = db.PatientPrescriptions.Where(p => p.PMID == pp.PMID && p.MedicineID == pp.MedicineID).FirstOrDefault();
                        if(prescription != null)
                        {
                            prescription.MedicineWithDose = "text";
                            prescription.DeliverQty = pp.DeliverQty;
                            prescription.RequestQty = pp.RequestQty;
                            db.Entry(prescription).State = EntityState.Modified;
                            HtmlHelpers.HtmlHelpers.CreateOrderRequest(prescription);
                        }
                        
                        db.SaveChanges();
                        var medInv = db.MedicineInventories.Where(miv => miv.MedicineID == pp.MedicineID).First();
                        medInv.AvailableQty = medInv.AvailableQty - pp.DeliverQty;
                        db.Entry(medInv).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var master = db.PrescriptionMasters.Where(p => p.PMID == PMID).FirstOrDefault();
                    master.Discount = prescriptions[0].Discount;
                    master.PaidAmount = prescriptions[0].PaidAmount;
                    master.TotalAmount = prescriptions[0].TotalAmount;
                    master.IsDelivered = true;
                    db.Entry(master).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true, message = string.Format("Prescription for ENMRNO - {0} delivered Successfully", prescriptions[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult ManualDrugRequest(string enmrNo)
        {
            ViewBag.Users = new SelectList(HtmlHelpers.HtmlHelpers.GetDoctors(), "UserID", "NameDisplay");
            ViewBag.Intakes = new SelectList(HtmlHelpers.HtmlHelpers.GetIntakes(), "FrequencyID", "Frequency");
            return View(new MDRModel
            {
                ENMRNO = enmrNo,
                MedicineID = 0,
                Quantity = 0,
                TotalCost = 0,
                BatchNo = string.Empty,
                LotNo = string.Empty,
                IntakeFrequencyID = 0
            });
        }

        [HttpPost]
        public ActionResult ManualDrugRequest(List<PatientPrescription> mdrRequest)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (mdrRequest != null && mdrRequest.Count() > 0)
                {
                    System.Data.Entity.Core.Objects.ObjectParameter pmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("PMID", typeof(Int32));
                    
                    db.CreateMasterPrescription(mdrRequest[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]),0,false, pmidOut);
                    int pmid = Convert.ToInt32(pmidOut.Value);
                    foreach (PatientPrescription pp in mdrRequest)
                    {
                        pp.PMID = pmid;
                        pp.MedicineWithDose = "text";
                        pp.IntakeFrequencyID = pp.IntakeFrequencyID;
                        db.PatientPrescriptions.Add(pp);
                        var medInv = db.MedicineInventories.Where(miv => miv.MedicineID == pp.MedicineID).First();
                        medInv.AvailableQty = medInv.AvailableQty - pp.DeliverQty;
                        db.Entry(medInv).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    var master = db.PrescriptionMasters.Where(p => p.PMID == pmid).FirstOrDefault();
                    master.Discount = mdrRequest[0].Discount;
                    master.PaidAmount = mdrRequest[0].PaidAmount;
                    master.TotalAmount = mdrRequest[0].TotalAmount;
                    master.IsDelivered = true;
                    db.Entry(master).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Manual Drug Request for ENMRNO - {0} delivered Successfully", mdrRequest[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        [HttpGet]
        public ActionResult AddModify(string enmrNo = null)
        {
            List<BloodGroup> BloodGroups = HtmlHelpers.HtmlHelpers.GetBloodGroups();
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            if (enmrNo == null)
            {
                ViewBag.BloodGroups = new SelectList(BloodGroups, "GroupID", "GroupName");
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                OutPatient newPatient = new OutPatient();
                newPatient.ENMRNO = HtmlHelpers.HtmlHelpers.GetSequencedEnmrNo();
                newPatient.Purposes = HtmlHelpers.HtmlHelpers.GetPurposes();
                newPatient.PurposeIds = null;
                return View(newPatient);
            }
            else
            {
                var patient = GetPatientDetails(enmrNo);
                if (patient != null)
                {
                    ViewBag.BloodGroups = new SelectList(BloodGroups, "GroupID", "GroupName", patient.BloodGroupID);
                    ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay", patient.DoctorID);
                    
                    return View(patient);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(OutPatient op)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (op.SNO == 0)
                {
                    ConstructPurpose(op);
                    db.OutPatients.Add(op);
                    db.SaveChanges();
                    CreateVisit(op);
                    HtmlHelpers.HtmlHelpers.CreatePatientDirectory("OutPatient", op.ENMRNO);
                    return Json(new { success = true, message = string.Format("ENMR - {0} created Successfully", op.ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ConstructPurpose(op);
                    db.Entry(op).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("ENMR - {0} updated Successfully", op.ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private static void ConstructPurpose(OutPatient op)
        {
            if (op != null && op.PurposeIds != null)
            {
                op.Purpose = string.Join(",", op.PurposeIds);
            }
            else
            {
                op.Purpose = string.Empty;
            }
        }

        private void CreateVisit(OutPatient op)
        {
            decimal fee = 0;

            //Time zone convertion
           // DateTime dateNow = DateTime.Now;
            //Console.WriteLine("The date and time are {0} UTC.",TimeZoneInfo.ConvertTimeToUtc(dateNow));
           // DateTime utcdate = DateTime.ParseExact(DateTime.Now.ToString(), "M/dd/yyyy h: mm:ss tt", CultureInfo.InvariantCulture);
           // var istdate = TimeZoneInfo.ConvertTimeFromUtc(dateNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            using (var db = new HISDBEntities())
            {
                var consultFee = db.ConsultationFees.Where(cf => cf.ConsultTypeID == 1 && cf.DoctorID == op.DoctorID).FirstOrDefault();
                if(consultFee != null)
                {
                    fee = consultFee.Fee.HasValue ? consultFee.Fee.Value : 0;
                }
                var pvh = new PatientVisitHistory
                {
                    DoctorID = op.DoctorID,
                    ENMRNO = op.ENMRNO,
                    ConsultTypeID = 1,
                    Fee = fee,
                    DateOfVisit = DateTime.Now.AddHours(12.5),
                    Weight = op.Weight,
                    Temperature = op.Temperature,
                    BP = op.BP,
                    Purpose = op.Purpose,
                    BMI = op.BMI
                };

                db.PatientVisitHistories.Add(pvh);
                db.SaveChanges();
            }
        }

        public OutPatient GetPatientDetails(string enmrNo)
        {
            OutPatient outPatient = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var outpatient = (from op in dc.OutPatients
                                 join user in dc.Users on op.DoctorID equals user.UserID
                                 join bg in dc.BloodGroups on op.BloodGroupID equals bg.GroupID
                                 where op.ENMRNO.Equals(enmrNo)
                                 select new { op, user, bg.GroupName }).FirstOrDefault();
                if (outpatient != null)
                {
                    outPatient = outpatient.op;
                    outPatient.DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(
                                         outpatient.user.FirstName,
                                         outpatient.user.MiddleName,
                                         outpatient.user.LastName);
                    outPatient.BloodGroupDisplay = outpatient.GroupName;
                    outPatient.Name = HtmlHelpers.HtmlHelpers.GetFullName(
                                         outpatient.op.FirstName,
                                         outpatient.op.MiddleName,
                                         outpatient.op.LastName);
                    outPatient.Address = HtmlHelpers.HtmlHelpers.GetFullAddress(
                                         outpatient.op.Address1,
                                         outpatient.op.Address2,
                                         outpatient.op.City,
                                         outpatient.op.State,
                                         outpatient.op.PinCode);
                    outPatient.GenderDisplay = HtmlHelpers.HtmlHelpers.GetGender(outpatient.op.Gender);
                    outPatient.MaritalStatusDisplay = HtmlHelpers.HtmlHelpers.GetMaritalStatus(outpatient.op.MaritalStatus);
                    outPatient.DOBDisplay = HtmlHelpers.HtmlHelpers.DateFormat(outpatient.op.DOB);
                    outPatient.EnrolledDisplay = HtmlHelpers.HtmlHelpers.DateFormat(outpatient.op.Enrolled);
                    outPatient.Purposes = HtmlHelpers.HtmlHelpers.GetPurposes();
                    outPatient.PurposeIds = !string.IsNullOrEmpty(outpatient.op.Purpose) ? outpatient.op.Purpose.Split(',') : null;
                    outPatient.Purpose = HtmlHelpers.HtmlHelpers.GetPurpose(outpatient.op.Purpose, outPatient.Purposes);
                }
                return outPatient;
            }
        }

        public JsonResult GetPatientVisits(string enmrNo)
        {
            return Json(new { data = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NewVisit(PatientVisitHistory pvh)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                if (pvh != null && pvh.PurposeIds != null)
                {
                    pvh.Purpose = string.Join(",", pvh.PurposeIds);
                }               
                pvh.DateOfVisit = pvh.DateOfVisit.AddHours(12.5);
                hs.PatientVisitHistories.Add(pvh);
                hs.SaveChanges();
                return Json(new { success = true, message = string.Format("Consultation created Successfully for ENMRNO: {0}", pvh.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetConsultationFee(int doctorId, int consultTypeId )
        {
            List<ConsultationFee> consultationFees = HtmlHelpers.HtmlHelpers.GetConsultationFees();
            if (doctorId > 0 && consultTypeId > 0)
            {
                var fee = consultationFees.Where(bc => bc.DoctorID == doctorId && bc.ConsultTypeID == consultTypeId).FirstOrDefault();
                return Json(fee != null ? fee.Fee : null, JsonRequestBehavior.AllowGet);
            }
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NewVisit(string enmrNo)
        {
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<ConsultationType> Consultations = HtmlHelpers.HtmlHelpers.GetConsultationTypes();
            ViewBag.Consultations = new SelectList(Consultations, "ConsultTypeID", "ConsultType");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            PatientVisitHistory pvh = new PatientVisitHistory();
            pvh.ENMRNO = enmrNo;
            return View(pvh);
        }

        public List<PatientPrescription> GetPatientVisitPrescriptions(string enmrNo, int visitID)
        {
            return PatientVisitPrescriptions(enmrNo, visitID);
        }

        public List<PatientPrescription> GetPatientNotDeliverPrescriptions(string enmrNo, int visitID)
        {
            return PatientVisitNotDeliverPrescriptions(enmrNo, visitID);
        }

        public List<PatientPrescription> GetPatientPrintPrescriptions(string enmrNo, int visitID)
        {
            return PatientVisitPrintPrescriptions(enmrNo, visitID);
        }

        private static List<PatientPrescription> PatientVisitPrescriptions(string enmrNo, int visitID)
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
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID
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
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID,
                                     IsDelivered = x.pm.IsDelivered.HasValue ? x.pm.IsDelivered.Value : false,
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        private static List<PatientPrescription> PatientVisitNotDeliverPrescriptions(string enmrNo, int visitID)
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
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID && pm.IsDelivered == false 
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
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        private static List<PatientPrescription> PatientVisitPrintPrescriptions(string enmrNo, int visitID)
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
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID
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
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pm.DatePrescribed),
                                     ENMRNO = x.pm.ENMRNO,
                                     VisitID = x.pm.VisitID.Value,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID,
                                     PMID = x.pm.PMID,
                                     PaidAmount =x.pm.PaidAmount.HasValue ? x.pm.PaidAmount.Value : 0,
                                     Discount= x.pm.Discount.HasValue ? x.pm.Discount.Value :0,
                                     TotalAmount=x.pm.TotalAmount.HasValue ? x.pm.TotalAmount.Value : 0
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        public List<PatientTest> GetPatientVisitTests(string enmrNo, int visitID)
        {
            return PatientVisitTests(enmrNo, visitID);
        }

        public List<PatientTest> GetPatientNotDeliverVisitTests(string enmrNo, int visitID)
        {
            return PatientNotDeliverVisitTests(enmrNo, visitID);
        }

        public List<PatientScan> GetPatientNotDeliverVisitScans(string enmrNo, int visitID)
        {
            return PatientNotDeliverVisitScans(enmrNo, visitID);
        }

        private static List<PatientTest> PatientVisitTests(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
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
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     TestName = x.tt.TestName,
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pt.TestDate),
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     LTMID = x.pt.LTMID,
                                     TestID = x.pt.TestID,
                                     IsBillPaid = x.ltm.IsBillPaid.HasValue ? x.ltm.IsBillPaid.Value : false
                                 }).ToList();

                return patientTests;
            }
        }

        private static List<PatientTest> PatientNotDeliverVisitTests(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID && ltm.IsDelivered == false
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt
                                    })
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     TestName = x.tt.TestName,
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pt.TestDate),
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     LTMID = x.pt.LTMID,
                                     TestID = x.pt.TestID
                                 }).ToList();

                return patientTests;
            }
        }

        private static List<PatientScan> PatientNotDeliverVisitScans(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.Scans on pt.ScanID equals tt.ScanID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID && ltm.IsDelivered == false
                                    select new
                                    {
                                        pt,
                                        u,
                                        tt
                                    })
                                  .OrderByDescending(b => b.pt.ScanID)
                                  .AsEnumerable()
                                 .Select(x => new PatientScan
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     ScanName = x.tt.ScanName,
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pt.ScanDate),
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     STMID = x.pt.STMID,
                                     ScanID = x.pt.ScanID
                                 }).ToList();

                return patientTests;
            }
        }

        public List<PatientTest> GetPatientVisitTestsBillPay(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
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
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     TestName = x.tt.TestName,
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.ltm.DatePrescribed),
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     LTMID = x.pt.LTMID,
                                     TestID = x.pt.TestID,
                                     TestCost = x.tt.TestCost.HasValue? x.tt.TestCost.Value : 0
                                 }).ToList();

                return patientTests;
            }
        }

        public List<PatientTest> GetPatientVisitTestsBillPayPrint(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
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
                                  .OrderByDescending(b => b.pt.PTID)
                                  .AsEnumerable()
                                 .Select(x => new PatientTest
                                 {
                                     ENMRNO = x.pt.ENMRNO,
                                     TestName = x.tt.TestName,
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.ltm.DatePrescribed),
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     RecordedValues = x.pt.RecordedValues,
                                     TestImpression = x.pt.TestImpression,
                                     ReportPath = x.pt.ReportPath,
                                     LTMID = x.pt.LTMID,
                                     TestID = x.pt.TestID,
                                     TestCost = x.tt.TestCost.HasValue ? x.tt.TestCost.Value : 0,
                                     TotalAmount = x.ltm.TotalAmount.HasValue ? x.ltm.TotalAmount.Value : 0,
                                     Discount = x.ltm.Discount.HasValue ? x.ltm.Discount.Value : 0,
                                     PaidAmount = x.ltm.PaidAmount.HasValue ? x.ltm.PaidAmount.Value : 0

                                 }).ToList();

                return patientTests;
            }
        }


        private List<PatientPrescriptionHistory> PatientPrescriptionHistory(string enmrNo)
        {
            var prescriptionsHistory = new List<PatientPrescriptionHistory>();
            using (var db = new HISDBEntities())
            {
                var prescribedVisits = (from pv in db.PatientVisitHistories
                                        //join pm in db.PrescriptionMasters on pv.SNO equals pm.VisitID
                                        //join pp in db.PatientPrescriptions on pm.PMID equals pp.PMID
                                        
                                        where pv.ENMRNO == enmrNo select pv).ToList();
                foreach (var pre in prescribedVisits)
                {
                    List<PatientPrescription> prescriptions = GetPatientVisitPrescriptions(pre.ENMRNO, pre.SNO);
                    var visitPrescription = new PatientPrescriptionHistory();
                    visitPrescription.VisitName = VisitNameWithDate(pre);
                    visitPrescription.SNO = pre.SNO;
                    visitPrescription.Prescriptions = prescriptions;
                    visitPrescription.PatientTests = GetPatientVisitTests(pre.ENMRNO, pre.SNO);
                    visitPrescription.PatientScans = HtmlHelpers.HtmlHelpers.GetOpPatientScans(enmrNo,pre.SNO);
                    prescriptionsHistory.Add(visitPrescription);
                }
                return prescriptionsHistory;
            }
        }

        private string VisitNameWithDate(PatientVisitHistory visit)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == visit.ConsultTypeID).FirstOrDefault().ConsultType;

                return string.Format("{0} : {1}", consultName, HtmlHelpers.HtmlHelpers.DateFormat(visit.DateOfVisit));
            }
        }
        //private string VisitName(int consultTypeID)
        //{
        //    using (HISDBEntities db = new HISDBEntities())
        //    {
        //        var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == consultTypeID).FirstOrDefault().ConsultType;

        //        return consultName;
        //    }
        //}

        [HttpGet]
        public ActionResult OPPrescription(string enmrNo)
        {
            string currentVisit = string.Empty;
           // bool isLatestVisitPrescribed = false;
            int visitID = 1;
            using (HISDBEntities hs = new HISDBEntities())
            {
                var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                if (latestVisit != null)
                {
                    visitID = latestVisit.SNO;
                    currentVisit = VisitNameWithDate(latestVisit);
                }
            }
            List<PatientPrescriptionHistory> patientVisitHistory = PatientPrescriptionHistory(enmrNo);
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<IntakeFrequency> Intakes = HtmlHelpers.HtmlHelpers.GetIntakes();
            ViewBag.Intakes = new SelectList(Intakes, "FrequencyID", "Frequency");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            ViewBag.History = patientVisitHistory;
            ViewBag.MDR = GetPatientVisitPrescriptions(enmrNo, 0).Where(v => v.VisitID == 0 && v.ISIP == false).ToList();
            ViewBag.IsNewVisit = patientVisitHistory.Count() <= 0 ? true : false;
            PatientPrescription pp = new PatientPrescription();
            pp.ENMRNO = enmrNo;
            pp.VisitID = visitID;
            pp.VisitName = currentVisit;
            pp.DoctorName = HtmlHelpers.HtmlHelpers.LoginUserName();
            pp.TestTypes = HtmlHelpers.HtmlHelpers.GetTestTypes();
            pp.Scans = HtmlHelpers.HtmlHelpers.GetScans();
            return View(pp);
        }

        [HttpPost]
        public ActionResult OPPrescription(IList<PatientPrescription> prescriptions)
        {
            int pmid = 0;
            int stmid = 0;
            using (HISDBEntities db = new HISDBEntities())
            {
                int lastVisitID = 0;
                if (prescriptions != null && prescriptions.Count() > 0)
                {
                    var suggestedTestsIfAny = prescriptions[0];
                    if (prescriptions[0].HasPrescription)
                    {
                        lastVisitID = prescriptions[0].VisitID;
                        var pMaster = db.PrescriptionMasters.Where(pm => pm.VisitID == lastVisitID && pm.IsDelivered == false).FirstOrDefault();

                        if (pMaster != null)
                        {
                            pmid = pMaster.PMID;
                        }
                        else
                        {
                            System.Data.Entity.Core.Objects.ObjectParameter pmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("PMID", typeof(Int32));

                            db.CreateMasterPrescription(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, false, pmidOut);
                            pmid = Convert.ToInt32(pmidOut.Value);
                        }

                        foreach (PatientPrescription pp in prescriptions)
                        {
                            pp.PMID = pmid;
                            db.PatientPrescriptions.Add(pp);
                        }
                        db.SaveChanges();
                    }
                    //Save Tests
                    if (suggestedTestsIfAny.TestIds != null)
                    {
                        SaveLabTestOrPackage(
                            prescriptions,
                            db, 
                            suggestedTestsIfAny.VisitID, suggestedTestsIfAny.TestIds);
                    }
                    //Save Packages
                    if (suggestedTestsIfAny.KitIds != null)
                    {
                        SaveLabTestOrPackage(
                            prescriptions,
                            db,
                            suggestedTestsIfAny.VisitID, suggestedTestsIfAny.KitIds);
                    }

                    //Save Scans
                    if (suggestedTestsIfAny.ScanIds != null)
                    {
                        int visitID = prescriptions[0].VisitID;
                        string emrno = prescriptions[0].ENMRNO;
                        var sMaster = db.ScanTestMasters.Where(pm => pm.ENMRNO == emrno && pm.VisitID == visitID && pm.IsDelivered == false).FirstOrDefault();
                        if (sMaster != null)
                        {
                            stmid = sMaster.STMID;
                        }
                        else
                        {
                            System.Data.Entity.Core.Objects.ObjectParameter ltmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("STMID", typeof(Int32));
                            db.CreateMasterScanTest(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, false, ltmidOut);
                            stmid = Convert.ToInt32(ltmidOut.Value);
                        }
                        foreach (var id in suggestedTestsIfAny.ScanIds)
                        {
                            var patientscan = new PatientScan
                            {
                                ScanID = Convert.ToInt32(id),
                                STMID = stmid
                            };
                            db.PatientScans.Add(patientscan);
                        }
                    }

                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Prescription for ENMRNO - {0} created Successfully", prescriptions[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }

        }

        private static void SaveLabTestOrPackage(IList<PatientPrescription> prescriptions, HISDBEntities db, int visitId, string[] tests)
        {
            int ltmid = 0;
            var lMaster = db.LabTestMasters.Where(pm => pm.VisitID == visitId && pm.IsDelivered == false).FirstOrDefault();
            if (lMaster != null)
            {
                ltmid = lMaster.LTMID;
            }
            else
            {
                System.Data.Entity.Core.Objects.ObjectParameter ltmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("LTMID", typeof(Int32));
                db.CreateMasterLabTest(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, false, ltmidOut);
                ltmid = Convert.ToInt32(ltmidOut.Value);
            }
            foreach (var id in tests)
            {
                var patientTest = new PatientTest
                {
                    TestID = Convert.ToInt32(id),
                    LTMID = ltmid
                };
                db.PatientTests.Add(patientTest);
            }
        }

        [HttpPost]
        public JsonResult GetMedicinesWithQuantity(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from bc in hs.BrandCategories
                                 join mm in hs.MedicineMasters on bc.CategoryID equals mm.BrandCategoryID
                                 join mi in hs.MedicineInventories on mm.MMID equals mi.MedicineID
                                 join bsc in hs.BrandSubCategories on mm.SubCategoryID equals bsc.SubCategoryID 
                                 //into subcat
                                 //from bsc in subcat.DefaultIfEmpty()
                                 where 
                                 (mm.MedicineName.Contains(Prefix)
                                 || bc.Category.Contains(Prefix)
                                 || bsc.SubCategory.Contains(Prefix)
                                 || mm.MedDose.Contains(Prefix))
                                 select new { bc, mm, mi,bsc.SubCategory }).AsEnumerable()
                                 .Select(m => new MedicineMaster
                                 {
                                     MMID = m.mm.MMID,
                                     MedicineDisplay = HtmlHelpers.HtmlHelpers.GetMedicineCategoryWithDoseAvailableQty(m.SubCategory,m.bc.Category, m.mm.MedicineName, m.mm.MedDose, m.mi.AvailableQty.HasValue?m.mi.AvailableQty.Value:0),
                                     ItemPrice = m.mi.PricePerItem,
                                     SelectDisplay = m.mm.MedicineName,//string.Format("{0} - {1}",m.mm.MedicineName, m.mm.MedDose)
                                 }).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PatientTests(string enmrNo)
        {
            ViewBag.ENMRNO = enmrNo;
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                patientTests = GetPatientNotDeliverVisitTests(enmrNo, latestVisit.SNO);
               // string visitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                    
                }
            }
            return View(patientTests);
        }

        [HttpPost]
        public ActionResult PatientTests(List<PatientTest> ptItems)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ptItems != null && ptItems.Count() > 0)
                {
                    int ltmid = ptItems[0].LTMID;
                    var masterTest = db.LabTestMasters.Where(ltm => ltm.LTMID == ltmid).FirstOrDefault();
                    masterTest.IsDelivered = true;
                    db.Entry(masterTest).State = EntityState.Modified;
                    foreach (PatientTest pt in ptItems)
                    {
                        var test = db.PatientTests.Where(p => p.LTMID == pt.LTMID && p.TestID == pt.TestID).FirstOrDefault();
                        if (test != null)
                        {
                            test.RecordedValues = pt.RecordedValues;
                            test.TestDate = pt.TestDate;
                            test.TestImpression = pt.TestImpression;
                            db.Entry(test).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Lab Tests for ENMRNO - {0} updated Successfully", ptItems[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult PatientScans(string enmrNo)
        {
            ViewBag.ENMRNO = enmrNo;
            var latestVisit = new PatientVisitHistory();
            var PatientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                PatientScans = GetPatientNotDeliverVisitScans(enmrNo, latestVisit.SNO);
                if (PatientScans.Count() > 0)
                {
                    PatientScans[0].ENMRNO = enmrNo;
                    PatientScans[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                }
            }
            return View(PatientScans);
        }

        [HttpPost]
        public ActionResult PatientScans(List<PatientScan> ptItems)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ptItems != null && ptItems.Count() > 0)
                {
                    int ltmid = ptItems[0].STMID;
                    var masterTest = db.ScanTestMasters.Where(ltm => ltm.STMID == ltmid).FirstOrDefault();
                    masterTest.IsDelivered = true;
                    db.Entry(masterTest).State = EntityState.Modified;
                    foreach (PatientScan pt in ptItems)
                    {
                        var test = db.PatientScans.Where(p => p.STMID == pt.STMID && p.ScanID == pt.ScanID).FirstOrDefault();
                        if (test != null)
                        {
                            test.RecordedValues = pt.RecordedValues;
                            test.ScanDate = pt.ScanDate;
                            test.TestImpression = pt.TestImpression;
                            db.Entry(test).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Scan Tests for ENMRNO - {0} updated Successfully", ptItems[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpGet]
        [Description(" - OutPatient Lab Test Bill Payment Form.")]
        public ActionResult LabTestBillPay(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                patientTests = GetPatientVisitTestsBillPay(enmrNo, latestVisit.SNO);
                //string visitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);

                }
            }
            return View(patientTests);
        }
        
        public ActionResult LabTestBillPayPrint(string enmrNo, int visitid)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo && pvh.SNO == visitid).FirstOrDefault();

                patientTests = GetPatientVisitTestsBillPayPrint(enmrNo, latestVisit.SNO);
                //string visitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);

                }
            }
            return View(patientTests);
        }

        [HttpPost]
        [Description(" - OutPatient Lab Test Bill Payment Form.")]
        public ActionResult LabTestBillPay(MasterBillPayModel masterLab)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (masterLab != null)
                {

                    var testMaster = db.LabTestMasters.Where(p => p.LTMID == masterLab.ID).FirstOrDefault(); ;
                    if (testMaster != null)
                    {
                        testMaster.Discount = masterLab.Discount;
                        testMaster.PaidAmount = masterLab.PaidAmount;
                        testMaster.TotalAmount = masterLab.TotalAmount;
                        testMaster.IsBillPaid = true;
                        db.Entry(testMaster).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }

                return Json(new { success = true, message = string.Format("Lab Tests for ENMRNO - {0} paid Successfully", masterLab.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetTestName(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from tt in hs.TestTypes
                                 where tt.TestName.StartsWith(Prefix)
                                 select new { tt }).AsEnumerable()
                                 .Select(t => new TestType
                                 {
                                     TestID = t.tt.TestID,
                                     TestName = t.tt.TestName
                                 }).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - OutPatient Scans Bill Payment Form.")]
        public ActionResult opScanTestBillPay(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                patientScans = HtmlHelpers.HtmlHelpers.GetPatientVisitScansBillPay(enmrNo, latestVisit.SNO);
                //string visitName = VisitName(latestVisit.ConsultTypeID);    
                if (patientScans.Count() > 0)
                {
                    patientScans[0].ENMRNO = enmrNo;
                    patientScans[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);

                }
            }
            return View(patientScans);
        }

        public ActionResult opScanTestBillPayprint(string enmrNo,int visitid)
        {
            var latestVisit = new PatientVisitHistory();
            var patientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo && pvh.SNO == visitid).FirstOrDefault();

                patientScans = HtmlHelpers.HtmlHelpers.GetPatientVisitScansBillPayPrint(enmrNo, latestVisit.SNO);
                //string visitName = VisitName(latestVisit.ConsultTypeID);
                if (patientScans.Count() > 0)
                {
                    patientScans[0].ENMRNO = enmrNo;
                    patientScans[0].VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(latestVisit.ConsultTypeID);

                }
            }
            return View(patientScans);
        }

        [HttpPost]
        [Description(" - OutPatient Lab Test Bill Payment Form.")]
        public ActionResult opScanTestBillPay(MasterBillPayModel masterLab)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (masterLab != null)
                {

                    var testMaster = db.ScanTestMasters.Where(p => p.STMID == masterLab.ID).FirstOrDefault(); ;
                    if (testMaster != null)
                    {
                        testMaster.Discount = masterLab.Discount;
                        testMaster.PaidAmount = masterLab.PaidAmount;
                        testMaster.TotalAmount = masterLab.TotalAmount;
                        testMaster.IsBillPaid = true;
                        db.Entry(testMaster).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }

                return Json(new { success = true, message = string.Format("Lab Tests for ENMRNO - {0} paid Successfully", masterLab.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Admission.")]
        public ActionResult Admission(string enmrNo)
        {
            return View(new AdmissionModel() { ENMRNO = enmrNo });
        }

        [HttpGet]
        [Description(" - Refunds.")]
        public ActionResult Refunds(string enmrNo, int visitID, int refundID)
        {
            var refunds = new RefundViewModel();
            // Consultation
            if (refundID == 1)
            {
                refunds = GetConsultationBillingHistory(enmrNo).Where(v=>v.VisitID == visitID).FirstOrDefault();
            }
            else if (refundID == 2) // Prescription
            {
                refunds = GetPrescriptionBillingHistory(enmrNo).Where(v => v.VisitID == visitID).FirstOrDefault();
            }
            else if (refundID == 3) // Lab Test
            {
                refunds = GetLabTestBillingHistory(enmrNo).Where(v => v.VisitID == visitID).FirstOrDefault();

            }
            else if (refundID == 4) // Scan
            {
                refunds = GetScanBillingHistory(enmrNo).Where(v => v.VisitID == visitID).FirstOrDefault();

            }
            return View(refunds);
        }

        [HttpPost]
        [Description(" - Refunds.")]
        public ActionResult Refunds(RefundViewModel rvm)
        {
            if (rvm != null)
            {
                using (HISDBEntities hs = new HISDBEntities())
                {
                    if (rvm.RefundID == 1) // Consultation
                    {
                        var pvm = hs.PatientVisitHistories.Where(p => p.SNO == rvm.VisitID && p.ENMRNO == rvm.ENMRNO).FirstOrDefault();
                        pvm.RefundAmount = rvm.PrevRefAmount + rvm.RefundAmount;
                        pvm.RefundReason = rvm.RefundReason;
                        hs.Entry(pvm).State = EntityState.Modified;
                    }
                    else if (rvm.RefundID == 2) // Prescription
                    {
                        var pvm = hs.PrescriptionMasters.Where(p => p.VisitID == rvm.VisitID && p.ENMRNO == rvm.ENMRNO).FirstOrDefault();
                        pvm.RefundAmount = rvm.PrevRefAmount + rvm.RefundAmount;
                        pvm.RefundReason = rvm.RefundReason;
                        hs.Entry(pvm).State = EntityState.Modified;
                    }
                    else if (rvm.RefundID == 3) // Lab Test
                    {
                        var pvm = hs.LabTestMasters.Where(p => p.VisitID == rvm.VisitID && p.ENMRNO == rvm.ENMRNO).FirstOrDefault();
                        pvm.RefundAmount = rvm.PrevRefAmount + rvm.RefundAmount;
                        pvm.RefundReason = rvm.RefundReason;
                        hs.Entry(pvm).State = EntityState.Modified;
                    }
                    else if (rvm.RefundID == 4) // Scan
                    {
                        var pvm = hs.ScanTestMasters.Where(p => p.VisitID == rvm.VisitID && p.ENMRNO == rvm.ENMRNO).FirstOrDefault();
                        pvm.RefundAmount = rvm.PrevRefAmount + rvm.RefundAmount;
                        pvm.RefundReason = rvm.RefundReason;
                        hs.Entry(pvm).State = EntityState.Modified;
                    }
                    hs.SaveChanges();
                }
                return Json(new { success = true, message = string.Format("Patient ENMRNO: {0} bill refunded Successfully", rvm.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = string.Format("Patient ENMRNO: {0} bill not refunded due to some error.", rvm.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Bill History.")]
        public ActionResult BillHistory(string enmrNo)
        {
            return View(new RefundViewModel() { ENMRNO = enmrNo });
        }

        [HttpPost]
        [Description(" - Admission.")]
        public ActionResult Admission(AdmissionModel model)
        {
            if(model != null)
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    db.ConvertOutPatientToInPatient(model.ENMRNO, model.EstAmount, model.AdvAmount);

                    return Json(new { success = true, message = string.Format("Patient ENMRNO: {0} admitted Successfully", model.ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = true, message = string.Format("Error Occured while admitting the ENMRNO: {0} admitted Successfully", model.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UploadFiles()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;

            string emrno = Request.Form.AllKeys[0];
            int test = Convert.ToInt32(Request.Form.AllKeys[1]);

            HttpPostedFileBase file = files[0];
            string fname;

            // Checking for Internet Explorer    
            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            {
                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                fname = testfiles[testfiles.Length - 1];
            }
            else
            {
                fname = file.FileName;
                FileName = file.FileName;
            }

            string subPath = "~/PatientRecords/" + emrno + "/"; // your code goes here

            bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

            // Get the complete folder path and store the file inside it.
            string dbfname = Path.Combine(subPath.Replace("~", ".."), fname);
            fname = Path.Combine(Server.MapPath(subPath), fname);            
            file.SaveAs(fname);
            
            using (HISDBEntities db = new HISDBEntities())
            {
                int emr = db.LabTestMasters.Where(a => a.ENMRNO == emrno).OrderByDescending(a=>a.LTMID).Select(b => b.LTMID).FirstOrDefault();

                db.Database.ExecuteSqlCommand("update PatientTests set ReportPath='" + dbfname + "' where LTMID = '" + emr + "' and TestID=" + test);
                db.SaveChanges();
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadScanFiles()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;

            string emrno = Request.Form.AllKeys[0];
            int test = Convert.ToInt32(Request.Form.AllKeys[1]);

            HttpPostedFileBase file = files[0];
            string fname;

            // Checking for Internet Explorer    
            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            {
                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                fname = testfiles[testfiles.Length - 1];
            }
            else
            {
                fname = file.FileName;
                FileName = file.FileName;
            }

            string subPath = "~/PatientRecords/" + emrno + "/"; // your code goes here

            bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

            if (!exists)
                System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

            // Get the complete folder path and store the file inside it.
            string dbfname = Path.Combine(subPath.Replace("~", ".."), fname);
            fname = Path.Combine(Server.MapPath(subPath), fname);
            file.SaveAs(fname);

            using (HISDBEntities db = new HISDBEntities())
            {
                int emr = db.ScanTestMasters.Where(a => a.ENMRNO == emrno).OrderByDescending(a => a.STMID).Select(b => b.STMID).FirstOrDefault();

                db.Database.ExecuteSqlCommand("update PatientScans set ReportPath='" + dbfname + "' where STMID = '" + emr + "' and ScanID=" + test);
                db.SaveChanges();
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintHistory(string enmrNo)
        {
            ViewBag.Prescriptions = HtmlHelpers.HtmlHelpers.GetPatientPrescriptions(enmrNo);
            ViewBag.Tests = HtmlHelpers.HtmlHelpers.GetPatientTests(enmrNo);
            ViewBag.Scans = HtmlHelpers.HtmlHelpers.GetPatientScans(enmrNo);
            ViewBag.Visits = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo);
            return View(GetPatientDetails(enmrNo));
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ActionResult PrintPaymentHistory(string enmrNo, string visitid)
        {
            // List<PatientPrescriptionHistory> patientVisitHistory = new OutPatientController.PatientPrescriptionHistory(enmrNo);

            ViewBag.Prescriptions = HtmlHelpers.HtmlHelpers.GetPatientPrescriptionbyVisit(enmrNo,Convert.ToInt32(visitid));
           // ViewBag.InPateintPrescriptions = HtmlHelpers.HtmlHelpers.InPatientPrescriptions(enmrNo);
            ViewBag.Tests = HtmlHelpers.HtmlHelpers.GetPatientTestbyVisit(enmrNo,Convert.ToInt32(visitid));
            ViewBag.Scans = HtmlHelpers.HtmlHelpers.GetPatientScanbyVisit(enmrNo,Convert.ToInt32(visitid));
            //ViewBag.InpatientTests = HtmlHelpers.HtmlHelpers.GetInPatientTests(enmrNo);
            //ViewBag.InpatientScans = HtmlHelpers.HtmlHelpers.GetInPatientScans(enmrNo);
            ViewBag.Visits = HtmlHelpers.HtmlHelpers.GetOutPatientVisit(enmrNo, Convert.ToInt32(visitid));
            return View(GetPatientDetails(enmrNo));
        }

        public JsonResult GetBillings(int refundID, string enmrNo)
        {
            //int refundID = 0;
            var refunds = new List<RefundViewModel>();
            if (refundID == 0) refundID = 1;
            using (HISDBEntities hs = new HISDBEntities())
            {
                // Consultation
                if (refundID == 1)
                {
                    refunds = GetConsultationBillingHistory(enmrNo);
                }
                else if (refundID == 2) // Prescription
                {
                    refunds = GetPrescriptionBillingHistory(enmrNo);
                }
                else if (refundID == 3) // Lab Test
                {
                    refunds = GetLabTestBillingHistory(enmrNo);

                }
                else if (refundID == 4) // Scan
                {
                    refunds = GetScanBillingHistory(enmrNo);

                }
            }
            return Json(new { data = refunds }, JsonRequestBehavior.AllowGet);
        }

        private static List<RefundViewModel> GetScanBillingHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                return (from pv in hs.PatientVisitHistories
                    join sm in hs.ScanTestMasters on pv.SNO equals sm.VisitID
                    where pv.ENMRNO == enmrNo && sm.IsBillPaid == true
                    select new { pv, sm })
                                              .OrderByDescending(p => p.sm.DatePrescribed)
                                              .AsEnumerable()
                                              .Select(x => new RefundViewModel
                                              {
                                                  VisitID = x.pv.SNO,
                                                  VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(x.pv.ConsultTypeID),
                                                  BillAmount = x.sm.TotalAmount.HasValue?x.sm.TotalAmount.Value:0,
                                                  PaidAmount = x.sm.PaidAmount.HasValue ? x.sm.PaidAmount.Value : 0,
                                                  Discount = x.sm.Discount.HasValue ? x.sm.Discount.Value : 0,
                                                  //RefundAmount = x.sm.RefundAmount.HasValue ? x.sm.RefundAmount.Value : 0,
                                                  PrevRefAmount = x.sm.RefundAmount.HasValue ? x.sm.RefundAmount.Value : 0,
                                                  ENMRNO = x.pv.ENMRNO
                                              }).ToList();
        }
        }

        private static List<RefundViewModel> GetLabTestBillingHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                return (from pv in hs.PatientVisitHistories
                    join lm in hs.LabTestMasters on pv.SNO equals lm.VisitID
                    where pv.ENMRNO == enmrNo && lm.IsBillPaid == true
                    select new { pv, lm })
                                              .OrderByDescending(p => p.lm.DatePrescribed)
                                              .AsEnumerable()
                                              .Select(x => new RefundViewModel
                                              {
                                                  VisitID = x.pv.SNO,
                                                  VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(x.pv.ConsultTypeID),
                                                  BillAmount = x.lm.TotalAmount.HasValue?x.lm.TotalAmount.Value:0,
                                                  PaidAmount = x.lm.PaidAmount.HasValue ? x.lm.PaidAmount.Value : 0,
                                                  Discount = x.lm.Discount.HasValue ? x.lm.Discount.Value : 0,
                                                  //RefundAmount = x.lm.RefundAmount.HasValue ? x.lm.RefundAmount.Value : 0,
                                                  PrevRefAmount = x.lm.RefundAmount.HasValue ? x.lm.RefundAmount.Value : 0,
                                                  ENMRNO = x.pv.ENMRNO
                                              }).ToList();
        }
        }

        private static List<RefundViewModel> GetPrescriptionBillingHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                return (from pv in hs.PatientVisitHistories
                    join ps in hs.PrescriptionMasters on pv.SNO equals ps.VisitID
                    where pv.ENMRNO == enmrNo && ps.IsDelivered == true
                    select new { pv, ps })
                                              .OrderByDescending(p => p.ps.DatePrescribed)
                                              .AsEnumerable()
                                              .Select(x => new RefundViewModel
                                              {
                                                  VisitID = x.pv.SNO,
                                                  VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(x.pv.ConsultTypeID),
                                                  BillAmount = x.ps.TotalAmount.HasValue ? x.ps.TotalAmount.Value : 0,
                                                  PaidAmount = x.ps.PaidAmount.HasValue ? x.ps.PaidAmount.Value : 0,
                                                  Discount = x.ps.Discount.HasValue ? x.ps.Discount.Value : 0,
                                                  //RefundAmount = x.ps.RefundAmount.HasValue ? x.ps.RefundAmount.Value : 0,
                                                  PrevRefAmount = x.ps.RefundAmount.HasValue ? x.ps.RefundAmount.Value : 0,
                                                  ENMRNO = x.pv.ENMRNO
                                              }).ToList();
        }
        }

        private static List<RefundViewModel> GetConsultationBillingHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                return (from pv in hs.PatientVisitHistories
                        where pv.ENMRNO == enmrNo
                        select new { pv })
                                          .OrderByDescending(p => p.pv.SNO)
                                          .AsEnumerable()
                                          .Select(x => new RefundViewModel
                                          {
                                              VisitID = x.pv.SNO,
                                              VisitName = HtmlHelpers.HtmlHelpers.GetVisitName(x.pv.ConsultTypeID),
                                              BillAmount = x.pv.Fee,
                                              PaidAmount = x.pv.Fee - (x.pv.Discount.HasValue ? x.pv.Discount.Value : 0),
                                              Discount = x.pv.Discount.HasValue ? x.pv.Discount.Value : 0,
                                              //RefundAmount = x.pv.RefundAmount.HasValue ? x.pv.RefundAmount.Value : 0,
                                              PrevRefAmount = x.pv.RefundAmount.HasValue ? x.pv.RefundAmount.Value : 0,
                                              ENMRNO = x.pv.ENMRNO,
                                          }).ToList();
            }
        }
    }
}