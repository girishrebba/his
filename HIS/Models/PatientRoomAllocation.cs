using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PatientRoomAllocationMetaData))]
    public partial class PatientRoomAllocation
    {
        public PatientRoomAllocation() {
            this.AllocationStatus = true;
        }
        //public string FromDtFormat()
        //{
        //    return this.FromDate != null ? this.FromDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        //}
        public string EndDtFormat()
        {
            return this.EndDate != null ? this.EndDate.Value.ToString("MM/dd/yyyy") : string.Empty;
        }
    }

    public class PatientRoomAllocationMetaData
    {
        [Required(ErrorMessage = "ENMRNO is Required", AllowEmptyStrings = false)]
        public string ENMRNO { get; set; }
        [Required(ErrorMessage = "Room No is Required", AllowEmptyStrings = false)]
        public string RoomNo { get; set; }
        [Required(ErrorMessage = "Bed is Required", AllowEmptyStrings = false)]
        public string BedNo { get; set; }
        [Required(ErrorMessage = "From date is Required", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> FromDate { get; set; }
       // [Required(ErrorMessage = "End is Required", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> EndDate { get; set; }
      //  [Required(ErrorMessage = "Blood Group is Required", AllowEmptyStrings = false)]
        //[Required(ErrorMessage = "Allocation Status is Required", AllowEmptyStrings = false)]
        public int AllocationStatus { get; set; }
    }
 }