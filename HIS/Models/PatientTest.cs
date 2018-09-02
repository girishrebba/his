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
        public string VisitName { get; set; }
        public string ENMRNO { get; set; }
        public int PrescribedBy { get; set; }
        public decimal TestCost { get; set; }
        public decimal Discount { get; set; }
    }

    public class PatientTestMetaData
    {
        //[Required(ErrorMessage = "Test Name is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Name")]
        public string TestName { get; set; }

        //[Required(ErrorMessage = "Test Date is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Date")]
        public string TestDate { get; set; }

        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf|.docx|.doc)$", ErrorMessage = "Only pdf/doc/docx files allowed.")]
        public HttpPostedFileBase ReportPath { get; set; }
    }
}