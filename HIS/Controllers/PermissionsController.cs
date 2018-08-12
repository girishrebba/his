using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class PermissionsController : Controller
    {
        // GET: Permissions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPermissions()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var per = (from bg in hs.Permissions
                             select new { bg.Permission_Id, bg.PermissionDescription }).ToList();

                return Json(new { data = per }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Permission());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Permissions.Where(x => x.Permission_Id == id).FirstOrDefault<Permission>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Permission bgp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bgp.Permission_Id == 0)
                {
                    db.Permissions.Add(bgp);
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
                Permission bg = db.Permissions.Where(x => x.Permission_Id == id).FirstOrDefault<Permission>();
                db.Permissions.Remove(bg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}