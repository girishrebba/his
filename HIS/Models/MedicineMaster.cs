using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HIS
{
    [MetadataType(typeof(MedicineMasterMetaData))]
    public partial class MedicineMaster
    {
        public string BrandName { get; set; }
        public string Category { get; set; }
        public string MedicineDisplay { get; set; }
        public decimal? ItemPrice { get; set; }
        public string SelectDisplay { get; set; }
        public string SubCategory { get; set; }
        public string SupplierName { get; set; }
        public List<BrandSubCategory> SubCategories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public int AvailableQuantity { get; set; }
    }

    public class MedicineMasterMetaData
    {
        [Display(Name = "Brand:")]
        [Required(ErrorMessage = "Please select Brand", AllowEmptyStrings = false)]
        public int BrandID { get; set; }
        [Display(Name = "Category:")]
        [Required(ErrorMessage = "Please select Category", AllowEmptyStrings = false)]
        public int BrandCategoryID { get; set; }
        [Display(Name = "Medicine Name:")]
        [Required(ErrorMessage = "Please enter Medicine", AllowEmptyStrings = false)]
        public string MedicineName { get; set; }
        [Display(Name = "Dose:")]
        [Required(ErrorMessage = "Please enter Dose", AllowEmptyStrings = false)]
        public string MedDose { get; set; }
        [Display(Name = "Trigger Quantity:")]
        [Required(ErrorMessage = "Please enter Trigger Quantity", AllowEmptyStrings = false)]
        public int TriggerQty { get; set; }
        [Required(ErrorMessage = "Please choose Sub-Category", AllowEmptyStrings = false)]
        public int SubCategoryID { get; set; }
        [Required(ErrorMessage = "Please choose Supplier", AllowEmptyStrings = false)]
        public int SupplierID { get; set; }
    }
}