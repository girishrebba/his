using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PatientTestMetaData))]
    public partial class PatientTest
    {
        public string TestName { get; set; }
        public string DateDisplay { get; set; }
        public string DoctorName { get; set; }
    }

    public class PatientTestMetaData
    {

        //[Required(ErrorMessage = "Test Name is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Name")]
        //public string TestName { get; set; }
        [Required(ErrorMessage = "Test Name is required", AllowEmptyStrings = false)]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Test Date is required", AllowEmptyStrings = false)]
        [Display(Name = "Test Date")]
        public string TestDate { get; set; }

        [Required(ErrorMessage = "Prescribed Doctor is required", AllowEmptyStrings = false)]
        [Display(Name = "Prescribed Doctor")]
        public string PrescribedDoctor { get; set; }

        //[Required(ErrorMessage = "Recorded Value is required", AllowEmptyStrings = false)]
        //[Display(Name = "Recorded Value")]
        //public string RecordedValues { get; set; }

        //[Required(ErrorMessage = "Test Impression is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Impression")]
        //public string TestImpression { get; set; }

        //[Required(ErrorMessage = "Test Report file is required", AllowEmptyStrings = false)]
        //[Display(Name = "Report File")]
        //public string ReportPath { get; set; }


        public int SNO { get; set; }
    }
}