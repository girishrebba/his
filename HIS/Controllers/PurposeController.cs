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
    public class PurposeController : Controller
    {
        // GET: Purpose
        [His]
        [Description(" - Purpose view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPurpose()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var purpose = (from p in hs.Purposes
                                   select new { p.PurposeID, p.PurposeName }).ToList();

                return Json(new { data = purpose }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Purpose Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Purpose());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Purposes.Where(x => x.PurposeID == id).FirstOrDefault<Purpose>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Purpose p)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (p.PurposeID == 0)
                {
                    db.Purposes.Add(p);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Purpose Delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                Purpose prpse = db.Purposes.Where(x => x.PurposeID == id).FirstOrDefault<Purpose>();
                db.Purposes.Remove(prpse);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}