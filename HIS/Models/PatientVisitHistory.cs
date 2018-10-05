using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PatientVisitHistoryMetaData))]
    public partial class PatientVisitHistory
    {
        public string DOVDisplay { get; set; }
        public string ConsultType { get; set; }
        public string DoctorName { get; set; }
        public string ValidDate { get; set; }
    }

    public class PatientVisitHistoryMetaData
    {
        [Required(ErrorMessage = "Please enter Date", AllowEmptyStrings = false)]
        public System.DateTime DateOfVisit { get; set; }
        [Required(ErrorMessage = "Please choose Consultation", AllowEmptyStrings = false)]
        public int ConsultTypeID { get; set; }
        [Required(ErrorMessage = "Please choose Doctor", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Please adjust Fee", AllowEmptyStrings = false)]
        public decimal Fee { get; set; }
    }
}