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
    public class BrandCategoryController : Controller
    {
        // GET: BrandCategory
        [His]
        [Description(" - Brand Category view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBrandCategories()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var brandCategories = (from bg in hs.BrandCategories
                                       join b in hs.Brands on bg.BrandID equals b.BrandID
                                   select new { bg.CategoryID, b.BrandName, bg.Category }).ToList();

                return Json(new { data = brandCategories }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Brand Category Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            List<Brand> Brands = GetBrands();
            if (id == 0)
            {
                ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName");
                return View(new BrandCategory());
            }
            else
            {
                var brandCategory = GetBrandCategory(id);
                if(brandCategory != null)
                {
                    ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName", brandCategory.BrandID);
                    return View(brandCategory);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Get contact by ID
        public BrandCategory GetBrandCategory(int categoryID)
        {
            BrandCategory brandCategory = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from a in dc.BrandCategories
                         join b in dc.Brands on a.BrandID equals b.BrandID
                         where a.CategoryID.Equals(categoryID)
                         select new
                         {
                             a,
                             b.BrandName,
                         }).FirstOrDefault();
                if (v != null)
                {
                    brandCategory = v.a;
                    brandCategory.BrandName = v.BrandName;
                }
                return brandCategory;
            }
        }

        //Fetch Brands from database
        public List<Brand> GetBrands()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brands = (from b in dc.Brands
                              select new { b.BrandID, b.BrandName })
                              .OrderBy(b=>b.BrandName).AsEnumerable()
                              .Select(x => new Brand { BrandID = x.BrandID, BrandName = x.BrandName }).ToList();
                return brands;
            }
        }


        [HttpPost]
        public ActionResult AddModify(BrandCategory bcg)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bcg.CategoryID == 0)
                {
                    db.BrandCategories.Add(bcg);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(bcg).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Brand Category delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                BrandCategory bcg = db.BrandCategories.Where(x => x.CategoryID == id)
                    .FirstOrDefault<BrandCategory>();
                db.BrandCategories.Remove(bcg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}