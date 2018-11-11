using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(ConsultantMetaData))]
    public partial class Consultant
    {
        public string NameDisplay { get; set; }
        public string DOBDisplay { get; set; }
        public string MaritalStatusDisplay { get; set; }
        public string GenderDisplay { get; set; }
        public string DoctorTypeDisplay { get; set; }
        
    }

    public class ConsultantMetaData
    {
        [Required(ErrorMessage = "Please enter First Name", AllowEmptyStrings = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name", AllowEmptyStrings = false)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter Email", AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Phone", AllowEmptyStrings = false)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
    }
}