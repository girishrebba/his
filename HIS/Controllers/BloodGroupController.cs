using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class BloodGroupController : Controller
    {
        // GET: BloodGroup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBloodGroups()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var bloodGroups = (from bg in hs.BloodGroups
                                 select new { bg.GroupID, bg.GroupName }).ToList();

                return Json(new { data = bloodGroups }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new BloodGroup());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.BloodGroups.Where(x => x.GroupID == id).FirstOrDefault<BloodGroup>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(BloodGroup bgp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bgp.GroupID == 0)
                {
                    db.BloodGroups.Add(bgp);
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
                BloodGroup bg = db.BloodGroups.Where(x => x.GroupID == id).FirstOrDefault<BloodGroup>();
                db.BloodGroups.Remove(bg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}