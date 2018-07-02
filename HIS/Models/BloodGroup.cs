using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(BloodGroupMetaData))]
    public partial class BloodGroup
    {
       
    }

    public class BloodGroupMetaData
    {
        [Required(ErrorMessage = "Blood Group is required", AllowEmptyStrings = false)]
        [Display(Name = "Blood Group")]
        public string GroupName { get; set; }
    }
}