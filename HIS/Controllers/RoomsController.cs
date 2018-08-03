using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class RoomsController : Controller
    {
        // GET: Rooms
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRooms()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var rooms = (from u in hs.Rooms
                             select new { u }).AsEnumerable()
                         .Select(x => new Room
                         {
                             RoomNo = x.u.RoomNo,
                             RoomName = x.u.RoomName,
                              Description = x.u.Description,
                              CostPerDay = x.u.CostPerDay,
                             RoomStatusDisplay = x.u.GetRoomStatus(),
                              DateDisplay = x.u.NextAvailbilityDateFormat(),
                              RoomBedCapacity = x.u.RoomBedCapacity,
                              RoomType= x.u.GetRoomType()
                         }).ToList();


                return Json(new { data = rooms }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new Room());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Rooms.Where(x => x.RoomNo == id).FirstOrDefault<Room>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Room b)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (b.RoomNo == 0)
                {
                    db.Rooms.Add(b);
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
                Room b = db.Rooms.Where(x => x.RoomNo == id)
                    .FirstOrDefault<Room>();
                db.Rooms.Remove(b);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}