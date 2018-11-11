using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel;

namespace HIS.Controllers
{
    public class ConsultantController : Controller
    {
        // GET: Consultant
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetConsultants()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var consultants = (from c in hs.Consultants
                             join spl in hs.Specializations on c.SpecializationID equals spl.SpecializationID
                             select new { c, spl }).AsEnumerable()
                         .Select(x => new Consultant
                         {
                             ConsultantID = x.c.ConsultantID,
                             NameDisplay = HtmlHelpers.HtmlHelpers.GetFullName(x.c.FirstName,x.c.MiddleName,x.c.LastName),
                             DOBDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.c.DOB),
                             GenderDisplay = HtmlHelpers.HtmlHelpers.GetGender(x.c.Gender),
                             Email = x.c.Email,
                             Phone = x.c.Phone,
                             MaritalStatusDisplay = HtmlHelpers.HtmlHelpers.GetMaritalStatus(x.c.MaritalStatus),
                             Qualification = x.c.Qualification,
                             DoctorTypeDisplay = x.spl.DoctorType
                         }).ToList();
                return Json(new { data = consultants }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<Specialization> Specializations = HtmlHelpers.HtmlHelpers.GetSpecializations();
            if (id == 0)
            {
                ViewBag.Specializations = new SelectList(Specializations, "SpecializationID", "DoctorType");
                return View(new Consultant());
            }
            else
            {
                var user = GetConsultant(id);
                if (user != null)
                {
                    ViewBag.Specializations = new SelectList(Specializations, "SpecializationID", "DoctorType", user.SpecializationID);
                    return View(user);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public Consultant GetConsultant(int consultID)
        {
            Consultant consultant = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from c in dc.Consultants
                         join s in dc.Specializations on c.SpecializationID equals s.SpecializationID
                         where c.ConsultantID.Equals(consultID)
                         select new { c }).FirstOrDefault();
                if (v != null)
                {
                    consultant = v.c;
                    consultant.DOBDisplay = HtmlHelpers.HtmlHelpers.DateFormat(v.c.DOB);
                }
                return consultant;
            }
        }

        [HttpPost]
        public ActionResult AddModify(Consultant consultant)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (consultant.ConsultantID == 0)
                {
                    db.Consultants.Add(consultant);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Consultant saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(consultant).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Consultant updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Consultant Visit payment")]
        public ActionResult ConsultantVisits()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var consultants = (from c in hs.Consultants                                   
                                   select new { c }).AsEnumerable()
                         .Select(x => new Consultant
                         {
                             ConsultantID = x.c.ConsultantID,
                             NameDisplay = HtmlHelpers.HtmlHelpers.GetFullName(x.c.FirstName, x.c.MiddleName, x.c.LastName)                                                          
                         }).ToList();
               // return Json(new { data = consultants }, JsonRequestBehavior.AllowGet);
                ViewBag.ConsultantsList = new SelectList(consultants, "ConsultantID", "NameDisplay");
            }

            return View();
        }

        [HttpPost]
        [Description(" - Consultant Payment Save")]
        public ActionResult ConsultantPaymentSave(ConsultantVisit consultant)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (consultant.ConsultantID == 0)
                {
                    db.ConsultantVisits.Add(consultant);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Consultant saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(consultant).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Consultant updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

    }
}