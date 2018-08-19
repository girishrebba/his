using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Reflection;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class PermissionsController : Controller
    {
        // GET: Permissions
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPermissions()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var per = (from bg in hs.Permissions
                           select new { bg.Permission_Id, bg.PermissionDescription,bg.PermissionStatus }).ToList();

                return Json(new { permissiondata = per }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Getcontrollers()
        {

            Assembly asm = Assembly.GetAssembly(typeof(HIS.MvcApplication));
            var controlleractionlist = asm.GetTypes().
            SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            .Where(d => d.ReturnType.Name == "ActionResult").Select(n => new
            {
                Controller = n.DeclaringType?.Name.Replace("Controller", "")
            }).Distinct().ToList();  

           return Json(new { data = controlleractionlist.ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            Assembly asm = Assembly.GetAssembly(typeof(HIS.MvcApplication));

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name })
                    .OrderBy(x => x.Controller).ToList();


            if (id == 0)
                return View(new Permission());
            else
            {
                using (HISDBEntities db = new HISDBEntities())
                {
                    return View(db.Permissions.Where(x => x.Permission_Id == id).FirstOrDefault<Permission>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddModify(Permission bgp)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (bgp.Permission_Id == 0)
                {
                    db.Permissions.Add(bgp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(bgp).State = EntityState.Modified;
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
                Permission bg = db.Permissions.Where(x => x.Permission_Id == id).FirstOrDefault<Permission>();
                db.Permissions.Remove(bg);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult SaveGrid(string[] chkboxes)
        {
            using (HISDBEntities db = new HISDBEntities())
            {

                //db.Database.ExecuteSqlCommand("TRUNCATE TABLE UserPermission");
                //db.SaveChanges();
                string chk = "'";

                foreach (var i in chkboxes)
                {
                    chk += i +"','";
                    //string[] data = i.Split('_');
                    Permission p = db.Permissions.Where(x => x.PermissionDescription == i).FirstOrDefault<Permission>();
                    if (p != null)
                    {
                        p.PermissionStatus = true;
                        db.Entry(p).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else {
                        Permission pa = new Permission();
                        pa.PermissionDescription = i;
                        pa.PermissionStatus = true;
                        db.Permissions.Add(pa);
                        db.SaveChanges();
                    }
                }

                db.Database.ExecuteSqlCommand("update Permissions set PermissionStatus=0 where PermissionDescription not in (" + chk.Substring(0,chk.Length-2) + ")");
                db.SaveChanges();

                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}