using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(MedicineInventoryMetaData))]
    public partial class MedicineInventory
    {
        public string BrandName { get; set; }
        public string Category { get; set; }
    }

    public class MedicineInventoryMetaData
    {
        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = "Brand is required", AllowEmptyStrings = false)]
        public int BrandID { get; set; }
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "Category is required", AllowEmptyStrings = false)]
        public int BrandCategoryID { get; set; }
        [Display(Name = "Medicine Name")]
        [Required(ErrorMessage = "Medicine is required", AllowEmptyStrings = false)]
        public string MedicineName { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required", AllowEmptyStrings = false)]
        public int AvailableQty { get; set; }
        [Display(Name = "Item Cost(Rs)")]
        [Required(ErrorMessage = "Item cost is required", AllowEmptyStrings = false)]
        public Nullable<decimal> PricePerItem { get; set; }
        [Display(Name = "Sheet Cost(Rs)")]
        [Required(ErrorMessage = "Sheet cost is required", AllowEmptyStrings = false)]
        public Nullable<decimal> PricePerSheet { get; set; }
        [Display(Name = "Batch Number")]
        [Required(ErrorMessage = "Batch is required", AllowEmptyStrings = false)]
        public string BatchNo { get; set; }
        [Display(Name = "Lot Number")]
        [Required(ErrorMessage = "Lot is required", AllowEmptyStrings = false)]
        public string LotNo { get; set; }
        [Display(Name = "Expiry Date")]
        [Required(ErrorMessage = "Expiry date is required", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> ExpiryDate { get; set; }
    }
}