﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class BedTypeController : Controller
    {
        // GET: BedType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBedtypes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var bedtype = (from bg in hs.BedTypes
                                   select new { bg.BedType1, bg.BedTypeID ,bg.BedTypeDescription}).ToList();

                return Json(new { data = bedtype }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new BedType());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.BedTypes.Where(x => x.BedTypeID == id).FirstOrDefault<BedType>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(BedType bgp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bgp.BedTypeID == 0)
                {
                    db.BedTypes.Add(bgp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(bgp).State = EntityState.Modified;
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
                BedType bg = db.BedTypes.Where(x => x.BedTypeID == id).FirstOrDefault<BedType>();
                db.BedTypes.Remove(bg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}