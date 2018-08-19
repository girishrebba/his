using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class RolePermissionsController : Controller
    {
        // GET: RolePermissions
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetRoles()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var roles = (from bg in hs.UserTypes
                             orderby bg.UserTypeID
                             select new { bg.UserTypeID, bg.UserTypeName }).ToList();

                return Json(new { data = roles }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPermissions()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {

                List<object> PermissionData = new List<object>();
                var roles = (from bg in hs.UserTypes
                             orderby bg.UserTypeID
                             select new { bg.UserTypeID, bg.UserTypeName }).ToList();

                var per = (from bg in hs.Permissions
                           select bg).ToList();

                ////var userper = (from bg in hs.UserPermissions
                ////               orderby bg.UserTypeID
                ////               select new { bg.UserTypeID, bg.PermissionID }).ToList();

                foreach (var permission in per)
                {
                    var userper = (from bg in hs.UserPermissions
                                   where bg.PermissionID == permission.Permission_Id
                                   orderby bg.UserTypeID
                                   select bg.UserTypeID).ToList();
                    for (int i=0; i < roles.Count() ; i++)
                    {
                        if (userper.Contains(roles[i].UserTypeID))
                        {
                            //if (userper[i] == roles[i].UserTypeID)
                            //{
                                PermissionData.Add(new object[] { permission,  true });
                            //}
                            //else
                            //{
                            //    PermissionData.Add(new object[] { permission,  false });
                            //}
                            
                        }
                        else
                        {
                            PermissionData.Add(new object[] { permission,  false });
                        }
                    }
                        

                }



                return Json(new { dt = per, userpermission= PermissionData }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult test(string[] chkboxes)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE UserPermission");             
                db.SaveChanges();

                
                foreach (var i in chkboxes) {
                    string[] data = i.Split('_');
                    UserPermission up = new UserPermission();
                    up.PermissionID = Convert.ToInt16(data[0]);
                    up.UserTypeID = Convert.ToInt16(data[1]);
                    db.UserPermissions.Add(up);                    
                }
                db.SaveChanges();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}