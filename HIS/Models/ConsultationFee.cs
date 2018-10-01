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
        [Required(ErrorMessage = "Please choose Doctor", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Please choose Consultation", AllowEmptyStrings = false)]
        public int ConsultTypeID { get; set; }
        [Required(ErrorMessage = "Please enter Fee", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public Nullable<decimal> Fee { get; set; }
        [Required(ErrorMessage = "Please enter validity in days", AllowEmptyStrings = false)]
        public int ValidDays { get; set; }
    }
}