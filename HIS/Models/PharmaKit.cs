using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HIS
{

    public partial class PharmaKit
    {
        
    }

    [MetadataType(typeof(PharmaKitMetaData))]
    public class PharmaKitViewModel : PharmaKit
    {
        public int MedicineID { get; set; }
        public string MedicineWithDose { get; set; }
        public string InputMedicine { get; set; }
        public int InputQuantity { get; set; }
        public int Quantity { get; set; }
    }
    public class PharmaKitMetaData
    {
        [Required(ErrorMessage = "Please enter Package Name", AllowEmptyStrings = false)]
        public string PKitName { get; set; }

        [Required(ErrorMessage = "Please enter Package Cost", AllowEmptyStrings = false)]
        public string PKitCost { get; set; }

        [Required(ErrorMessage = "Please enter quantity", AllowEmptyStrings = false)]
        public int Quantity { get; set; }
        
    }
}