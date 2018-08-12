using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    
    [MetadataType(typeof(RolePermissionsMetaData))]
    public partial class RolePermissions
    {
    }


    public class RolePermissionsMetaData
    {
       
        public string RoleID { get; set; }

        
        public string PermissionID { get; set; }
    }

}