using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public partial class PharmaKitItem
    {
        public string MedicineWithDose { get; set; }
        public string KitName { get; set; }

    }

    public class PharmaKitItemMetaData
    {
        [Display(Name = "Medicine:")]
        [Required(ErrorMessage = "Please enter Medicine", AllowEmptyStrings = false)]
        public int MedicineWithDose { get; set; }
        [Display(Name = "Quantity:")]
        [Required(ErrorMessage = "Please enter quantity", AllowEmptyStrings = false)]
        public int Quantity { get; set; }
    }
}