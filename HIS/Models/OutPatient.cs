﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(OutPatientMetaData))]
    public partial class OutPatient
    {
        public int RowNum { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string GenderDisplay { get; set; }
        public string DoctorName { get; set; }
        public string BloodGroupDisplay { get; set; }
        public string MaritalStatusDisplay { get; set; }
        public string DOBDisplay { get; set; }
        public string EnrolledDisplay { get; set; }
        public string ValidDateDisplay { get; set; }
        public string PurposeDisplay { get; set; }
        public List<Purpose> Purposes { get; set; }
        public string[] PurposeIds { get; set; }
        public string VisitDate { get; set; }
        public string GetFullName()
        {
            return string.Format("{0} {1} {2}",
                string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName,
                string.IsNullOrEmpty(this.MiddleName) ? string.Empty : this.MiddleName,
                string.IsNullOrEmpty(this.LastName) ? string.Empty : this.LastName);
        }

        public string GetDOBFormat()
        {
            return this.DOB != null ? this.DOB.Value.ToString("dd-MMM-yyyy") : string.Empty;
        }

        public string GetAge()
        {
            return this.Age != null ? this.Age.ToString() : string.Empty;
        }

        public string GetEnrolledFormat()
        {
            return this.Enrolled != null ? this.Enrolled.Value.ToString("dd-MMM-yyyy") : string.Empty;
        }        
    }

    public class OutPatientMetaData
    {
        [Required(ErrorMessage = "First Name is Required", AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required", AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone is Required", AllowEmptyStrings = false)]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address is Required", AllowEmptyStrings = false)]
        public string Address1 { get; set; }
        [Required(ErrorMessage = "Enroll date is Required", AllowEmptyStrings = false)]
        public Nullable<System.DateTime> Enrolled { get; set; }
        [Required(ErrorMessage = "Doctor is Required", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Blood Group is Required", AllowEmptyStrings = false)]
        public int BloodGroupID { get; set; }
        [MaxLength(3)]
        [Required(ErrorMessage = "Age is Required", AllowEmptyStrings = false)]
        public int Age { get; set; }
    }
}