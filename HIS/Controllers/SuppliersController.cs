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
    public class SuppliersController : Controller
    {
        [His]
        [Description(" - Suppliers view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSuppliers()
        {
            return Json(new { data = HtmlHelpers.HtmlHelpers.GetSuppliers() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Description(" - Suppliers Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Supplier());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault<Supplier>());
                }
            }
        }

        [HttpPost]
        [Description(" - Suppliers Add/Edit page.")]
        public ActionResult AddModify(Supplier p)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (p.SupplierID == 0)
                {
                    db.Suppliers.Add(p);
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
        [Description(" - Supplier Delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                Supplier splr = db.Suppliers.Where(x => x.SupplierID == id).FirstOrDefault<Supplier>();
                db.Suppliers.Remove(splr);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}