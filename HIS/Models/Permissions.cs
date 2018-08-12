using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
   
    [MetadataType(typeof(PermissionsMetaData))]
    public partial class Permissions
    {
    }


    public class PermissionsMetaData
    {
       
        [Required(ErrorMessage = "Permission Description is required", AllowEmptyStrings = false)]
        [Display(Name = "Permission Description")]
        public string PermissionDescription { get; set; }
    }
}