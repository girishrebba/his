﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class BrandController : Controller
    {
        // GET: Brand
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrands()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var brands = (from b in hs.Brands
                                 select new { b.BrandID, b.BrandName }).ToList();

                return Json(new { data = brands }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Brand());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Brands.Where(x => x.BrandID == id).FirstOrDefault<Brand>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Brand b)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (b.BrandID == 0)
                {
                    db.Brands.Add(b);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(b).State = EntityState.Modified;
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
                Brand b = db.Brands.Where(x => x.BrandID == id)
                    .FirstOrDefault<Brand>();
                db.Brands.Remove(b);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}