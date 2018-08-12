using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS
{
    [MetadataType(typeof(RoomMetaData))]
    public partial class Room
    {
        public string DateDisplay { get; set; }
        public string RoomTypeDisplay { get; set; }
        public string RoomStatusDisplay { get; set; }



        public string NextAvailbilityDateFormat()
        {
            return this.NextAvailbility != null ? this.NextAvailbility.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public string GetRoomType()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var data = (from pra in hs.RoomTypes
                            where pra.RoomTypeID == this.RoomTypeID
                            select pra.RoomType1).FirstOrDefault();
                if (data != "" && data != null)
                {
                    this.RoomTypeDisplay = data;
                }
            }
            return this.RoomTypeDisplay != null ? this.RoomTypeDisplay : string.Empty;
        }

        public string GetRoomStatus()
        {
            var status = "Available";
            using (HISDBEntities hs = new HISDBEntities())
            {
                var rooms = (from pra in hs.Rooms
                            where pra.RoomNo == this.RoomNo
                            select pra.RoomBedCapacity).FirstOrDefault();

                var beds     = (from pra in hs.PatientRoomAllocations
                            where pra.RoomNo == this.RoomNo 
                            select pra.BedNo).Count();
                if (rooms == beds) { status = "Occupied"; }
            }
            return status;
        }
    }

        public class RoomMetaData
    {
        [Required(ErrorMessage = "Room Number is Required", AllowEmptyStrings = false)]
        public string RoomNo { get; set; }
        //[Required(ErrorMessage = "Room Type is Required", AllowEmptyStrings = false)]
        //public string RoomType { get; set; }
        [Required(ErrorMessage = "Description is Required", AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Costperday is Required", AllowEmptyStrings = false)]
        public int CostPerDay { get; set; }
        //[Required(ErrorMessage = "Room status is Required", AllowEmptyStrings = false)]
        //public int RoomStatus { get; set; }
        //[Required(ErrorMessage = "Next Avalibility date is Required", AllowEmptyStrings = false)]
        //public Nullable<System.DateTime> NextAvailbility { get; set; }
        [Required(ErrorMessage = "Room beds capacity is Required", AllowEmptyStrings = false)]
        public int RoomBedCapacity { get; set; }
        
    }
}