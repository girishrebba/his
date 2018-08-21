using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public class PatientPrescriptionHistory
    {
        public string VisitName { get; set; }
        public IList<PatientPrescription> Prescriptions { get; set; }
        public IList<PatientTest> PatientTests { get; set; }
    }
}