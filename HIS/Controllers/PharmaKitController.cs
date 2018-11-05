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
    public class PharmaKitController : Controller
    {
        [His]
        [Description(" - Pharma Kits view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPharmaKits()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var pKits = (from pk in hs.PharmaKits
                             select new { pk.PKitID, pk.PKitName, pk.PKitCost }).ToList();

                return Json(new { data = pKits }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Pharma Kits Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new PharmaKit());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.PharmaKits.Where(x => x.PKitID == id).FirstOrDefault<PharmaKit>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(PharmaKit pk)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (pk.PKitID == 0)
                {
                    db.PharmaKits.Add(pk);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(pk).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}