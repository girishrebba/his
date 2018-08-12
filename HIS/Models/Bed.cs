using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS
{
    [MetadataType(typeof(BedMetaData))]
    public partial class Bed
    {
        public string Roomname { get; set; }
        public string DateDisplay { get; set; }
        public string BedTypeDisplay { get; set; }
        public string BedStatusDisplay { get; set; }



        public string NextAvailbilityDateFormat()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = (from pra in hs.PatientRoomAllocations
                            where pra.RoomNo == this.RoomNo && pra.BedNo == this.BedNo
                            select pra.EndDate).FirstOrDefault();
                if (data.HasValue) {
                    if (data.Value != null) { this.NextAvailbility = data.Value.AddDays(1); }
                }
            }
            return this.NextAvailbility != null ? this.NextAvailbility.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public string GetBedType()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = (from pra in hs.BedTypes
                            where pra.BedTypeID == this.BedTypeID
                            select pra.BedType1).FirstOrDefault();
                if (data != "" && data != null)
                {
                    this.BedTypeDisplay = data;                
                }
            }
            return this.BedTypeDisplay != null ? this.BedTypeDisplay : string.Empty;
        }

        public string GetBedStatus()
        {
            var status = "Available";
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = (from pra in hs.PatientRoomAllocations
                            where pra.RoomNo == this.RoomNo && pra.BedNo == this.BedNo select pra.AllocationID).Count();
                if (data != 0) { status = "Occupied"; }
            }
            return status;
        }
    }

    public class BedMetaData
    {
        [Required(ErrorMessage = "Bed Number is Required", AllowEmptyStrings = false)]
        public string BedNo { get; set; }
        //[Required(ErrorMessage = "Bed Type is Required", AllowEmptyStrings = false)]
        //public string BedType { get; set; }
        [Required(ErrorMessage = "Description is Required", AllowEmptyStrings = false)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Room no is Required", AllowEmptyStrings = false)]
        public int RoomNo { get; set; }
        //[Required(ErrorMessage = "Next Avalibility date is Required", AllowEmptyStrings = false)]
        //public Nullable<System.DateTime> NextAvailbility { get; set; }
        //[Required(ErrorMessage = "Bed status is Required", AllowEmptyStrings = false)]
        //public int BedStatus { get; set; }

    }
}