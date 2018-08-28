using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class PatientPrescription
    {
        public string ENMRNO { get; set; }
        public int VisitID { get; set; }
        public string MedicineWithDose { get; set; }
        public string DateDisplay { get; set; }
        public string IntakeDisplay { get; set; }
        public string DoctorName { get; set; }
        public string VisitName { get; set; }
        public decimal ItemCost { get; set; }
        public decimal TotalCost { get; set; }
        public List<TestType> TestTypes { get; set; }
        public string[] TestIds { get; set; }
        public decimal Discount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class PPMetaData
    {
        [Required(ErrorMessage = "Please enter Medicine", AllowEmptyStrings = false)]
        public int MedicineWithDose { get; set; }
        
        [Required(ErrorMessage = "Please enter quantity", AllowEmptyStrings = false)]
        public int Quantity { get; set; }
        
        [Required(ErrorMessage = "Please choose frquency", AllowEmptyStrings = false)]
        public int IntakeFrequencyID { get; set; }
    }
}