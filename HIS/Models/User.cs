using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string DoctorName { get; set; }
        public string NameDisplay { get; set; }
        public string DOBDisplay { get; set; }
        public string MaritalStatusDisplay { get; set; }
        public string GenderDisplay { get; set; }
        public string StatusDisplay { get; set; }
        public string UserTypeName { get; set; }
        public string DoctorTypeDisplay { get; set; }
        public string GetFullName()
        {
            return string.Format("{0} {1} {2}",
                string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName,
                string.IsNullOrEmpty(this.MiddleName) ? string.Empty : this.MiddleName,
                string.IsNullOrEmpty(this.LastName) ? string.Empty : this.LastName);
        }

        public string GetDOBFormat()
        {
            return this.DOB != null ? this.DOB.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public string GetGender()
        {
            if (this.Gender == 0)
                return "Male";
            else if (this.Gender == 1)
                return "Female";
            else return "Others";
        }

        public string GetMaritalStatus()
        {
            if (this.MaritalStatus == 0)
                return "Single";
            else if (this.MaritalStatus == 1)
                return "Married";
            else return "Others";
        }

        public string GetUserStatus()
        {
            if (this.UserStatus == true)
                return "Active";
            else return "InActive";
        }
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