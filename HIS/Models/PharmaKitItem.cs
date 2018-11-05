using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public partial class PharmaKitItem
    {
        public string KitName { get; set; }
        public List<MedicineMaster> MedItems { get; set; }

    }
}