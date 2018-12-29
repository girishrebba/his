using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;
using HIS.Controllers;
using System.ComponentModel;
using System.IO;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class InPatientController : Controller
    {
        // GET: InPatient
        [His]
        [Description(" - Inpatient View page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInPatients()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var inPatients = (from ip in hs.InPatients
                                  join user in hs.Users on ip.DoctorID equals user.UserID
                                  select new
                                  {
                                      ip,
                                      user
                                  })
                                 .OrderByDescending(b => b.ip.Enrolled)
                                 .AsEnumerable()
                                 .Select(x => new InPatient
                                 {
                                     SNO = x.ip.SNO,
                                     ENMRNO = x.ip.ENMRNO,
                                     Name = x.ip.GetFullName(),
                                     DOBDisplay = x.ip.GetDOBFormat(),
                                     Address = x.ip.GetFullAddress(),
                                     GenderDisplay = x.ip.GetGender(),
                                     Phone = x.ip.Phone,
                                     DoctorName = x.user.GetFullName(),
                                     Purpose = x.ip.Purpose,
                                     EnrolledDisplay = x.ip.GetEnrolledFormat(),
                                     IsDischarged = x.ip.IsDischarged.HasValue ? x.ip.IsDischarged : false,
                                     DischargeDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.ip.DischargedOn),
                                     PrevENMR = x.ip.PrevENMR
                                     
                                     
                                 }).ToList();

                return Json(new { data = inPatients }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewPatient(string enmrNo)
        {
            return View(GetPatientDetails(enmrNo));
        }
        
        [HttpGet]
        [Description(" - Inpatient Add/Edit page.")]
        public ActionResult AddModify(string enmrNo = null)
        {
            List<BloodGroup> BloodGroups = HtmlHelpers.HtmlHelpers.GetBloodGroups();
            List<InsuranceProvider> Insuranceproviders = HtmlHelpers.HtmlHelpers.GetInsuranceProviders();
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            if (enmrNo == null)
            {
                ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName");
                ViewBag.InsuranceprovidersList = new SelectList(Insuranceproviders, "ProviderID", "ProviderName");
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                InPatient newPatient = new InPatient();
                newPatient.ENMRNO = HtmlHelpers.HtmlHelpers.GetSequencedEnmrNo();
                newPatient.Purposes = HtmlHelpers.HtmlHelpers.GetPurposes();
                newPatient.PharmaKits = HtmlHelpers.HtmlHelpers.GetPharmaKits();
                newPatient.PurposeIds = null;
                return View(newPatient);
            }
            else
            {
                var patient = GetPatientDetails(enmrNo);
                if (patient != null)
                {
                    ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName", patient.BloodGroupID);
                    ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay", patient.DoctorID);
                    ViewBag.InsuranceprovidersList = new SelectList(Insuranceproviders, "ProviderID", "ProviderName",patient.ProviderID);
                    patient.Purposes = HtmlHelpers.HtmlHelpers.GetPurposes();
                    patient.PurposeIds = !string.IsNullOrEmpty(patient.Purpose)?patient.Purpose.Split(',') : null;
                    patient.PharmaKits = HtmlHelpers.HtmlHelpers.GetPharmaKits();
                    return View(patient);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(InPatient ip)
        {
            string message = string.Empty;
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ip.SNO == 0)
                {
                    ConstructPurpose(ip);
                    db.InPatients.Add(ip);
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("ENMRNO - {0} profile created Successfully", ip.ENMRNO)}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ConstructPurpose(ip);
                    db.SaveChanges();
                    db.Entry(ip).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("ENMRNO - {0} {1} Successfully", ip.ENMRNO, message) }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private static void ConstructPurpose(InPatient ip)
        {
            if (ip != null && ip.PurposeIds != null)
            {
                ip.Purpose = string.Join(",", ip.PurposeIds);
            }
            else
            {
                ip.Purpose = string.Empty;
            }
        }

        public InPatient GetPatientDetails(string enmrNo)
        {
            InPatient inPatient = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var inpatient = (from ip in dc.InPatients
                                 join user in dc.Users on ip.DoctorID equals user.UserID
                                 join bg in dc.BloodGroups on ip.BloodGroupID equals bg.GroupID
                                 where ip.ENMRNO.Equals(enmrNo)
                                 select new { ip, user, bg.GroupName }).FirstOrDefault();
                if (inpatient != null)
                {
                    inPatient = inpatient.ip;
                    inPatient.DoctorName = inpatient.user.GetFullName();
                    inPatient.BloodGroupDisplay = inpatient.GroupName;
                    inPatient.Name = inpatient.ip.GetFullName();
                    inPatient.Address = inpatient.ip.GetFullAddress();
                    inPatient.GenderDisplay = inpatient.ip.GetGender();
                    inPatient.MaritalStatusDisplay = inpatient.ip.GetMaritalStatus();
                    inPatient.DOBDisplay = inpatient.ip.GetDOBFormat();
                    inPatient.EnrolledDisplay = inpatient.ip.GetEnrolledFormat();
                    inPatient.DischargeDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(inpatient.ip.DischargedOn);
                }

                return inPatient;
            }
        }

        [HttpGet]
        [Description(" -  Inpatient Fee details page.")]
        public ActionResult Fee(string enmrNo = null)
        {
            FeeCollection fc = new FeeCollection();
            fc.ENMRNO = enmrNo;
            ViewBag.PayModes = new SelectList(HtmlHelpers.HtmlHelpers.GetPaymentModes(), "ModeID", "Mode"); 
            return View(fc);
        }

        public ActionResult GetFeeHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                List<FeeCollection> feeList = GetPaymentHistory(enmrNo);

                return Json(new { data = feeList }, JsonRequestBehavior.AllowGet);
            }
        }

        private static List<FeeCollection> GetPaymentHistory(string enmrNo)
        {
            using (var hs = new HISDBEntities())
            {
                return (from f in hs.FeeCollections
                        join ip in hs.InPatients on f.ENMRNO equals ip.ENMRNO
                        join pm in hs.PaymentModes on f.PaymentMode equals pm.ModeID
                        where f.ENMRNO.Equals(enmrNo)
                        select new { f, pm.Mode }).OrderByDescending(c => c.f.PaidOn).AsEnumerable()
                                               .Select(x => new FeeCollection
                                               {
                                                   PaidDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.f.PaidOn),
                                                   Amount = x.f.Amount,
                                                   Purpose = x.f.Purpose,
                                                   PayModeDisplay = x.Mode,
                                                   PayType = x.f.PayType,
                                                   PayTypeDisplay = HtmlHelpers.HtmlHelpers.PayTypeDisplay(x.f.PayType)
                                               }).ToList();
            }
        }

        public JsonResult GetObservations(string enmrNo)
        {
            return Json(new { data = GetObservationHistory(enmrNo) }, JsonRequestBehavior.AllowGet);
        }

        public List<InPatientHistory> GetObservationHistory(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var ipHistory = (from hist in hs.InPatientHistories
                                 join ip in hs.InPatients on hist.ENMRNO equals ip.ENMRNO
                                 join u in hs.Users on hist.DoctorID equals u.UserID
                                 join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                 where hist.ENMRNO.Equals(enmrNo)
                                 select new { hist, u }).OrderByDescending(c => c.hist.ObservationDate).AsEnumerable()
                                .Select(x => new InPatientHistory
                                {
                                    DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.hist.ObservationDate),
                                    Observations = x.hist.Observations,
                                    DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName)
                                }).ToList();

                return ipHistory;
            }
        }

        [HttpPost]
        public ActionResult Fee(FeeCollection fc)
        {
            if (fc != null)
            {
                using (HISDBEntities hs = new HISDBEntities())
                {
                    hs.FeeCollections.Add(fc);
                    hs.SaveChanges();
                    return Json(new { success = true, message = string.Format("Fee paid Successfully for the ENMR:{0}", fc.ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, message = "Error in Fee Adding, Please try Again!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" -  Inpatient Observations page.")]
        public ActionResult Observations(string enmrNo = null)
        {
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<InPatientHistory> ipHistory = new List<InPatientHistory>();
            InPatientHistory iph = new InPatientHistory { ENMRNO = enmrNo };
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            return View(iph);
        }

        [HttpPost]
        public ActionResult Observations(InPatientHistory iph)
        {
            if (iph != null)
            {
                using (HISDBEntities hs = new HISDBEntities())
                {
                    hs.InPatientHistories.Add(iph);
                    hs.SaveChanges();
                    return Json(new { success = true, message = string.Format("Observation for ENMRNO - {0} recorded Successfully", iph.ENMRNO)}, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { success = false, message = "Error in Observation recording, Please try Again!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" -  Inpatient Bedallocation page.")]
        public ActionResult BedAllocation(string enmrNo)
        {
            return View(GetPatientBedDetails(enmrNo));
        }

        [HttpPost]
        public ActionResult SavePatinetRoomAllocation(PatientRoomAllocation ip)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ip.AllocationID == 0)
                {
                    db.PatientRoomAllocations.Add(ip);
                    db.SaveChanges();
                    return Json(new { success = true, message =string.Format("Room Allocated for the ENMRNO - {0} Successfully", ip.ENMRNO)}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(ip).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Room Allocation for the ENMRNO - {0} updated Successfully", ip.ENMRNO) }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public PatientRoomAllocation GetPatientBedDetails(string enmrNo)
        {
            PatientRoomAllocation roomalloc = new PatientRoomAllocation();
            using (HISDBEntities dc = new HISDBEntities())
            {
                var roomallocation = (from pra in dc.PatientRoomAllocations
                                      where pra.ENMRNO.Equals(enmrNo)
                                      select new { pra }).FirstOrDefault();

                List<Room> room = (from u in dc.Rooms
                                   select new { u })
                              .OrderBy(b => b.u.RoomNo).AsEnumerable()
                              .Select(x => new Room { RoomNo = x.u.RoomNo, RoomName = x.u.RoomName }).ToList();

                List<Bed> beds = (from u in dc.Beds
                                  select new { u })
                             .OrderBy(b => b.u.BedNo).AsEnumerable()
                             .Select(x => new Bed { BedNo = x.u.BedNo, BedName = x.u.BedName + " - " + x.u.Description }).ToList();

                ViewBag.Rooms = new SelectList(HtmlHelpers.HtmlHelpers.GetAvailableRooms(), "RoomNo", "RoomName");
                ViewBag.Beds = new SelectList(beds, "BedNo", "BedName");
                ViewBag.Allocatedbed = 0;
                roomalloc.ENMRNO = enmrNo;
                roomalloc.FromDate = DateTime.Now.Date;
                if (roomallocation != null)
                {
                    ViewBag.Allocatedbed = roomallocation.pra.BedNo;
                    roomalloc.ENMRNO = roomallocation.pra.ENMRNO;
                    roomalloc.AllocationID = roomallocation.pra.AllocationID;
                    roomalloc.AllocationStatus = roomallocation.pra.AllocationStatus;
                    roomalloc.BedNo = roomallocation.pra.BedNo;
                    roomalloc.FromDate = roomallocation.pra.FromDate;
                    roomalloc.EndDate = roomallocation.pra.EndDate;
                    roomalloc.RoomNo = roomallocation.pra.RoomNo;
                    ViewBag.Rooms = new SelectList(room, "RoomNo", "RoomName", roomallocation.pra.RoomNo);
                    ViewBag.Beds = new SelectList(beds, "BedNo", "BedName", roomallocation.pra.BedNo);
                }
                return roomalloc;
            }
        }

        public JsonResult FillBeds(int Room)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var beds = (from u in db.Beds.Where(a => a.RoomNo == Room)
                                  select new { u })
                            .OrderBy(b => b.u.BedNo).AsEnumerable()
                            .Select(x => new Bed { BedNo = x.u.BedNo, BedName = x.u.BedName + " - " + x.u.Description }).ToList();
                return Json(beds, JsonRequestBehavior.AllowGet);
            }
        }

        public string DischargeSummaryNote(string enmrNo)
        {
            using (var db = new HISDBEntities())
            {
                return db.InPatients.Where(dp => dp.ENMRNO == enmrNo).FirstOrDefault().DiscSummary;
            }
        }

        public JsonResult PatientHistory(string enmrNo)
        {
            var history = new PatientHistory
            {
                Visits = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo),
                Prescriptions = HtmlHelpers.HtmlHelpers.GetPatientPrescriptions(enmrNo),
                Tests = HtmlHelpers.HtmlHelpers.GetPatientTests(enmrNo),
                Scans = HtmlHelpers.HtmlHelpers.GetPatientScans(enmrNo),
            Observations = GetObservationHistory(enmrNo),
                DischargeNote = DischargeSummaryNote(enmrNo)
            };
            return Json(new { data = history }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintHistory(string enmrNo)
        {
           // List<PatientPrescriptionHistory> patientVisitHistory = new OutPatientController.PatientPrescriptionHistory(enmrNo);
            ViewBag.Prescriptions = HtmlHelpers.HtmlHelpers.GetPatientPrescriptions(enmrNo);
            ViewBag.InPateintPrescriptions = HtmlHelpers.HtmlHelpers.InPatientPrescriptions(enmrNo);
            ViewBag.Tests = HtmlHelpers.HtmlHelpers.GetPatientTests(enmrNo);
            ViewBag.Scans = HtmlHelpers.HtmlHelpers.GetPatientScans(enmrNo);
            ViewBag.InpatientTests = HtmlHelpers.HtmlHelpers.GetInPatientTests(enmrNo);
            ViewBag.InpatientScans = HtmlHelpers.HtmlHelpers.GetInPatientScans(enmrNo);
            ViewBag.Visits = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo);
            return View(GetPatientDetails(enmrNo));
        }

        public ActionResult PrintPaymentHistory(string enmrNo)
        {
            // List<PatientPrescriptionHistory> patientVisitHistory = new OutPatientController.PatientPrescriptionHistory(enmrNo);
            ViewBag.Prescriptions = HtmlHelpers.HtmlHelpers.GetPatientPrescriptions(enmrNo);
            ViewBag.InPateintPrescriptions = HtmlHelpers.HtmlHelpers.InPatientPrescriptions(enmrNo);
            ViewBag.Tests = HtmlHelpers.HtmlHelpers.GetPatientTests(enmrNo);
            ViewBag.Scans = HtmlHelpers.HtmlHelpers.GetPatientScans(enmrNo);
            ViewBag.InpatientTests = HtmlHelpers.HtmlHelpers.GetInPatientTests(enmrNo);
            ViewBag.InpatientScans = HtmlHelpers.HtmlHelpers.GetInPatientScans(enmrNo);
            ViewBag.Visits = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo);
            return View(GetPatientDetails(enmrNo));
        }

        [HttpGet]
        public ActionResult Discharge(string enmrNo)
        {
            var dischargeModel = new DischargeModel();
            List<FeeCollection> feeCollection = GetPaymentHistory(enmrNo);
            dischargeModel.ENMRNO = enmrNo;
            dischargeModel.RoomChargeTable = HtmlHelpers.HtmlHelpers.GetRoomBilling(enmrNo);
            dischargeModel.Tests = HtmlHelpers.HtmlHelpers.GetInPatientTests(enmrNo);
            dischargeModel.Scans = HtmlHelpers.HtmlHelpers.GetInPatientScans(enmrNo);
            dischargeModel.InsuranceScantionedAmount = HtmlHelpers.HtmlHelpers.InsuranceScantionedAmount(enmrNo);
            dischargeModel.PharmaPackageAmount = HtmlHelpers.HtmlHelpers.PharmaPackAmount(enmrNo);
            dischargeModel.FeeAdvanceTable = feeCollection.Where(x=>x.PayType == 1).ToList();
            dischargeModel.FeeChargesTable = feeCollection.Where(x => x.PayType == 2).ToList();
            dischargeModel.FeeRefundedTable = feeCollection.Where(x => x.PayType == 3).ToList();

            dischargeModel.CanBeDischarge = false;
            ViewBag.TotalFee = dischargeModel.FeeAdvanceTable.Sum(i => i.Amount);
            
            if (dischargeModel.RoomChargeTable != null)
            {
                ViewBag.RoomFee = (dischargeModel.RoomChargeTable.OccupiedDays * dischargeModel.RoomChargeTable.CostPerDay);
            }
            else { ViewBag.RoomFee = 0; }

            dischargeModel.PLedger = (dischargeModel.FeeAdvanceTable.Sum(i => i.Amount) + dischargeModel.InsuranceScantionedAmount);
            dischargeModel.HLedger = (ViewBag.RoomFee + dischargeModel.PharmaPackageAmount + dischargeModel.FeeChargesTable.Sum(i => i.Amount)+ dischargeModel.FeeRefundedTable.Sum(i => i.Amount));

            if (dischargeModel.PLedger < dischargeModel.HLedger)
            {
                //ViewBag.PayAmount = ViewBag.RoomFee + dischargeModel.PharmaPackageAmount - ViewBag.TotalFee - dischargeModel.InsuranceScantionedAmount;
                ViewBag.PayAmount = dischargeModel.HLedger - dischargeModel.PLedger;
                ViewBag.Refund = 0;
                    }
            else { ViewBag.PayAmount = 0;
                ViewBag.Refund = dischargeModel.PLedger - dischargeModel.HLedger;
            }

            if (dischargeModel.PLedger == dischargeModel.HLedger)
            {
                dischargeModel.CanBeDischarge = true;
                ViewBag.PayAmount = 0;
                ViewBag.Refund = 0;
            }
            return View(dischargeModel);
        }

        [HttpPost]
        public ActionResult Discharge(DischargeModel model)
        {
            if(model != null)
            {
                using (var db = new HISDBEntities())
                {
                    var patient = db.InPatients.Where(p => p.ENMRNO == model.ENMRNO).FirstOrDefault();
                    if (patient != null) {
                        patient.IsDischarged = true;
                        patient.DischargedOn = DateTime.Today;
                        patient.DiscSummary = model.DischargeSummary;
                        db.Entry(patient).State = EntityState.Modified;
                    }

                    var patientBed = db.PatientRoomAllocations.Where(pr => pr.ENMRNO == model.ENMRNO).OrderByDescending(pr => pr.AllocationID).FirstOrDefault();
                    if (patientBed != null)
                    {
                        patientBed.AllocationStatus = false;
                        patientBed.EndDate = DateTime.Today;
                        db.Entry(patientBed).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                return Json(new { success = true, message = string.Format("ENMRNO - {0} discharged Successfully", model.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = string.Format("Error occured while discharging the ENMRNO - {0}. Please try again!!", model.ENMRNO) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Prescription(string enmrNo)
        {
           
            int visitID = 0;
            
            List<PatientPrescriptionHistory> patientVisitHistory = PatientPrescriptionHistory(enmrNo);
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<IntakeFrequency> Intakes = HtmlHelpers.HtmlHelpers.GetIntakes();
            ViewBag.Intakes = new SelectList(Intakes, "FrequencyID", "Frequency");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            ViewBag.History = patientVisitHistory;
            ViewBag.MDR = GetPatientVisitPrescriptions(enmrNo, 0).Where(v => v.ISIP== false).ToList();
            ViewBag.InPrescriptions = HtmlHelpers.HtmlHelpers.InPatientPrescriptions(enmrNo);
            ViewBag.InpatientTests = HtmlHelpers.HtmlHelpers.GetInPatientTests(enmrNo);
            ViewBag.InpatientScans = HtmlHelpers.HtmlHelpers.GetInPatientScans(enmrNo);
            ViewBag.IsNewVisit = patientVisitHistory.Count() <= 0 ? true : false;
            PatientPrescription pp = new PatientPrescription();
            pp.ENMRNO = enmrNo;
            pp.VisitID = visitID;
            pp.VisitName = "In Patient - Prescription";
            pp.TestTypes = HtmlHelpers.HtmlHelpers.GetTestTypes();
            pp.Scans = HtmlHelpers.HtmlHelpers.GetScans();
            return View(pp);
        }

        [HttpPost]
        public ActionResult Prescription(IList<PatientPrescription> prescriptions)
        {
            int pmid = 0;
            int stmid = 0;
            string enmrNo = string.Empty;
            using (HISDBEntities db = new HISDBEntities())
            {
                if (prescriptions != null && prescriptions.Count() > 0)
                {
                    enmrNo = prescriptions[0].ENMRNO;
                    var suggestedTestsIfAny = prescriptions[0];
                    if (prescriptions[0].HasPrescription)
                    {
                        var pMaster = db.PrescriptionMasters.Where(pm => pm.ENMRNO == enmrNo && pm.VisitID == 0 && pm.IsDelivered == false && pm.ISIP == true).FirstOrDefault();
                        if (pMaster != null)
                        {
                            pmid = pMaster.PMID;
                        }
                        else
                        {
                            System.Data.Entity.Core.Objects.ObjectParameter pmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("PMID", typeof(Int32));

                            db.CreateMasterPrescription(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, true, pmidOut);
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
                        SaveLabTestOrPackage(prescriptions, enmrNo, db, suggestedTestsIfAny.TestIds);
                    }
                    // Save Package
                    if (suggestedTestsIfAny.KitIds != null)
                    {
                        SaveLabTestOrPackage(prescriptions, enmrNo, db, suggestedTestsIfAny.KitIds);
                    }
                    //Save Scans
                    if (suggestedTestsIfAny.ScanIds != null)
                    {
                        var sMaster = db.ScanTestMasters.Where(pm => pm.ENMRNO == enmrNo && pm.VisitID == 0 && pm.IsDelivered == false).FirstOrDefault();
                        if (sMaster != null)
                        {
                            stmid = sMaster.STMID;
                        }
                        else
                        {
                            System.Data.Entity.Core.Objects.ObjectParameter ltmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("STMID", typeof(Int32));
                            db.CreateMasterScanTest(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, true, ltmidOut);
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

        private static void SaveLabTestOrPackage(IList<PatientPrescription> prescriptions, string enmrNo, HISDBEntities db, string[] tests)
        {
            int ltmid;
            var lMaster = db.LabTestMasters.Where(pm => pm.ENMRNO == enmrNo && pm.VisitID == 0 && pm.IsDelivered == false).FirstOrDefault();
            if (lMaster != null)
            {
                ltmid = lMaster.LTMID;
            }
            else
            {
                System.Data.Entity.Core.Objects.ObjectParameter ltmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("LTMID", typeof(Int32));
                db.CreateMasterLabTest(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, true, ltmidOut);
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

        [HttpGet]
        public ActionResult PatientTests(string enmrNo)
        {
            ViewBag.ENMRNO = enmrNo;
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                patientTests = GetPatientNotDeliverVisitTests(enmrNo, 0);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = "In Patient";
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
            var PatientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                PatientScans = GetPatientNotDeliverVisitScans(enmrNo, 0);
                if (PatientScans.Count() > 0)
                {
                    PatientScans[0].ENMRNO = enmrNo;
                    PatientScans[0].VisitName = "In Patient";
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
        [Description(" - InPatient Lab Test Bill Payment Form.")]
        public ActionResult LabTestBillPay(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                patientTests = GetPatientVisitTestsBillPay(enmrNo, 0);
                string visitName = "In Patient";
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = visitName;

                }
            }
            return View(patientTests);
        }

        [HttpGet]
        public ActionResult LabTestBillPayPrint(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                patientTests = GetPatientVisitTestsBillPayPrint(enmrNo, 0);
                string visitName = "In Patient";
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = visitName;

                }
            }
            return View(patientTests);
        }

        [HttpPost]
        [Description(" - InPatient Lab Test Bill Payment Form.")]
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

        [HttpGet]
        [Description(" - InPatient Scan Test Bill Payment Form.")]
        public ActionResult ScanTestBillPay(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                patientScans = HtmlHelpers.HtmlHelpers.GetPatientVisitScansBillPay(enmrNo, 0);
                string visitName = "In Patient";
                if (patientScans.Count() > 0)
                {
                    patientScans[0].ENMRNO = enmrNo;
                    patientScans[0].VisitName = visitName;

                }
            }
            return View(patientScans);
        }

        [HttpGet]
        public ActionResult ScanTestBillPayPrint(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientScans = new List<PatientScan>();
            using (var hs = new HISDBEntities())
            {
                patientScans = HtmlHelpers.HtmlHelpers.GetPatientVisitScansBillPayPrint(enmrNo, 0);
                string visitName = "In Patient";
                if (patientScans.Count() > 0)
                {
                    patientScans[0].ENMRNO = enmrNo;
                    patientScans[0].VisitName = visitName;

                }
            }
            return View(patientScans);
        }

        [HttpPost]
        [Description(" - InPatient Scan Test Bill Payment Form.")]
        public ActionResult ScanTestBillPay(MasterBillPayModel masterLab)
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

                return Json(new { success = true, message = string.Format("Scan Tests for ENMRNO - {0} paid Successfully", masterLab.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult DeliverPrescription(string enmrNo)
        {
            int visitID = 0;
            var prescriptions = new List<PatientPrescription>();
            using (var hs = new HISDBEntities())
            {
                prescriptions = GetPatientNotDeliverPrescriptions(enmrNo, visitID);
                string visitName = "In Patient";
                if (prescriptions.Count() > 0)
                {
                    foreach (var pp in prescriptions)
                    {
                        var itemCost = hs.MedicineInventories.Where(mi => mi.MedicineID == pp.MedicineID).First().PricePerItem.Value;
                        pp.ItemCost = itemCost;
                        pp.TotalCost = pp.Quantity * itemCost;
                        pp.VisitName = visitName;
                        pp.DeliverQty = pp.Quantity;
                        pp.RequestQty = 0;
                    }
                }
            }
            return View(prescriptions);
        }

        [HttpGet]
        public ActionResult DeliverPrescriptionprint(string enmrNo)
        {
            int visitID = 0;
            var prescriptions = new List<PatientPrescription>();
            using (var hs = new HISDBEntities())
            {
                prescriptions = PatientVisitDeliveredPrescriptionsPrint(enmrNo, visitID);
                string visitName = "In Patient";
                if (prescriptions.Count() > 0)
                {
                    foreach (var pp in prescriptions)
                    {
                        var itemCost = hs.MedicineInventories.Where(mi => mi.MedicineID == pp.MedicineID).First().PricePerItem.Value;
                        pp.ItemCost = itemCost;
                        pp.TotalCost = pp.Quantity * itemCost;
                        pp.VisitName = visitName;
                        pp.DeliverQty = pp.Quantity;
                        //pp.RequestQty = 0;
                    }
                }
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
                        if (prescription != null)
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


        private List<PatientPrescriptionHistory> PatientPrescriptionHistory(string enmrNo)
        {
            var prescriptionsHistory = new List<PatientPrescriptionHistory>();
            using (var db = new HISDBEntities())
            {
                var prescribedVisits = (from pv in db.PatientVisitHistories
                                        join pm in db.PrescriptionMasters on pv.SNO equals pm.VisitID
                                        //join pp in db.PatientPrescriptions on pm.PMID equals pp.PMID
                                        where pv.ENMRNO == enmrNo
                                        select pv).ToList();
                foreach (var pre in prescribedVisits)
                {
                    var visitPrescription = new PatientPrescriptionHistory();
                    visitPrescription.VisitName = VisitNameWithDate(pre);
                    visitPrescription.Prescriptions = GetPatientVisitPrescriptions(pre.ENMRNO, pre.SNO);
                    visitPrescription.PatientTests = GetPatientVisitTests(pre.ENMRNO, pre.SNO);
                    visitPrescription.PatientScans = HtmlHelpers.HtmlHelpers.GetOpPatientScans(enmrNo, pre.SNO);
                    prescriptionsHistory.Add(visitPrescription);
                }
                return prescriptionsHistory;
            }
        }

        public List<PatientPrescription> GetPatientVisitPrescriptions(string enmrNo, int visitID)
        {
            return PatientVisitPrescriptions(enmrNo, visitID);
        }

        public List<PatientPrescription> GetPatientNotDeliverPrescriptions(string enmrNo, int visitID)
        {
            return PatientVisitNotDeliverPrescriptions(enmrNo, visitID);
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
                                     ISIP = x.pm.ISIP.HasValue ? x.pm.ISIP.Value : false
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        private static List<PatientPrescription> PatientVisitNotDeliverPrescriptions(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var latestPMID = hs.PrescriptionMasters.Where(pms => pms.ENMRNO == enmrNo).OrderByDescending(pms => pms.PMID).FirstOrDefault().PMID;
                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID && pm.IsDelivered == false && pm.ISIP == true && pm.PMID == latestPMID
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

        private static List<PatientPrescription> PatientVisitDeliveredPrescriptionsPrint(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var latestPMID = hs.PrescriptionMasters.Where(pms => pms.ENMRNO == enmrNo).OrderByDescending(pms => pms.PMID).FirstOrDefault().PMID;
                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID &&  pm.ISIP == true && pm.PMID == latestPMID
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
                                    join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                    join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                    join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                    where ltm.ENMRNO == enmrNo && ltm.VisitID == visitID
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

        private static List<PatientTest> PatientNotDeliverVisitTests(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
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

                var patientscans = (from pt in hs.PatientScans
                                    join ltm in hs.ScanTestMasters on pt.STMID equals ltm.STMID
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
                                  .OrderByDescending(b => b.pt.PSID)
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

                return patientscans;
            }
        }


        public List<PatientTest> GetPatientVisitTestsBillPay(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
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
                                     TestCost = x.tt.TestCost.HasValue ? x.tt.TestCost.Value : 0
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
                                     TestCost = x.tt.TestCost.HasValue ? x.tt.TestCost.Value : 0
                                 }).ToList();

                return patientTests;
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
        private string VisitName(int consultTypeID)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == consultTypeID).FirstOrDefault().ConsultType;

                return consultName;
            }
        }

        [HttpPost]
        public JsonResult GetTestNames(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var tests = (from tt in hs.TestTypes
                                 where tt.IsKit == false 
                                 && tt.TestName.StartsWith(Prefix)
                                 select new { tt }).AsEnumerable()
                                 .Select(m => new TestType
                                 {
                                     TestID = m.tt.TestID,
                                     TestName = m.tt.TestName,
                                 }).ToList();
                return Json(tests, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DischargeFileUpload()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;

            string emrno = Request.Form.AllKeys[0];
           // int test = Convert.ToInt32(Request.Form.AllKeys[1]);

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
               // int emr = db.LabTestMasters.Where(a => a.ENMRNO == emrno).OrderByDescending(a => a.LTMID).Select(b => b.LTMID).FirstOrDefault();

                db.Database.ExecuteSqlCommand("update inpatients set DischargeReportpath='" + dbfname + "' where ENMRNO = '" + emrno + "'");
                db.SaveChanges();
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PackagesAllocation(string enmrNo)
        {
            PackageViewModel pvm = new PackageViewModel();
            return View(pvm);
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

    }
}