using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class PatientPrescription
    {
        public string MedicineWithDose { get; set; }
        public string DateDisplay { get; set; }
        public string IntakeDisplay { get; set; }
        public string DoctorName { get; set; }
        public string CurrentVsit { get; set; }
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