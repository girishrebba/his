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
    public class BrandSubCategoriesController : Controller
    {
        [His]
        [Description(" - Sub Categories view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSubCategories()
        {
            return Json(new { data = HtmlHelpers.HtmlHelpers.GetSubCategories() }, JsonRequestBehavior.AllowGet);  
        }

        [HttpGet]
        [Description(" - Sub Category Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new BrandSubCategory());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.BrandSubCategories.Where(x => x.SubCategoryID == id).FirstOrDefault<BrandSubCategory>());
                }
            }
        }

        [HttpPost]
        [Description(" - Sub Category Add/Edit page.")]
        public ActionResult AddModify(BrandSubCategory p)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (p.SubCategoryID == 0)
                {
                    db.BrandSubCategories.Add(p);
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
        [Description(" - Sub Category Delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                BrandSubCategory scat = db.BrandSubCategories.Where(x => x.SubCategoryID == id).FirstOrDefault<BrandSubCategory>();
                db.BrandSubCategories.Remove(scat);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}