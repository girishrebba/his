using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(LabKitMetaData))]
    public partial class LabKit
    {
    }

    public class LabKitMetaData
    {

        [Required(ErrorMessage = "Please enter Kit Name", AllowEmptyStrings = false)]
        public string LKitName { get; set; }
    }
}