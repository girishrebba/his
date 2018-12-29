using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class TestType
    {
    }
    [MetadataType(typeof(LabKitMetaData))]
    public class LabKitViewModel : TestType
    {
        public string LKitName { get; set; }
        public string InputTest { get; set; }
        public decimal LKitCost { get; set; }
        public int LKitID { get; set; }
    }

    public class LabKitMetaData
    {
        [Required(ErrorMessage = "Please enter Kit Name", AllowEmptyStrings = false)]
        public string LKitName { get; set; }

        [Required(ErrorMessage = "Please enter Test Cost", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public string LKitCost { get; set; }
    }
}