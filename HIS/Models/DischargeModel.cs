using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(DischargeModelMetaData))]
    public partial class DischargeModel
    {
        public string ENMRNO { get; set; }
        public RoomBilling RoomChargeTable { get; set; }
        public List<FeeCollection> FeeCollectionTable { get; set; }
        public string DischargeSummary { get; set; }
    }

    public class DischargeModelMetaData
    {
        [Required(ErrorMessage = "Please enter Summary", AllowEmptyStrings = false)]
        public string DischargeSummary { get; set; }
    }
}