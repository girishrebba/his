using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HIS.Models
{
    [MetadataType(typeof(RoomTypeMetaData))]
    public partial class RoomType
    {
        public int RoomTypeID { get; set; }
    }


    public class RoomTypeMetaData
    {

        [Required(ErrorMessage = "Room Type is Required", AllowEmptyStrings = false)]
        public string RoomType { get; set; }
      

    }
}