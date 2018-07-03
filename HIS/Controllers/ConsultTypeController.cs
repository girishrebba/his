using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class ConsultTypeController : Controller
    {
        // GET: ConsultType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetConsultTypes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var consultTypes = (from ct in hs.ConsultationTypes
                                 select new { ct.ConsultTypeID, ct.ConsultType }).ToList();

                return Json(new { data = consultTypes }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new ConsultationType());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.ConsultationTypes.Where(x => x.ConsultTypeID == id).FirstOrDefault<ConsultationType>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(ConsultationType ct)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ct.ConsultTypeID == 0)
                {
                    db.ConsultationTypes.Add(ct);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(ct).State = EntityState.Modified;
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
                ConsultationType ct = db.ConsultationTypes.Where(x => x.ConsultTypeID == id)
                    .FirstOrDefault<ConsultationType>();
                db.ConsultationTypes.Remove(ct);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}