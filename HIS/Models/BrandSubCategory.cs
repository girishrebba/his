using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(SubCategoryMetaData))]
    public partial class BrandSubCategory
    { 

    }

    public class SubCategoryMetaData
    {
        [Required(ErrorMessage = "Sub category is required", AllowEmptyStrings = false)]
        public string SubCategory { get; set; }
    }
}