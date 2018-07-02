using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class UserTypeController : Controller
    {
        // GET: UserType
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserTypes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var userTypes = (from ut in hs.UserTypes
                          select new { ut.UserTypeID, ut.UserTypeName}).ToList();
                
                return Json(new { data = userTypes}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new UserType());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.UserTypes.Where(x => x.UserTypeID== id).FirstOrDefault<UserType>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(UserType utp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (utp.UserTypeID == 0)
                {
                    db.UserTypes.Add(utp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(utp).State = EntityState.Modified;
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
                UserType ut = db.UserTypes.Where(x => x.UserTypeID == id)
                    .FirstOrDefault<UserType>();
                db.UserTypes.Remove(ut);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}