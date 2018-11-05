using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PurposeMetaData))]
    public partial class Purpose
    {

    }

    public class PurposeMetaData
    {
        [Required(ErrorMessage = "Purpose is required", AllowEmptyStrings = false)]
        public string PurposeName { get; set; }
    }
}