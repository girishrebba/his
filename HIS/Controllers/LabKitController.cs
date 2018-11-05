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
    public class LabKitController : Controller
    {
        [His]
        [Description(" - Lab Kits view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetLabKits()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var lKits = (from lk in hs.LabKits
                                    select new { lk.LKitID, lk.LKitName, lk.LKitCost }).ToList();

                return Json(new { data = lKits }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Lab Kits Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new LabKit());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.LabKits.Where(x => x.LKitID == id).FirstOrDefault<LabKit>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(LabKit lk)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (lk.LKitID == 0)
                {
                    db.LabKits.Add(lk);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(lk).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}