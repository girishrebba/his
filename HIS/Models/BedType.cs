using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(BedTypeMetaData))]
    public partial class BedType
    {
        public string BedTypeDisplay { get; set; }
        public string BedDescription { get; set; }
    }

    public class BedTypeMetaData
    {
      
        [Required(ErrorMessage = "Bed Type is Required", AllowEmptyStrings = false)]
        public string BedType1 { get; set; }
        [Required(ErrorMessage = "Description is Required", AllowEmptyStrings = false)]
        public string BedTypeDescription { get; set; }

    }
}