using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS.Models;
using HIS.Action_Filters;
using System.ComponentModel;


namespace HIS.Controllers
{
   // [SessionActionFilter]
    public class HomeController : Controller
    {
        // GET: Home
        //[His]
        public ActionResult Index()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                DateTime dt = System.DateTime.Today.AddDays(-1);
                ViewBag.TotalInpatients = hs.InPatients.Count();
                ViewBag.InpatientsToday = hs.InPatients.Where(a => a.Enrolled > dt).Count();
                ViewBag.OnpatientsToday = hs.OutPatients.Where(a => a.Enrolled > dt).Count();
                ViewBag.UnDelivertedTests = hs.LabTestMasters.Where(a => a.IsDelivered == false).Count();
                ViewBag.UnDelivertedPrescription = hs.PrescriptionMasters.Where(a => a.IsDelivered == false).Count();
                ViewBag.UnDelivertedScans = hs.ScanTestMasters.Where(a => a.IsDelivered == false).Count();

                var beds = (from b in hs.Beds
                            join pb in hs.PatientRoomAllocations on b.BedNo equals pb.BedNo
                            where pb.AllocationStatus == true
                            select b.BedNo).ToList();
                var avlbeds = hs.Beds.Select(a => a.BedNo).Except(beds);
                var allbeds = (from b in hs.Beds
                               select new { b }).Where(a => avlbeds.Contains(a.b.BedNo)).AsEnumerable()
                               .Select(x => new Bed { BedNo = x.b.BedNo, RoomNo = x.b.RoomNo }).ToList();

                //var avlrms = hs.Rooms.TakeWhile(a => allbeds.Where(b=>b.RoomNo == a.RoomNo).Select(b => b.RoomNo).Distinct().Count();
                var bedsList = allbeds.GroupBy(test => test.RoomNo)
                   .Select(grp => grp.First())
                   .ToList();

                List<Room> roomsList = new List<Room>();
                foreach(var b in bedsList)
                {
                    var room = hs.Rooms.Where(r => r.RoomNo == b.RoomNo).FirstOrDefault();
                    roomsList.Add(new Room { RoomNo = room.RoomNo, RoomName = room.RoomName });
                }

                ViewBag.AllocatedBeds = beds.Count();
                ViewBag.AvailableBeds = avlbeds.Count();
                ViewBag.AvailableRooms = roomsList.Count();



            }
                return View();
        }
    }
}