using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class SpecializationController : Controller
    {
        // GET: Specialization
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetSpecializations()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var specializations = (from spl in hs.Specializations
                                   select new { spl.SpecializationID, spl.DoctorType }).ToList();

                return Json(new { data = specializations }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Specialization());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Specializations.Where(x => x.SpecializationID == id).FirstOrDefault<Specialization>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Specialization spl)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (spl.SpecializationID == 0)
                {
                    db.Specializations.Add(spl);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(spl).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                Specialization spl = db.Specializations.Where(x => x.SpecializationID == id)
                    .FirstOrDefault<Specialization>();
                db.Specializations.Remove(spl);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}