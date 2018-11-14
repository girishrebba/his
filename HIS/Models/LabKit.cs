using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class LabKit
    {
    }

    //[MetadataType(typeof(LabKitMetaData))]
    //public class LabKitViewModel : LabKit
    //{
    //    public int TestID { get; set; }
    //    public string TestName { get; set; }
    //    public string InputTest { get; set; }
    //}
        public class LabKitMetaData
    {
        [Required(ErrorMessage = "Please enter Kit Name", AllowEmptyStrings = false)]
        public string LKitName { get; set; }
    }
}