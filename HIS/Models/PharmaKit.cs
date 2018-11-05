using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PharmaKitMetaData))]
    public partial class PharmaKit
    {
    }

    public class PharmaKitMetaData
    {

        [Required(ErrorMessage = "Please enter Kit Name", AllowEmptyStrings = false)]
        public string PKitName { get; set; }
    }
}