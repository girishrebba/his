using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(IntakeFrequencyMetaData))]
    public partial class IntakeFrequency
    {
    }

    public class IntakeFrequencyMetaData
    {
        [Required(ErrorMessage = "Intake Frequency is required", AllowEmptyStrings = false)]
        [Display(Name = "Intake Frequency")]
        public string Frequency { get; set; }
    }
}