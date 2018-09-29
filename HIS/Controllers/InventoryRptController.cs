using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class InventoryRptController : Controller
    {
        // GET: InventoryRpt
        public ActionResult Index()
        {
            return View();
        }

        // GET: Revenuerpt
        public JsonResult GetReportdata()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = hs.InventoryReport().ToList();
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}