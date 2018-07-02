using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
    }

    public class UserMetaData
    {
        [Required(ErrorMessage = "First Name required", AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name required", AllowEmptyStrings = false)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password required", AllowEmptyStrings = false)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email required", AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone required", AllowEmptyStrings = false)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "User Type required", AllowEmptyStrings = false)]
        [Display(Name = "User Type")]
        public int UserTypeID { get; set; }
    }
}