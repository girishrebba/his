using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HIS
{
    [MetadataType(typeof(InPatientMetaData))]
    public partial class InPatient
    {
        public string Name{ get; set; }
        public string Address { get; set; }
        public string GenderDisplay { get; set; }
        public string DoctorName { get; set; }
        public string BloodGroupDisplay { get; set; }
        public string MaritalStatusDisplay { get; set; }
        public string DOBDisplay { get; set; }
        public string EnrolledDisplay { get; set; }
        public string DischargeDateDisplay { get; set; }
        public List<Purpose> Purposes { get; set; }
        public string[] PurposeIds { get; set; }
        public string PurposeDisplay { get; set; }
        public List<PharmaKit> PharmaKits { get; set; }
        public string GetFullName()
        {
            return string.Format("{0} {1} {2}", 
                string.IsNullOrEmpty(this.FirstName)?string.Empty : this.FirstName,
                string.IsNullOrEmpty(this.MiddleName) ? string.Empty : this.MiddleName,
                string.IsNullOrEmpty(this.LastName) ? string.Empty : this.LastName);
        }

        public string GetDOBFormat()
        {
            return this.DOB != null ? this.DOB.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public string GetEnrolledFormat()
        {
            return this.Enrolled != null ? this.Enrolled.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public string GetAge()
        {
            return this.Age != null ? this.Age.ToString() : string.Empty;
        }

        public string GetFullAddress()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                string.IsNullOrEmpty(this.Address1) ? string.Empty : this.Address1 + ",",
                string.IsNullOrEmpty(this.Address2) ? string.Empty : this.Address2 + ",",
                string.IsNullOrEmpty(this.City) ? string.Empty : this.City+",",
                string.IsNullOrEmpty(this.State) ? string.Empty : this.State,
                string.IsNullOrEmpty(this.PinCode) ? string.Empty : "- "+this.PinCode);
        }
        public string GetGender()
        {
            if (this.Gender == 0)
                return "Male";
            else if (this.Gender == 1)
                return "Female";
            else return "Others";
        }

        public string GetMaritalStatus()
        {
            if (this.MaritalStatus == 0)
                return "Single";
            else if (this.MaritalStatus == 1)
                return "Married";
            else return "Others";
        }
    }

    public class PackageViewModel
    {
        public string ENMRNO { get; set; }
        public List<PharmaKit> PharmaKits { get; set; }
        public string[] PharmaKitIds { get; set; }
        public List<LabKit> LabKits { get; set; }
        public string[] LabKitIds { get; set; }
    }

    public class InPatientMetaData
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
        public Nullable<DateTime> Enrolled { get; set; }
        [Required(ErrorMessage = "Doctor is Required", AllowEmptyStrings = false)]
        public int DoctorID { get; set; }
        [Required(ErrorMessage = "Blood Group is Required", AllowEmptyStrings = false)]
        public int BloodGroupID { get; set; }
        [MaxLength(3)]
        [Required(ErrorMessage = "Age is Required", AllowEmptyStrings = false)]
        public int Age { get; set; }


        public Nullable<int> ProviderID { get; set; }
        public string InsuranceFileNo { get; set; }
        public Nullable<decimal> InsuranceEstAmt { get; set; }
        public Nullable<decimal> InsuranceRecievedAmt { get; set; }
    }
}