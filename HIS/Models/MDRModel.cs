using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public class MDRModel
    {
        public string ENMRNO { get; set; }
        public int MedicineID { get; set; }
        public string MedicineWithDose { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public string BatchNo { get; set; }
        public string LotNo { get; set; }
        public decimal ItemPrice { get; set; }
        public DateTime ReqDate { get; set; }
        public int PrescribedBy { get; set; }
        public decimal Discount { get; set; }
        public int PMID { get; set; }
        public int IntakeFrequencyID { get; set; }
    }

    public class MDRMetaData
    {
        [Required(ErrorMessage = "Please enter Medicine", AllowEmptyStrings = false)]
        public string MedicineWithDose { get; set; }

        [Required(ErrorMessage = "Please enter quantity", AllowEmptyStrings = false)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please choose doctor", AllowEmptyStrings = false)]
        public int PrescribedBy { get; set; }
    }
}