using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS.Models
{
    [MetadataType(typeof(ConsultantVisitsMetaData))]
    public partial class ConsultantVisits
    {
        public string Consultantname { get; set; }
        public string ConsultationTypeName { get; set; }
        public string NameDisplay { get; set; }
        public int ConsultantID { get; set; }
        public string Consultationdt { get; set; }
        public Nullable<decimal> Consultationamt { get; set; }
        public int ConsultantVisitId { get; set; }
    }

    public class ConsultantVisitsMetaData
    {
        [Required(ErrorMessage = "Please choose Consultant", AllowEmptyStrings = false)]
        public int ConsultantID { get; set; }
        
        [Required(ErrorMessage = "Please enter Fee amount", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public Nullable<decimal> Amount { get; set; }
        [Required(ErrorMessage = "Please enter validity in days", AllowEmptyStrings = false)]
        public int Consultationdate { get; set; }
    }
}