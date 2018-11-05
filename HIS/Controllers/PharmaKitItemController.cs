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
    public class PharmaKitItemController : Controller
    {
        // GET: PharmaKitItem
        
        [His]
        [Description(" - Pharma Kit Items view page.")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPharmaKitItems()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var pKitItems = (from pk in hs.PharmaKits
                             join pki in hs.PharmaKitItems on pk.PKitID equals pki.PKitItemID
                             join mi in hs.MedicineMasters on pki.MedicineID equals mi.MMID
                             select new { pk, mi, pki }).AsEnumerable()
                             .Select(x => new PharmaKitItem {
                                 PKitItemID = x.pki.PKitItemID,
                                 PKitID = x.pk.PKitID,
                                 KitName = x.pk.PKitName,
                                // MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mi.MedicineName, x.mi.MedDose),
                                 MedicineID = x.mi.MMID
                             }).ToList();
                return Json(new { data = pKitItems }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Pharma Kit Item Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            List<PharmaKit> PharmaKits = HtmlHelpers.HtmlHelpers.GetPharmaKits();
            List<MedicineMaster> Medicines = HtmlHelpers.HtmlHelpers.GetMedicinesWithDose();
            if (id == 0)
            {
                ViewBag.Kits = new SelectList(PharmaKits, "PKitID", "PKitName");
                return View(new PharmaKitItem());
            }
            else
            {
                var pharmaKitItem = GetPharmaKitItem(id);
                if (pharmaKitItem != null)
                {
                    ViewBag.Kits = new SelectList(PharmaKits, "PKitID", "PKitName", pharmaKitItem.PKitID);
                    return View(pharmaKitItem);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Get Pharma Kit ID
        public PharmaKitItem GetPharmaKitItem(int pkititemId)
        {
            PharmaKitItem pharmaKitItem = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from a in dc.PharmaKitItems
                         join b in dc.PharmaKits on a.PKitItemID equals b.PKitID
                         where a.PKitItemID.Equals(pkititemId)
                         select new
                         {
                             a,
                             b.PKitName
                         }).FirstOrDefault();
                if (v != null)
                {
                    pharmaKitItem = v.a;
                    pharmaKitItem.KitName = v.PKitName;
                }
                return pharmaKitItem;
            }
        }

        [HttpPost]
        public ActionResult AddModify(PharmaKitItem pkitItem)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (pkitItem.PKitItemID == 0)
                {
                    db.PharmaKitItems.Add(pkitItem);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(pkitItem).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Pharma Kit Item Delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                PharmaKitItem pKitItem = db.PharmaKitItems.Where(x => x.PKitItemID == id).FirstOrDefault<PharmaKitItem>();
                db.PharmaKitItems.Remove(pKitItem);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}