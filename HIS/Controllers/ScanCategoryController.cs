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
    public class ScanCategoryController : Controller
    {
        // GET: ScanCategory
        [His]
        [Description(" - Scan Category View Page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetScanCategories()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var scanCategories = (from sc in hs.ScanCategories
                                       join s in hs.Scans on sc.ScanID equals s.ScanID
                                       select new { sc.SCID, s.ScanName, sc.Category }).ToList();

                return Json(new { data = scanCategories }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Scan Category Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            List<Scan> Scans = HtmlHelpers.HtmlHelpers.GetScans();
            if (id == 0)
            {
                ViewBag.Scans = new SelectList(Scans, "ScanID", "ScanName");
                return View(new ScanCategory());
            }
            else
            {
                var scanCategory = GetScanCategory(id);
                if (scanCategory != null)
                {
                    ViewBag.Scans = new SelectList(Scans, "ScanID", "ScanName", scanCategory.ScanID);
                    return View(scanCategory);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Get contact by ID
        public ScanCategory GetScanCategory(int categoryID)
        {
            ScanCategory scanCategory = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from a in dc.ScanCategories
                         join b in dc.Scans on a.ScanID equals b.ScanID
                         where a.SCID.Equals(categoryID)
                         select new
                         {
                             a,
                             b.ScanName,
                         }).FirstOrDefault();
                if (v != null)
                {
                    scanCategory = v.a;
                    scanCategory.ScanName = v.ScanName;
                }
                return scanCategory;
            }
        }

        [HttpPost]
        [Description(" - Scan Category Add/Edit page.")]
        public ActionResult AddModify(ScanCategory scg)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (scg.SCID == 0)
                {
                    db.ScanCategories.Add(scg);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(scg).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Scan Category Delete Page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                ScanCategory scg = db.ScanCategories.Where(x => x.SCID == id)
                    .FirstOrDefault<ScanCategory>();
                db.ScanCategories.Remove(scg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}