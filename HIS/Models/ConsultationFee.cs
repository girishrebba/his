using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(ConsultationFeeMetaData))]
    public partial class ConsultationFee
    {
        public string DoctorName { get; set; }
        public string ConsultationTypeName { get; set; }
    }

    public class ConsultationFeeMetaData
    {
        [Required(ErrorMessage = "Doctor is required", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Consultation is required", AllowEmptyStrings = false)]
        public int ConsultTypeID { get; set; }
        [Required(ErrorMessage = "Fee is required", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public Nullable<decimal> Fee { get; set; }
    }
}