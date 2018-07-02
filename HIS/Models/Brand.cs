using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(BrandMetaData))]
    public partial class Brand
    {
    }

    public class BrandMetaData
    {
        [Required(ErrorMessage = "Brand is required", AllowEmptyStrings = false)]
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
    }
}