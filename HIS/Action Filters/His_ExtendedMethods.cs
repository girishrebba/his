using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

//Get requesting user's roles/permissions from database tables...      
public static class His_ExtendedMethods
{
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
                HisUser user = new HisUser(result);
                if(user != null)
                {
                    bFound = user.HasPermission(permission);
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
