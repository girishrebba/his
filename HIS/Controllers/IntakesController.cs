using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;


namespace HIS.Controllers
{
    [SessionActionFilter]
    public class IntakesController : Controller
    {
        // GET: Intakes
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetIntakes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var intakes = (from ifs in hs.IntakeFrequencies
                                       select new { ifs.FrequencyID, ifs.Frequency }).ToList();

                return Json(new { data = intakes }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
                return View(new IntakeFrequency());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.IntakeFrequencies.Where(x => x.FrequencyID == id).FirstOrDefault<IntakeFrequency>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(IntakeFrequency ifs)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ifs.FrequencyID == 0)
                {
                    db.IntakeFrequencies.Add(ifs);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(ifs).State = EntityState.Modified;
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
                IntakeFrequency ifs = db.IntakeFrequencies.Where(x => x.FrequencyID == id)
                    .FirstOrDefault<IntakeFrequency>();
                db.IntakeFrequencies.Remove(ifs);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}