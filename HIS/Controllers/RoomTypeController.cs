using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class RoomTypeController : Controller
    {
        // GET: RoomType
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Getroomtypes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var roomtype = (from bg in hs.RoomTypes
                               select new { bg.RoomTypeID, bg.RoomType1 }).ToList();

                return Json(new { data = roomtype }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new RoomType());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.RoomTypes.Where(x => x.RoomTypeID == id).FirstOrDefault<RoomType>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(RoomType bgp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bgp.RoomTypeID == 0)
                {
                    db.RoomTypes.Add(bgp);
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
                RoomType bg = db.RoomTypes.Where(x => x.RoomTypeID == id).FirstOrDefault<RoomType>();
                db.RoomTypes.Remove(bg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}