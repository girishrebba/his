using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(UserTpeMetaData))]
    public partial class UserType
    {
    }

    public class UserTpeMetaData
    {
        [Required(ErrorMessage = "User Type is required", AllowEmptyStrings = false)]
        [Display(Name = "User Type")]
        public string UserTypeName { get; set; }
    }
}
