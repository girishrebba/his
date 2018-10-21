using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HIS
{
    [MetadataType(typeof(PatientScanMetaData))]
    public partial class PatientScan
    {
        public string ScanName { get; set; }
        public string DateDisplay { get; set; }
        public string DoctorName { get; set; }
        public string VisitName { get; set; }
        public string ENMRNO { get; set; }
        public int PrescribedBy { get; set; }
        public decimal ScanCost { get; set; }
        public decimal Discount { get; set; }
    }


    public class PatientScanMetaData
    {
        //[Required(ErrorMessage = "Test Name is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Name")]
        public string ScanName { get; set; }

        //[Required(ErrorMessage = "Test Date is required", AllowEmptyStrings = false)]
        //[Display(Name = "Test Date")]
        public string ScanDate { get; set; }

        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf|.docx|.doc)$", ErrorMessage = "Only pdf/doc/docx files allowed.")]
        public string ReportPath { get; set; }
    }
}