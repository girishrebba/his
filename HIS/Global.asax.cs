using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace HIS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public class Global : System.Web.HttpApplication
        {
            public static List<Permission> permissionList;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Global.permissionList = new List<Permission>();
        }

        protected void Session_End(object sender, EventArgs e)
        {
           Global.permissionList = null;
           // Response.RedirectToRoute("../Login/index");
        }

        
    }
}
