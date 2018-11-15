using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class TestType
    {
    }
    public class LabKitViewModel : TestType
    {
        public string LKitName { get; set; }
        public string InputTest { get; set; }
        public decimal LKitCost { get; set; }
        public int LKitID { get; set; }
    }
}