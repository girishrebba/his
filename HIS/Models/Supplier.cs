using System;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(SupplierMetaData))]
    public partial class Supplier
    {
       
    }

    public class SupplierMetaData
    {
        [Required(ErrorMessage = "Supplier is required", AllowEmptyStrings = false)]
        public string SupplierName { get; set; }
    }
}