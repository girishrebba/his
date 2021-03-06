﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    public class PatientPrescriptionHistory
    {
        public int SNO { get; set; }
        public string VisitName { get; set; }
        public IList<PatientPrescription> Prescriptions { get; set; }
        public IList<PatientTest> PatientTests { get; set; }
        public IList<PatientScan> PatientScans { get; set; }
        public IList<PatientPrescription> MDRPrescriptions { get; set; }
        public IList<PatientPrescription> InPrescriptions { get; set; }
        public IList<PatientTest> InPatientTests { get; set; }
        public IList<PatientScan> InPatientScans { get; set; }
    }
}