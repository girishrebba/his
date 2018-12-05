using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS
{
    public partial class PatientHistory
    {
        public List<PatientVisitHistory> Visits { get; set; }
        public List<PatientTest> Tests { get; set; }
        public List<PatientScan> Scans { get; set; }
        public List<InPatientHistory> Observations { get; set; }
        public List<PatientPrescription> Prescriptions { get; set; }
        public string DischargeNote { get; set; }
    }
}