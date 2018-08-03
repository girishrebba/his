using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(IPHMetaData))]
    public partial class InPatientHistory
    {
        public string DateDisplay { get; set; }
        public string DoctorName { get; set; }
    }

    public class IPHMetaData
    {
        [Required(ErrorMessage = "Please enter Observation", AllowEmptyStrings = false)]
        public string Observations { get; set; }
        [Required(ErrorMessage = "Please enter Observation Date", AllowEmptyStrings = false)]
        public Nullable<DateTime> ObservationDate { get; set; }
        [Required(ErrorMessage = "Please choose doctor", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
    }
}