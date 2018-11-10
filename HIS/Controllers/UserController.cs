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
    public class UserController : Controller
    {
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUsers()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                var users = (from u in hs.Users
                         join ut in hs.UserTypes on u.UserTypeID equals ut.UserTypeID
                         join spl in hs.Specializations on u.SpecializationID equals spl.SpecializationID
                         select new { u, ut, spl }).AsEnumerable()
                         .Select(x=> new User
                         {
                             UserID = x.u.UserID,
                             NameDisplay = x.u.GetFullName(),
                             DOBDisplay = x.u.GetDOBFormat(),
                             GenderDisplay = x.u.GetGender(),
                             Email = x.u.Email,
                             Phone = x.u.Phone,
                             MaritalStatusDisplay = x.u.GetMaritalStatus(),
                             Qualification = x.u.Qualification,
                             UserTypeName = x.ut.UserTypeName,
                             DoctorTypeDisplay = x.spl.DoctorType,
                             StatusDisplay = x.u.GetUserStatus()
                         }).ToList();

                //var users = (from u in hs.VUsers
                //             select u).ToList();

                return Json(new { data = users }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<Specialization> Specializations = GetSpecializations();
            List<UserType> UserTypes = GetUserTypes();
            if (id == 0)
            {
                ViewBag.Specializations = new SelectList(Specializations, "SpecializationID", "DoctorType");
                ViewBag.UserTypes = new SelectList(UserTypes, "UserTypeID", "UserTypeName");
                return View(new User());
            }
            else
            {
                var user = GetUser(id);
                if (user != null)
                {
                    ViewBag.Specializations = new SelectList(Specializations, "SpecializationID", "DoctorType", user.SpecializationID);
                    ViewBag.UserTypes = new SelectList(UserTypes, "UserTypeID", "UserTypeName", user.UserTypeID);
                    return View(user);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        public User GetUser(int userID)
        {
            User user = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from u in dc.Users
                         join ut in dc.UserTypes on u.UserTypeID equals ut.UserTypeID
                         join b in dc.Specializations on u.SpecializationID equals b.SpecializationID
                         where u.UserID.Equals(userID)
                         select new { u }).FirstOrDefault();
                if (v != null)
                {
                    user = v.u;
                    user.DOBDisplay = v.u.GetDOBFormat();
                }
                return user;
            }
        }

        public List<Specialization> GetSpecializations()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var specializations = (from s in dc.Specializations
                              select new { s.SpecializationID, s.DoctorType })
                              .OrderBy(b => b.DoctorType).AsEnumerable()
                              .Select(x => new Specialization { SpecializationID = x.SpecializationID, DoctorType = x.DoctorType }).ToList();
                return specializations;
            }
        }

        public List<UserType> GetUserTypes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var userTypes = (from s in dc.UserTypes
                                       select new { s.UserTypeID, s.UserTypeName })
                              .OrderBy(b => b.UserTypeName).AsEnumerable()
                              .Select(x => new UserType{ UserTypeID = x.UserTypeID, UserTypeName = x.UserTypeName }).ToList();
                return userTypes;
            }
        }

        [HttpPost]
        public ActionResult AddModify(User user)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (user.UserID == 0)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}