using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(PurchaseOrderMetaData))]
    public partial class PurchaseOrder
    {
        public string MedicineWithDose { get; set; }
        public string OrderDateDisplay { get; set; }
        public string ExpiryDateDisplay { get; set; }
    }

    public class PurchaseOrderMetaData
    {
        [Display(Name = "PO Number:")]
        [Required(ErrorMessage = "Please enter PO number", AllowEmptyStrings = false)]
        public string PONumber { get; set; }
        [Display(Name = "Medicine:")]
        [Required(ErrorMessage = "Please enter Medicine", AllowEmptyStrings = false)]
        public int MedicineWithDose { get; set; }
        [Display(Name = "Ordered Qty:")]
        [Required(ErrorMessage = "Please enter quantity", AllowEmptyStrings = false)]
        public int OrderedQty { get; set; }
        [Display(Name = "Ordered Date:")]
        [Required(ErrorMessage = "Please enter date", AllowEmptyStrings = false)]
        public System.DateTime OrderedDate { get; set; }
        [Display(Name = "Item Cost:")]
        [Required(ErrorMessage = "Please enter item cost", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public Nullable<decimal> PricePerItem { get; set; }
        [Display(Name = "Sheet Cost:")]
        [Required(ErrorMessage = "Please sheet cost", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public Nullable<decimal> PricePerSheet { get; set; }
        [Display(Name = "Expiry Date:")]
        [Required(ErrorMessage = "Please enter expiry date", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    }
}