using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
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

                prescriptions = GetPatientPrescriptions(enmrNo, latestVisit.SNO);
                string visitName = VisitName(latestVisit.ConsultTypeID);
                if(prescriptions.Count() > 0)
                {
                    foreach(var pp in prescriptions)
                    {
                        var itemCost = hs.MedicineInventories.Where(mi => mi.MedicineID == pp.MedicineID).First().PricePerItem.Value;
                        pp.ItemCost = itemCost;
                        pp.TotalCost = pp.Quantity * itemCost;
                        pp.VisitName = visitName;
                        pp.DeliverQty = pp.Quantity;
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
                    foreach (PatientPrescription pp in prescriptions)
                    {
                        var prescription = db.PatientPrescriptions.Where(p => p.ENMRNO == pp.ENMRNO && p.VisitID == pp.VisitID && p.MedicineID == pp.MedicineID).FirstOrDefault();
                        if(prescription != null)
                        {
                            prescription.IsDelivered = true;
                            prescription.DeliverQty = pp.DeliverQty;
                            db.Entry(prescription).State = EntityState.Modified;
                        }
                        
                        db.SaveChanges();
                        var medInv = db.MedicineInventories.Where(miv => miv.MedicineID == pp.MedicineID).First();
                        medInv.AvailableQty = medInv.AvailableQty - pp.DeliverQty;
                        db.Entry(medInv).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { success = true, message = string.Format("Prescription for ENMRNO - {0} delivered Successfully", prescriptions[0].ENMRNO) }, JsonRequestBehavior.AllowGet);
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
                    db.OutPatients.Add(op);
                    db.SaveChanges();
                    CreateVisit(op);
                    HtmlHelpers.HtmlHelpers.CreatePatientDirectory("OutPatient", op.ENMRNO);
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

        private void CreateVisit(OutPatient op)
        {
            decimal fee = 0;
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
                    DateOfVisit = op.Enrolled.Value
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
                                            where ut.UserTypeName.Equals("Doctor") && pp.ENMRNO == enmrNo && pp.VisitID == visitID
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
                                     VisitID = x.pp.VisitID,
                                     DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                     Quantity = x.pp.Quantity,
                                     MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mm.MedicineName, x.mm.MedDose),
                                     IntakeDisplay = x.Frequency,
                                     MedicineID = x.pp.MedicineID                             
                                 }).ToList();

                return patientPrescriptions;
            }
        }

        [HttpGet]
        public ActionResult OPPrescription(string enmrNo)
        {
            string currentVisit = string.Empty;
            bool isLatestVisitPrescried = false;
            int visitID = 1;
            using (HISDBEntities hs = new HISDBEntities())
            {
                var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                if (latestVisit != null)
                {
                    visitID = latestVisit.SNO;
                    currentVisit = VisitNameWithDate(latestVisit);
                    isLatestVisitPrescried = hs.PatientPrescriptions.Where(p => p.VisitID == visitID).Any();
                }
            }
            List<PatientPrescriptionHistory> patientVisitHistory = PatientPrescriptionHistory(enmrNo);
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<IntakeFrequency> Intakes = HtmlHelpers.HtmlHelpers.GetIntakes();
            ViewBag.Intakes = new SelectList(Intakes, "FrequencyID", "Frequency");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            ViewBag.History = patientVisitHistory;
            ViewBag.IsNewVisit = patientVisitHistory.Count() <= 0 ? true : false;
            ViewBag.IsLastVisitPrescribed = isLatestVisitPrescried;
            PatientPrescription pp = new PatientPrescription();
            pp.ENMRNO = enmrNo;
            pp.VisitID = visitID;
            pp.VisitName = currentVisit;
            return View(pp);
        }

        private List<PatientPrescriptionHistory> PatientPrescriptionHistory(string enmrNo)
        {
            var prescriptionsHistory = new List<PatientPrescriptionHistory>();
            using (var db = new HISDBEntities())
            {
                var prescribedVisits = (from pv in db.PatientVisitHistories 
                                        join pp in db.PatientPrescriptions on pv.SNO equals pp.VisitID 
                                        where pv.ENMRNO == enmrNo select pv).ToList();
                foreach (var pre in prescribedVisits)
                {
                    var visitPrescription = new PatientPrescriptionHistory();
                    visitPrescription.VisitName = VisitNameWithDate(pre);
                    visitPrescription.Prescriptions = GetPatientPrescriptions(pre.ENMRNO, pre.SNO);
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
        private string VisitName(int consultTypeID)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                var consultName = db.ConsultationTypes.Where(ct => ct.ConsultTypeID == consultTypeID).FirstOrDefault().ConsultType;

                return consultName;
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
        public ActionResult ConvertOpToIp(string enmrNo)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                db.ConvertOutPatientToInPatient(enmrNo);

                return Json(new { success = true, message = string.Format("Patient ENMRNO: {0} converted to In Patient", enmrNo) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetMedicinesWithQuantity(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from mm in hs.MedicineMasters
                                 join mi in hs.MedicineInventories on mm.MMID equals mi.MedicineID
                                 where mm.MedicineName.StartsWith(Prefix)
                                 select new { mm, mi.AvailableQty }).AsEnumerable()
                                 .Select(m => new MedicineMaster
                                 {
                                     MMID = m.mm.MMID,
                                     MedicineDisplay = HtmlHelpers.HtmlHelpers.GetMedicineWithDoseAvailableQty(m.mm.MedicineName, m.mm.MedDose, m.AvailableQty.Value)
                                 }).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }
        }

    }
}