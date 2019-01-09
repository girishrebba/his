using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using HIS;

//Get requesting user's roles/permissions from database tables...      
public static class His_ExtendedMethods
{
    //public static List<Permission> permissionList;
    public static bool HasRole(this ControllerBase controller, string role)
    {
        bool bFound = false;
        try
        {
            //Check if the requesting user has the specified role...
            //bFound = new TrackerUser(controller.ControllerContext.HttpContext.User.Identity.Name).HasRole(role);            
        }
        catch { }
        return bFound;
    }

    public static bool HasRoles(this ControllerBase controller, string roles)
    {
        bool bFound = false;
        try
        {
            //Check if the requesting user has any of the specified roles...
            //Make sure you separate the roles using ; (ie "Sales Manager;Sales Operator"
            //bFound = new TrackerUser(controller.ControllerContext.HttpContext.User.Identity.Name).HasRoles(roles);
        }
        catch { }
        return bFound;
    }

    public static bool HasPermission(this ControllerBase controller, string permission, decimal _userId)
    {
        bool bFound = false;
        try
        {
            //Check if the requesting user has the specified application permission...
            bFound = new HisUser(_userId).HasPermission(permission);
        }
        catch { }
        return bFound;
    }

    public static bool HasPermission(this ControllerBase controller, string permission)
    {
        bool bFound = false;
        decimal result; 
        try
        {
            if(decimal.TryParse(HttpContext.Current.Session["UserID"].ToString(), out result))
            {
                if (HttpContext.Current.Session["UserRole"] == null)
                {
                    HisUser user = new HisUser(result, true);
                    HttpContext.Current.Session["UserRole"] = user.GetUserPermissions(true);
                    bFound = user.HasPermission(permission, (List<Permission>)HttpContext.Current.Session["UserRole"]);
                }
                else {
                    HisUser user = new HisUser(result);
                    bFound = user.HasPermission(permission, (List<Permission>)HttpContext.Current.Session["UserRole"]);
                }
                //HisUser user = new HisUser(result,true);
                //if(user != null)
                //{
                //    bFound = user.HasPermission(permission);
                //}
            }
            //Check if the requesting user has the specified application permission...
        }
        catch { }
        return bFound;
    }


    public static bool HasPermissionPage(string permission)
    {
        bool bFound = false;
        decimal result;
        try
        {
            if (decimal.TryParse(HttpContext.Current.Session["UserID"].ToString(), out result))
            {
                if (HttpContext.Current.Session["UserRole"] == null)
                {
                    HisUser user = new HisUser(result, true);
                    HttpContext.Current.Session["UserRole"] = user.GetUserPermissions(true);
                    bFound = user.HasPermission(permission, (List<Permission>)HttpContext.Current.Session["UserRole"]);
                }
                else
                {
                    HisUser user = new HisUser(result);
                    bFound = user.HasPermission(permission, (List<Permission>)HttpContext.Current.Session["UserRole"]);
                }                
            }
            //Check if the requesting user has the specified application permission...
        }
        catch { }
        return bFound;
    }

    //public static bool IsSysAdmin(this ControllerBase controller)
    //{        
    //    bool bIsSysAdmin = false;
    //    try
    //    {
    //        //Check if the requesting user has the System Administrator privilege...
    //        var userID = HttpContext.Current.Session["UserID"].ToString();
    //        bIsSysAdmin = new HisUser(Convert.ToInt64(userID)).isSuperAdmin;
    //    }
    //    catch { }
    //    return bIsSysAdmin;
    //}
}
