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
                ViewBag.UnDeliverted = hs.LabTestMasters.Where(a => a.IsDelivered == false).Count();
                
                var beds = (from b in hs.Beds
                            join pb in hs.PatientRoomAllocations on b.BedNo equals pb.BedNo
                            where pb.AllocationStatus == true
                            select b.BedNo).ToList();
                var avlbeds = hs.Beds.Select(a => a.BedNo).Except(beds);
               // var avlrms = hs.Beds.TakeWhile(a => avlbeds.Contains(a.BedNo)).Select(b=>b.RoomNo).Distinct().Count();
                ViewBag.AllocatedBeds = beds.Count();
                ViewBag.AvailableBeds = avlbeds.Count();
                ViewBag.AvailableRooms = 10;



            }
                return View();
        }
    }
}