using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class IpRevenuerptController : Controller
    {
        // GET: IpRevenuerpt
        public ActionResult Index()
        {
            return View();
        }

        // GET: IpRevenuerpt
        public JsonResult GetDoctors()
        {
            var data = HtmlHelpers.HtmlHelpers.GetDoctors();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        // GET: IpRevenuerpt
        public JsonResult GetReportdata(string doctors, DateTime stdt, DateTime enddt)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = hs.IpRevenueReport1(doctors, stdt, enddt).ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}