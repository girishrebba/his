using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace HIS.Controllers
{
    public class OutPatientController : Controller
    {
        // GET: OutPatient
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
                                     EnrolledDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.op.Enrolled)
                                 }).ToList();

                return Json(new { data = outPatients }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewPatient(int id)
        {
            return View(GetPatientDetails(id));
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<BloodGroup> BloodGroups = HtmlHelpers.HtmlHelpers.GetBloodGroups();
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            if (id == 0)
            {
                ViewBag.BloodGroups = new SelectList(BloodGroups, "GroupID", "GroupName");
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                OutPatient newPatient = new OutPatient();
                newPatient.ENMRNO = HtmlHelpers.HtmlHelpers.GetSequencedEnmrNo();
                return View(newPatient);
            }
            else
            {
                var patient = GetPatientDetails(id);
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
                    db.OutPatients.Add(op);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(op).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public OutPatient GetPatientDetails(int id)
        {
            OutPatient outPatient = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var outpatient = (from op in dc.OutPatients
                                 join user in dc.Users on op.DoctorID equals user.UserID
                                 join bg in dc.BloodGroups on op.BloodGroupID equals bg.GroupID
                                 where op.SNO.Equals(id)
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
                }
                return outPatient;
            }
        }

        public ActionResult GetPatientVisits(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var patientVisits = (from pv in hs.PatientVisitHistories
                                    join op in hs.OutPatients on pv.ENMRNO equals op.ENMRNO
                                    join ct in hs.ConsultationTypes on pv.ConsultTypeID equals ct.ConsultTypeID
                                    join u in hs.Users on pv.DoctorID equals u.UserID
                                     join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                     where ut.UserTypeName.Equals("Doctor") && pv.ENMRNO.Equals(enmrNo)
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
                                     DOVDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pv.DateOfVisit),
                                     ENMRNO = x.pv.ENMRNO,
                                     ConsultType = x.ConsultType,
                                     Fee = x.pv.Fee,
                                     Discount = x.pv.Discount,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName)
                                 }).ToList();   
                return Json(new { data = patientVisits }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult NewVisit(PatientVisitHistory pvh)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                hs.PatientVisitHistories.Add(pvh);
                hs.SaveChanges();
                return Json(new { success = true, message = string.Format("Consultation created Successfully for ENMRNO: {0}", pvh.ENMRNO) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetConsultationFee(int doctorId, int consultTypeId )
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
        public ActionResult NewVisit(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities hs = new HISDBEntities()) {
                enmrNo = hs.OutPatients.Where(op => op.SNO == id).FirstOrDefault().ENMRNO;
            }
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<ConsultationType> Consultations = HtmlHelpers.HtmlHelpers.GetConsultationTypes();
            ViewBag.Consultations = new SelectList(Consultations, "ConsultTypeID", "ConsultType");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            PatientVisitHistory pvh = new PatientVisitHistory();
            pvh.ENMRNO = enmrNo;
            return View(pvh);
        }
        public ActionResult GetOPPatientPrescriptions(string enmrNo)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join op in hs.OutPatients on pp.ENMRNO equals op.ENMRNO
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pp.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where ut.UserTypeName.Equals("Doctor") && pp.ENMRNO.Equals(enmrNo)
                                            select new
                                            {
                                                pp,
                                                u,
                                                mm,
                                                ifs.Frequency
                                            })
                                  .OrderByDescending(b => b.pp.DatePrescribed)
                                  .AsEnumerable()
                                 .Select(x => new PatientPrescription
                                 {
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pp.DatePrescribed),
                                     ENMRNO = x.pp.ENMRNO,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency
                                 }).ToList();
                return Json(new { data = patientPrescriptions }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<PatientPrescription> GetPatientPrescriptions(string enmrNo, int visitID)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join op in hs.OutPatients on pp.ENMRNO equals op.ENMRNO
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pp.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where ut.UserTypeName.Equals("Doctor") && pp.ENMRNO.Equals(enmrNo)&& pp.VisitID.Equals(visitID) 
                                            select new
                                            {
                                                pp,
                                                u,
                                                mm,
                                                ifs.Frequency
                                            })
                                  .OrderByDescending(b => b.pp.DatePrescribed)
                                  .AsEnumerable()
                                 .Select(x => new PatientPrescription
                                 {
                                     DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.pp.DatePrescribed),
                                     ENMRNO = x.pp.ENMRNO,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        [HttpGet]
        public ActionResult OPPrescription(int id)
        {
            string enmrNo = string.Empty;
            string currentVisit = string.Empty;
            int visitID = 1;
            var prescriptions = new List<PatientPrescriptionHistory>();
            using (HISDBEntities hs = new HISDBEntities())
            {
                enmrNo = HtmlHelpers.HtmlHelpers.Get_OUT_ENMR(id);
                var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                foreach(var pre in hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo))
                {
                    var visitPrescription = new PatientPrescriptionHistory();
                    visitPrescription.VisitName = VisitName(pre);
                    visitPrescription.Prescriptions =   GetPatientPrescriptions(pre.ENMRNO, pre.SNO);
                    prescriptions.Add(visitPrescription);
                }

                if(latestVisit != null)
                {
                    visitID = latestVisit.SNO;
                    currentVisit = VisitName(latestVisit);
                }
            }
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<IntakeFrequency> Intakes = HtmlHelpers.HtmlHelpers.GetIntakes();
            ViewBag.Intakes = new SelectList(Intakes, "FrequencyID", "Frequency");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            ViewBag.History = prescriptions;
            PatientPrescription pp = new PatientPrescription();
            pp.ENMRNO = enmrNo;
            pp.VisitID = visitID;
            pp.CurrentVsit = currentVisit;
            return View(pp);
        }

        private string VisitName(PatientVisitHistory visit)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == visit.ConsultTypeID).FirstOrDefault().ConsultType;

                return string.Format("{0} : {1}", consultName, HtmlHelpers.HtmlHelpers.DateFormat(visit.DateOfVisit));
            }
        }

        [HttpPost]
        public ActionResult OPPrescription(IList<PatientPrescription> prescriptions)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (prescriptions != null && prescriptions.Count() > 0)
                {
                    foreach (PatientPrescription pp in prescriptions)
                    {
                        pp.PrescribedBy = 1;
                        pp.DatePrescribed = DateTime.Now;
                        db.PatientPrescriptions.Add(pp);
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

        [HttpPost]
        public ActionResult ConvertOpToIp(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities db = new HISDBEntities())
            {
                enmrNo = HtmlHelpers.HtmlHelpers.Get_OUT_ENMR(id);
                db.ConvertOutPatientToInPatient(enmrNo);

                return Json(new { success = true, message = string.Format("Patient ENMRNO: {0} converted to In Patient", enmrNo) }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}