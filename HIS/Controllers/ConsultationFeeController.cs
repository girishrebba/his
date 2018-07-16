using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.HtmlHelpers;

namespace HIS.Controllers
{
    public class ConsultationFeeController : Controller
    {
        // GET: ConsultationFee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetConsultationFeeList()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var consultationFees = (from cf in hs.ConsultationFees
                                       join ct in hs.ConsultationTypes on cf.ConsultTypeID equals ct.ConsultTypeID
                                       join u in hs.Users on cf.DoctorID equals u.UserID
                                        select new
                                        {
                                            cf,ct,u
                                        }).AsEnumerable()
                                        .Select(x=> new ConsultationFee {
                                            ConsultationID = x.cf.ConsultationID,
                                            DoctorID = x.cf.DoctorID,
                                            ConsultTypeID = x.cf.ConsultTypeID,
                                            Fee = x.cf.Fee,
                                            DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(x.u.FirstName, x.u.MiddleName, x.u.LastName),
                                            ConsultationTypeName = x.ct.ConsultType
                                        })                                
                                        .ToList();

                return Json(new { data = consultationFees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<User> Users = HtmlHelpers.HtmlHelpers.GetUsers();
            List<ConsultationType> ConsultationTypes = HtmlHelpers.HtmlHelpers.GetConsultationTypes();
            if (id == 0)
            {
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                ViewBag.ConsultationTypes = new SelectList(ConsultationTypes, "ConsultTypeID", "ConsultType");
                return View(new ConsultationFee());
            }
            else
            {
                var consultationFee = GetConsultationFee(id);
                if (consultationFee != null)
                {
                    ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay", consultationFee.DoctorID);
                    ViewBag.ConsultationTypes = new SelectList(ConsultationTypes, "ConsultTypeID", "ConsultType", consultationFee.ConsultTypeID);
                    return View(consultationFee);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Get contact by ID
        public ConsultationFee GetConsultationFee(int consulationID)
        {
            ConsultationFee consultationFee = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from cf in dc.ConsultationFees
                         join ct in dc.ConsultationTypes on cf.ConsultTypeID equals ct.ConsultTypeID
                         join u in dc.Users on cf.DoctorID equals u.UserID
                         where cf.ConsultationID.Equals(consulationID)
                         select new
                         {
                             cf,
                             u,
                             ct.ConsultType,
                         }).FirstOrDefault();
                if (v != null)
                {
                    consultationFee = v.cf;
                    consultationFee.DoctorName = HtmlHelpers.HtmlHelpers.GetFullName(v.u.FirstName, v.u.MiddleName, v.u.LastName);
                    consultationFee.ConsultationTypeName = v.ConsultType;
                }
                return consultationFee;
            }
        }

        [HttpPost]
        public ActionResult AddModify(ConsultationFee cf)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (cf.ConsultationID == 0)
                {
                    db.ConsultationFees.Add(cf);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(cf).State = EntityState.Modified;
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
                ConsultationFee cf = db.ConsultationFees.Where(x => x.ConsultationID == id)
                    .FirstOrDefault<ConsultationFee>();
                db.ConsultationFees.Remove(cf);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}