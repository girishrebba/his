using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class BedrptController : Controller
    {
        // GET: Bedrpt
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetReportdata()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = hs.BedReport().ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}