using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS.Models
{

       // [MetadataType(typeof(PatientTestFileMetaData))]
        public partial class PatientTestFile
        {
            public string TestName { get; set; }
            public string DateDisplay { get; set; }
            public string DoctorName { get; set; }
            public string VisitName { get; set; }
            public string ENMRNO { get; set; }
        public int LTMID { get; set; }
        public int TestID { get; set; }
        public decimal RecordedValues { get; set; }
        public DateTime TestDate { get; set; }
        public string TestImpression { get; set; }
        public string ReportPath { get; set; }

        public int PrescribedBy { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }

    //    public class PatientTestFileMetaData
    //    {
    //        //[Required(ErrorMessage = "Test Name is required", AllowEmptyStrings = false)]
    //        //[Display(Name = "Test Name")]
    //        public string TestName { get; set; }

    //        //[Required(ErrorMessage = "Test Date is required", AllowEmptyStrings = false)]
    //        //[Display(Name = "Test Date")]
    //        public string TestDate { get; set; }

    //        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf|.docx|.doc)$", ErrorMessage = "Only pdf/doc/docx files allowed.")]
    //        public HttpPostedFileBase ReportPath { get; set; }
     

    //    public string ImagePath { get; set; }

    //    public HttpPostedFileBase ImageFile { get; set; }
    //}
    
}