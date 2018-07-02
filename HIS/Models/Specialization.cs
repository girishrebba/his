using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(SpecializationMetaData))]
    public partial class Specialization
    {
    }

    public class SpecializationMetaData
    {
        [Required(ErrorMessage = "Specialization is required", AllowEmptyStrings = false)]
        [Display(Name = "Specialzation")]
        public string DoctorType { get; set; }
    }
}