using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using HIS.Action_Filters;
using System.ComponentModel;

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

                prescriptions = GetPatientVisitPrescriptions(enmrNo, latestVisit.SNO);
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
                    int PMID = prescriptions[0].PMID;
                    foreach (PatientPrescription pp in prescriptions)
                    {
                        var prescription = db.PatientPrescriptions.Where(p => p.PMID == pp.PMID && p.MedicineID == pp.MedicineID).FirstOrDefault();
                        if(prescription != null)
                        {
                            //prescription.IsDelivered = true;
                            prescription.DeliverQty = pp.DeliverQty;
                            db.Entry(prescription).State = EntityState.Modified;
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
                    
                    db.CreateMasterPrescription(mdrRequest[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), 0, pmidOut);
                    int pmid = Convert.ToInt32(pmidOut.Value);
                    foreach (PatientPrescription pp in mdrRequest)
                    {
                        pp.PMID = pmid;
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

        public JsonResult GetPatientVisits(string enmrNo)
        {
            return Json(new { data = HtmlHelpers.HtmlHelpers.GetOutPatientVisits(enmrNo) }, JsonRequestBehavior.AllowGet);
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
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientPrescriptions = (from pp in hs.PatientPrescriptions
                                            join pm in hs.PrescriptionMasters on pp.PMID equals pm.PMID
                                            join op in hs.OutPatients on pm.ENMRNO equals op.ENMRNO
                                            join mm in hs.MedicineMasters on pp.MedicineID equals mm.MMID
                                            join ifs in hs.IntakeFrequencies on pp.IntakeFrequencyID equals ifs.FrequencyID
                                            join u in hs.Users on pm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where ut.UserTypeName.Equals("Doctor") && pm.ENMRNO.Equals(enmrNo) && pm.VisitID == visitID
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
            using (HISDBEntities hs = new HISDBEntities())
            {

                var patientTests = (from pt in hs.PatientTests
                                    join ltm in hs.LabTestMasters on pt.LTMID equals ltm.LTMID
                                    join op in hs.OutPatients on ltm.ENMRNO equals op.ENMRNO
                                            join tt in hs.TestTypes on pt.TestID equals tt.TestID
                                            join u in hs.Users on ltm.PrescribedBy equals u.UserID
                                            join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                                            where ut.UserTypeName.Equals("Doctor") && ltm.ENMRNO == enmrNo && ltm.VisitID == visitID
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
                                    where ut.UserTypeName.Equals("Doctor") && ltm.ENMRNO == enmrNo && ltm.VisitID == visitID && ltm.IsBillPaid == true
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

        [HttpGet]
        public ActionResult OPPrescription(string enmrNo)
        {
            string currentVisit = string.Empty;
            bool isLatestVisitPrescribed = false;
            int visitID = 1;
            using (HISDBEntities hs = new HISDBEntities())
            {
                var latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();
                if (latestVisit != null)
                {
                    visitID = latestVisit.SNO;
                    currentVisit = VisitNameWithDate(latestVisit);
                    isLatestVisitPrescribed = hs.PrescriptionMasters.Where(p => p.VisitID == visitID).Any();
                }
            }
            List<PatientPrescriptionHistory> patientVisitHistory = PatientPrescriptionHistory(enmrNo);
            List<User> Users = HtmlHelpers.HtmlHelpers.GetDoctors();
            List<IntakeFrequency> Intakes = HtmlHelpers.HtmlHelpers.GetIntakes();
            ViewBag.Intakes = new SelectList(Intakes, "FrequencyID", "Frequency");
            ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
            ViewBag.History = patientVisitHistory;
            ViewBag.IsNewVisit = patientVisitHistory.Count() <= 0 ? true : false;
            ViewBag.IsLastVisitPrescribed = isLatestVisitPrescribed;
            PatientPrescription pp = new PatientPrescription();
            pp.ENMRNO = enmrNo;
            pp.VisitID = visitID;
            pp.VisitName = currentVisit;
            pp.TestTypes = HtmlHelpers.HtmlHelpers.GetTestTypes();
            return View(pp);
        }

        private List<PatientPrescriptionHistory> PatientPrescriptionHistory(string enmrNo)
        {
            var prescriptionsHistory = new List<PatientPrescriptionHistory>();
            using (var db = new HISDBEntities())
            {
                var prescribedVisits = (from pv in db.PatientVisitHistories
                                        join pm in db.PrescriptionMasters on pv.SNO equals pm.VisitID
                                        join pp in db.PatientPrescriptions on pm.PMID equals pp.PMID
                                        
                                        where pv.ENMRNO == enmrNo select pv).ToList();
                foreach (var pre in prescribedVisits)
                {
                    var visitPrescription = new PatientPrescriptionHistory();
                    visitPrescription.VisitName = VisitNameWithDate(pre);
                    visitPrescription.Prescriptions = GetPatientVisitPrescriptions(pre.ENMRNO, pre.SNO);
                    visitPrescription.PatientTests = GetPatientVisitTests(pre.ENMRNO, pre.SNO);
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
                    System.Data.Entity.Core.Objects.ObjectParameter pmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("PMID", typeof(Int32));

                    db.CreateMasterPrescription(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, pmidOut);
                    int pmid = Convert.ToInt32(pmidOut.Value);

                    var suggestedTestsIfAny = prescriptions[0];
                    foreach (PatientPrescription pp in prescriptions)
                    {
                        pp.PMID = pmid;
                        db.PatientPrescriptions.Add(pp);
                    }
                    db.SaveChanges();
                    //Save Tests
                    if (suggestedTestsIfAny.TestIds != null)
                    {
                        System.Data.Entity.Core.Objects.ObjectParameter ltmidOut = new System.Data.Entity.Core.Objects.ObjectParameter("LTMID", typeof(Int32));
                        db.CreateMasterLabTest(prescriptions[0].ENMRNO, Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]), prescriptions[0].VisitID, ltmidOut);
                        int ltmid = Convert.ToInt32(ltmidOut.Value);

                        foreach (var id in suggestedTestsIfAny.TestIds)
                        {
                            var patientTest = new PatientTest {
                                TestID = Convert.ToInt32(id),
                                LTMID = ltmid
                            };
                            db.PatientTests.Add(patientTest);
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
                                 select new { mm, mi }).AsEnumerable()
                                 .Select(m => new MedicineMaster
                                 {
                                     MMID = m.mm.MMID,
                                     MedicineDisplay = HtmlHelpers.HtmlHelpers.GetMedicineWithDoseAvailableQty(m.mm.MedicineName, m.mm.MedDose, m.mi.AvailableQty.Value),
                                     ItemPrice = m.mi.PricePerItem
                                 }).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult PatientTests(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                patientTests = GetPatientVisitTests(enmrNo, latestVisit.SNO);
                string visitName = VisitName(latestVisit.ConsultTypeID);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = VisitName(latestVisit.ConsultTypeID);
                    
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
        [Description(" - OutPatient Lab Test Bill Payment Form.")]
        public ActionResult LabTestBillPay(string enmrNo)
        {
            var latestVisit = new PatientVisitHistory();
            var patientTests = new List<PatientTest>();
            using (var hs = new HISDBEntities())
            {
                latestVisit = hs.PatientVisitHistories.Where(pvh => pvh.ENMRNO == enmrNo).OrderByDescending(pvh => pvh.SNO).FirstOrDefault();

                patientTests = GetPatientVisitTestsBillPay(enmrNo, latestVisit.SNO);
                string visitName = VisitName(latestVisit.ConsultTypeID);
                if (patientTests.Count() > 0)
                {
                    patientTests[0].ENMRNO = enmrNo;
                    patientTests[0].VisitName = VisitName(latestVisit.ConsultTypeID);

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
    }
}