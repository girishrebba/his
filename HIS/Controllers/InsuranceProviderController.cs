using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;
using System.ComponentModel;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class InsuranceProviderController : Controller
    {
        [His]
        [Description(" - Insurance Provider view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInsuranceProvider()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var insProviders = (from ip in hs.InsuranceProviders
                                   select new { ip.ProviderID, ip.ProviderName,ip.Company }).ToList();

                return Json(new { data = insProviders }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Insurance Provider Add/Edit page.")]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new InsuranceProvider());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.InsuranceProviders.Where(x => x.ProviderID == id).FirstOrDefault<InsuranceProvider>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(InsuranceProvider ip)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ip.ProviderID == 0)
                {
                    db.InsuranceProviders.Add(ip);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(ip).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        [Description(" - Insurance Provider Delete page.")]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                InsuranceProvider iProvider = db.InsuranceProviders.Where(x => x.ProviderID == id).FirstOrDefault<InsuranceProvider>();
                db.InsuranceProviders.Remove(iProvider);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}