using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(ScanCategoryMetaData))]
    public partial class ScanCategory
    {
        public string ScanName { get; set; }
    }

    public class ScanCategoryMetaData
    {
        [Required(ErrorMessage = "Please enter category", AllowEmptyStrings = false)]
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Scan Name")]
        [Required(ErrorMessage = "Please choose scan", AllowEmptyStrings = false)]
        public int ScanID { get; set; }
    }
}