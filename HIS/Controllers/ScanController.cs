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
    public class ScanController : Controller
    {
        // GET: Brand
        [His]
        [Description(" - Scan View Page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetScans()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var scans = HtmlHelpers.HtmlHelpers.GetScans();
                return Json(new { data = scans }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Scan Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Scan());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Scans.Where(x => x.ScanID == id).FirstOrDefault<Scan>());
                }
            }
        }

        [HttpPost]
        [Description(" - Scan Add/Edit page.")]
        public ActionResult AddModify(Scan s)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (s.ScanID == 0)
                {
                    db.Scans.Add(s);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(s).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Scan Delete Page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                Scan s = db.Scans.Where(x => x.ScanID == id)
                    .FirstOrDefault<Scan>();
                db.Scans.Remove(s);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}