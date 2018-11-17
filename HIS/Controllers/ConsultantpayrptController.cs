using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class ConsultantpayrptController : Controller
    {
        // GET: Consultantpayrpt
        public ActionResult Index()
        {
            return View();
        }

        
        public JsonResult getConsultants()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var doctors = (from u in dc.Consultants
                               select new { u })
                               .OrderBy(b => b.u.FirstName).AsEnumerable()
                               .Select(x => new Consultant { ConsultantID = x.u.ConsultantID, NameDisplay = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName) }).ToList();
                return Json(new { data = doctors }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult GetReportdata(string doctors, DateTime stdt, DateTime enddt)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = hs.ConsutantPayReport(doctors, stdt, enddt).ToList();
                if(data != null && data.Count() > 0)
                {
                    foreach(var res in data)
                    {
                        res.DateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(res.Consultationdate);
                    }
                }
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}