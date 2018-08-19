using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HIS.Models;
using HIS.Action_Filters;


namespace HIS.Controllers
{
   // [SessionActionFilter]
    public class HomeController : Controller
    {
        // GET: Home
        //[His]
        public ActionResult Index()
        {
            return View();
        }
    }
}