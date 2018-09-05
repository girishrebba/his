using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(AdmissionModelMetaData))]
    public partial class AdmissionModel
    {
        public string ENMRNO { get; set; }
        public decimal EstAmount { get; set; }
        public decimal AdvAmount { get; set; }
    }

    public class AdmissionModelMetaData
    {
        [Required(ErrorMessage = "Please enter Estimation Amount", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public string EstAmount { get; set; }

        [Required(ErrorMessage = "Please enter Advance Amount", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public string AdvAmount { get; set; }
    }
}