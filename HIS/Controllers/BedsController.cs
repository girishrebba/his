﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using HIS.Action_Filters;
using System.ComponentModel;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class BedsController : Controller
    {
        // GET: Beds
        [His]
        [Description(" -  - Beds master view permission.")]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetBeds()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var beds = (from u in hs.Beds
                            join r in hs.Rooms on u.RoomNo equals r.RoomNo
                             select new { u, r }).AsEnumerable()
                         .Select(x => new Bed
                         {
                             Roomname = x.r.RoomName,
                             BedNo = x.u.BedNo,
                             BedName = x.u.BedName,
                             Description = x.u.Description,                            
                             DateDisplay = x.u.NextAvailbilityDateFormat(),
                             BedStatusDisplay = x.u.GetBedStatus(),
                             BedTypeDisplay = x.u.GetBedType()
                         }).ToList();


                return Json(new { data = beds }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" -  - Beds master Add/Edit permission.")]
        public ActionResult AddModify(int id = 0)
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                List<BedType> beds = (from u in dc.BedTypes
                                      select new { u })
                           .OrderBy(b => b.u.BedTypeID).AsEnumerable()
                           .Select(x => new BedType { BedTypeID = x.u.BedTypeID, BedType1 = x.u.BedType1 }).ToList();
                ViewBag.BedTypeList = new SelectList(beds, "BedTypeID", "BedType1");
            }
            List<Room> Rooms = GetRoomIds();
            if (id == 0)
            {
                ViewBag.Rooms = new SelectList(Rooms, "RoomNo", "RoomName");
                return View(new Bed());
            }
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    var bed = db.Beds.Where(x => x.BedNo == id).FirstOrDefault<Bed>();
                    ViewBag.Rooms = new SelectList(Rooms, "RoomNo", "RoomName", bed.RoomNo);

                    return View(bed);
                }
                
               
            }
        }

        [HttpPost]
        [Description(" -  - Beds master Add/Edit permission.")]
        public ActionResult AddModify(Bed b)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (b.BedNo == 0)
                {
                    db.Beds.Add(b);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    try
                    {
                        db.Entry(b).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
            }
        }

        [HttpPost]
        [Description(" -  - Beds master delete permission.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                Bed b = db.Beds.Where(x => x.BedNo == id)
                    .FirstOrDefault<Bed>();
                db.Beds.Remove(b);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


        //Fetch rooms from database
        public List<Room> GetRoomIds()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                //var roomslist = (from u in dc.Beds
                //                 join r in dc.Rooms on u.RoomNo equals r.RoomNo select r).ToList();
                var room = (from u in dc.Rooms                            
                             select new { u })
                              .OrderBy(b => b.u.RoomNo).AsEnumerable()
                              .Select(x => new Room { RoomNo = x.u.RoomNo, RoomName = x.u.RoomName }).ToList();
                return room;
            }
        }
    }
}