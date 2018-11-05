using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(IProviderMetaData))]
    public partial class InsuranceProvider
    {
    }

    public class IProviderMetaData
    {
        [Required(ErrorMessage = "Please enter Provider Name", AllowEmptyStrings = false)]
        public string ProviderName { get; set; }

        [Required(ErrorMessage = "Please enter Company Name", AllowEmptyStrings = false)]
        public string Company { get; set; }
    }
}