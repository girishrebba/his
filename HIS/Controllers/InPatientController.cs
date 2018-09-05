using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;

using System.ComponentModel;


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
                                     DischargeDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.ip.DischargedOn)
                                     
                                     
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
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            if (enmrNo == null)
            {
                ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName");
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                InPatient newPatient = new InPatient();
                newPatient.ENMRNO = HtmlHelpers.HtmlHelpers.GetSequencedEnmrNo();
                return View(newPatient);
            }
            else
            {
                var patient = GetPatientDetails(enmrNo);
                if (patient != null)
                {
                    ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName", patient.BloodGroupID);
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
        public ActionResult AddModify(InPatient ip)
        {
            string message = string.Empty;
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ip.SNO == 0)
                {
                    db.InPatients.Add(ip);
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("ENMRNO - {0} profile created Successfully", ip.ENMRNO)}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = "profile updated";
                    if (!string.IsNullOrEmpty(ip.DiscSummary) && ((ip.IsDischarged == null || ip.IsDischarged == false)&& ip.DischargedOn == null))
                    {
                        ip.IsDischarged = true;
                        ip.DischargedOn = DateTime.Today;
                        var patientBedRecord = db.PatientRoomAllocations.Where(pr => pr.ENMRNO == ip.ENMRNO).OrderByDescending(pr => pr.AllocationID).FirstOrDefault();
                        if (patientBedRecord != null)
                        {
                            patientBedRecord.AllocationStatus = false;
                            patientBedRecord.EndDate = DateTime.Today;
                            db.Entry(patientBedRecord).State = EntityState.Modified;
                        }
                        message = "Discarged";
                    }
                    db.Entry(ip).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("ENMRNO - {0} {1} Successfully", ip.ENMRNO, message) }, JsonRequestBehavior.AllowGet);
                }
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
                                                   PayModeDisplay = x.Mode
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
                                 where ut.UserTypeName.Equals("Doctor") && hist.ENMRNO.Equals(enmrNo)
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
                    return Json(new { success = true, message = "Fee added Successfully" }, JsonRequestBehavior.AllowGet);
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
                             .Select(x => new Bed { BedNo = x.u.BedNo, BedName = x.u.BedName }).ToList();

                ViewBag.Rooms = new SelectList(room, "RoomNo", "RoomName");
                ViewBag.Beds = new SelectList(beds, "BedNo", "BedName");
                roomalloc.ENMRNO = enmrNo;
                roomalloc.FromDate = DateTime.Now.Date;
                if (roomallocation != null)
                {
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
                            .Select(x => new Bed { BedNo = x.u.BedNo, BedName = x.u.BedName }).ToList();
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
                Observations = GetObservationHistory(enmrNo),
                DischargeNote = DischargeSummaryNote(enmrNo)
            };
            return Json(new { data = history }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintHistory(string enmrNo)
        {
            return View(GetPatientDetails(enmrNo));
        }

        public ActionResult Discharge(string enmrNo)
        {
            var dischargeModel = new DischargeModel();
            dischargeModel.ENMRNO = enmrNo;
            dischargeModel.RoomChargeTable = HtmlHelpers.HtmlHelpers.GetRoomBilling(enmrNo);
            dischargeModel.FeeCollectionTable = GetPaymentHistory(enmrNo);
            ViewBag.TotalFee = dischargeModel.FeeCollectionTable.Sum(i => i.Amount);
            ViewBag.RoomFee = (dischargeModel.RoomChargeTable.OccupiedDays * dischargeModel.RoomChargeTable.CostPerDay);
            if(ViewBag.TotalFee < ViewBag.RoomFee)
            {
                ViewBag.PayAmount = ViewBag.RoomFee - ViewBag.TotalFee;
                ViewBag.Refund = 0;
                    }
            else { ViewBag.PayAmount = 0;
                ViewBag.Refund = ViewBag.TotalFee - ViewBag.RoomFee;
            }
            return View(dischargeModel);
        }
    }
}