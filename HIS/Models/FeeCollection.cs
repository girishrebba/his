using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(FeeCollectionMetaData))]
    public partial class FeeCollection
    {
        public string PaidDateDisplay { get; set; }
    }

    public class FeeCollectionMetaData
    {
        [Required(ErrorMessage = "Please enter Amount", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Please choose Pay Mode", AllowEmptyStrings = false)]
        public int PaymentMode { get; set; }
    }
}