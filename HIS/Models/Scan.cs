using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(ScanMetaData))]
    public partial class Scan
    {
    }

    public class ScanMetaData
    {
        [Required(ErrorMessage = "Please enter Scan", AllowEmptyStrings = false)]
        [Display(Name = "Scan")]
        public string ScanName { get; set; }
    }
}