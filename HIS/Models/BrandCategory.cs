using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(BrandCategoryMetaData))]
    public partial class BrandCategory
    {
        public string BrandName { get; set; }

    }

    public class BrandCategoryMetaData
    {
        [Required(ErrorMessage = "Category is required", AllowEmptyStrings = false)]
        [Display(Name = "Category")]
        public string Category { get; set; }
        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = "Brand is required", AllowEmptyStrings = false)]
        public int BrandID { get; set; }
    }
}
