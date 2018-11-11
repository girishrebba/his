using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel;
using HIS.Models;

namespace HIS.Controllers
{
    public class ConsultantVisitsController : Controller
    {
        // GET: ConsultantVisits
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetConsultants()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var consultants = (from cv in hs.ConsultantVisits
                                   join c in hs.Consultants on cv.ConsultantID equals c.ConsultantID
                                   select new { c, cv }).AsEnumerable()
                         .Select(x => new ConsultantVisits
                         {
                             ConsultantVisitId =x.cv.ConsultantVisitId,
                             ConsultantID = x.c.ConsultantID,
                             Consultantname = HtmlHelpers.HtmlHelpers.GetFullName(x.c.FirstName, x.c.MiddleName, x.c.LastName),
                             Consultationdt = HtmlHelpers.HtmlHelpers.DateFormat(x.cv.Consultationdate),
                             Consultationamt = x.cv.Amount
                         }).ToList();
                return Json(new { data = consultants }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<Consultant> consultantlist;
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
                //ViewBag.ConsultantsList = new SelectList(consultants, "ConsultantID", "NameDisplay");
                consultantlist = consultants;
            }
            
            if (id == 0)
            {
                ViewBag.ConsultantsList = new SelectList(consultantlist, "ConsultantID", "NameDisplay");
                return View(new ConsultantVisit());
            }
            else
            {
                var user = GetConsultant(id);
                if (user != null)
                {
                    ViewBag.Specializations = new SelectList(consultantlist, "ConsultantID", "NameDisplay", user.ConsultantID);
                    return View(user);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public ConsultantVisits GetConsultant(int consultID)
        {
            ConsultantVisits consultant = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from c in dc.ConsultantVisits                         
                         where c.ConsultantID.Equals(consultID)
                         select new { c }).FirstOrDefault();
                if (v != null)
                {
                    consultant.ConsultantVisitId = v.c.ConsultantVisitId;
                    consultant.Consultationdt = HtmlHelpers.HtmlHelpers.DateFormat(v.c.Consultationdate);
                }
                return consultant;
            }
        }

        [HttpPost]
        public ActionResult AddModify(ConsultantVisit consultant)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (consultant.ConsultantVisitId == 0)
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