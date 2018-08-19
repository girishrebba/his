using HIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;

public class HisUser
{
    public decimal User_Id { get; set; }
    public int UserTypeid { get; set; }
    public string Username { get; set; }
    private UserPermission _UserPermissions { get; set; }
    private List<Permission> permissionList = new List<Permission>();
    //public bool isSuperAdmin
    //{
    //    get { return this.UserType.ToLower() == "s"; }
    //}

    public HisUser(decimal _userId)
    {
        this.User_Id = _userId;
        GetUserDetails();
    }

    private void GetUserDetails()
    {
        using (HISDBEntities _data = new HISDBEntities())
        {
            User _user = _data.Users.Where(u => u.UserID == this.User_Id).FirstOrDefault();
            if (_user != null)
            {
                this.User_Id = _user.UserID;
                this.Username = _user.UserName;
                this.UserTypeid = _user.UserTypeID;
               
                    GetUserPermissions();
             
            }
        }
    }

    private void GetUserPermissions()
    {
        using (HISDBEntities _data = new HISDBEntities())
        {
            var _perList = _data.UserPermissions.Where(u => u.UserTypeID == this.UserTypeid).ToList();
            if (_perList != null)
            {
              
                    foreach (var _permissionid in _perList)
                    {
                    var _permission = _data.Permissions.Where(u => u.Permission_Id == _permissionid.PermissionID).FirstOrDefault();
                    this.permissionList.Add(new Permission { Permission_Id = _permission.Permission_Id, PermissionDescription = _permission.PermissionDescription });
                    }                  
                
            }
        }
    }


    public bool HasPermission(string requiredPermission)
    {
        bool bFound = false;
        foreach (Permission role in this.permissionList)
        {
            if (role.PermissionDescription.ToLower() == requiredPermission.ToLower())
            {
                bFound = true;
                break;
            }
        }
        return bFound;
    }
}


public class UserPermission
{
    public decimal UserId { get; set; }
    public string UserType { get; set; }
    public List<Permission> Permissions = new List<Permission>();
}