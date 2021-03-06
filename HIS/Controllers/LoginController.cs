﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(FormCollection form)
        {
            string Username = form["username"].ToString();
            string Password = form["password"].ToString();
            using (HISDBEntities he = new HISDBEntities())
            {

                var user = he.Users.Where(a => a.UserName.Equals(Username) && a.Password.Equals(Password)).FirstOrDefault();

                if (user != null)
                {
                    Session["UserID"] = user.UserID;
                    Session["UserName"] = user.UserName;
                    int cid = Convert.ToInt16(user.UserID);
                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("LoginVal", "The password provided is incorrect.");
                    // return RedirectToAction("../User/Index");
                }
            }
            // If we got this far, something failed, redisplay form
            return View("Index");

        }

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            HIS.MvcApplication.Global.permissionList = null;
            return RedirectToAction("../Login");
        }

    }
}