using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class RevenuerptController : Controller
    {
        // GET: Revenuerpt
        public ActionResult Index()
        {
            return View();
        }

        // GET: Revenuerpt
        public JsonResult GetDoctors()
        {
            var data= HtmlHelpers.HtmlHelpers.GetDoctors();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // GET: Revenuerpt
        public JsonResult GetReportdata(string doctors, DateTime stdt, DateTime enddt)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = hs.RevenueReport(doctors, stdt, enddt).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
      
    }
}