using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace HIS.Controllers
{
    public class InPatientController : Controller
    {
        // GET: InPatient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetInPatients()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var inPatients = (from ip in hs.InPatients
                                 join user in hs.Users on ip.DoctorID equals user.UserID
                                 select new
                                 {
                                     ip,
                                     user
                                 })
                                 .OrderByDescending(b => b.ip.Enrolled)
                                 .AsEnumerable()
                                 .Select(x => new InPatient {
                                     SNO = x.ip.SNO,
                                     ENMRNO = x.ip.ENMRNO,
                                     Name = x.ip.GetFullName(),
                                     DOBDisplay = x.ip.GetDOBFormat(),
                                     Address = x.ip.GetFullAddress(),
                                     GenderDisplay = x.ip.GetGender(),
                                     Phone = x.ip.Phone,
                                     DoctorName = x.user.GetFullName(),
                                     Purpose = x.ip.Purpose,
                                     EnrolledDisplay = x.ip.GetEnrolledFormat()
                                 }).ToList();

                return Json(new { data = inPatients }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewPatient(int id)
        {
            return View(GetPatientDetails(id));           
        }

        //Fetch Brands from database
        public List<BloodGroup> GetBloodGroups()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var bloodGroups = (from bg in dc.BloodGroups
                              select new { bg.GroupID, bg.GroupName })
                              .OrderBy(b => b.GroupName).AsEnumerable()
                              .Select(x => new BloodGroup { GroupID = x.GroupID, GroupName = x.GroupName }).ToList();
                return bloodGroups;
            }
        }

        //Fetch Brands from database
        public List<User> GetUsers()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var users = (from u in dc.Users
                                   select new { u })
                              .OrderBy(b => b.u.UserName).AsEnumerable()
                              .Select(x => new User { UserID = x.u.UserID, NameDisplay = x.u.GetFullName() }).ToList();
                return users;
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<BloodGroup> BloodGroups = GetBloodGroups();
            List<User> Users = GetUsers();
            if (id == 0)
            {
                ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName");
                ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay");
                InPatient newPatient = new InPatient();
                newPatient.ENMRNO = HtmlHelpers.HtmlHelpers.GetSequencedEnmrNo();
                return View(newPatient);
            }
            else
            {
                var patient = GetPatientDetails(id);
                if (patient != null)
                {
                    ViewBag.BloodGroupsList = new SelectList(BloodGroups, "GroupID", "GroupName", patient.BloodGroupID);
                    ViewBag.Users = new SelectList(Users, "UserID", "NameDisplay", patient.DoctorID);
                    return View(patient);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(InPatient ip)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (ip.SNO == 0)
                {
                    db.InPatients.Add(ip);
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

        public InPatient GetPatientDetails(int id)
        {
            InPatient inPatient = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var inpatient = (from ip in dc.InPatients
                         join user in dc.Users on ip.DoctorID equals user.UserID
                         join bg in dc.BloodGroups on ip.BloodGroupID equals bg.GroupID
                         where ip.SNO.Equals(id)
                         select new { ip, user, bg.GroupName }).FirstOrDefault();
                if (inpatient != null)
                {
                    inPatient = inpatient.ip;
                    inPatient.DoctorName = inpatient.user.GetFullName();
                    inPatient.BloodGroupDisplay = inpatient.GroupName;
                    inPatient.Name = inpatient.ip.GetFullName();
                    inPatient.Address = inpatient.ip.GetFullAddress();
                    inPatient.GenderDisplay = inpatient.ip.GetGender();
                    inPatient.MaritalStatusDisplay = inpatient.ip.GetMaritalStatus();
                    inPatient.DOBDisplay = inpatient.ip.GetDOBFormat();
                    inPatient.EnrolledDisplay = inpatient.ip.GetEnrolledFormat();
                }
                return inPatient;
            }
        }
    }
}