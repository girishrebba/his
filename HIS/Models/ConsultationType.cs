using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(ConsultationTypeMetaData))]
    public partial class ConsultationType
    {
    }

    public class ConsultationTypeMetaData
    {
        [Required(ErrorMessage = "Consult type is required", AllowEmptyStrings = false)]
        [Display(Name = "Consultation")]
        public string ConsultType { get; set; }
    }
}