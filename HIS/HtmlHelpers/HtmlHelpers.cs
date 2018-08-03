using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace HIS.HtmlHelpers
{
    public static class HtmlHelpers
    {
        public static string DateFormat(this HtmlHelper helper, DateTime? date)
        {
            return (date != null && date != DateTime.MinValue) ? date.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public static string DateFormat(DateTime? date)
        {
            return (date != null && date != DateTime.MinValue) ? date.Value.ToString("MM/dd/yyyy") : string.Empty;
        }

        public static string GetFullName(string firstName, string middleName, string lastName)
        {
            return string.Format("{0} {1} {2}",
                string.IsNullOrEmpty(firstName) ? string.Empty : firstName,
                string.IsNullOrEmpty(middleName) ? string.Empty : middleName,
                string.IsNullOrEmpty(lastName) ? string.Empty : lastName);
        }

        public static string GetMedicineWithDose(string medicineName, string dose)
        {
            return string.Format("{0} - {1}", medicineName, dose);
        }

        public static string Get_IN_ENMR(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities hs = new HISDBEntities())
            {
                enmrNo = hs.InPatients.Where(op => op.SNO == id).FirstOrDefault().ENMRNO;
            }
            return enmrNo;
        }

        public static string Get_OUT_ENMR(int id)
        {
            string enmrNo = string.Empty;
            using (HISDBEntities hs = new HISDBEntities())
            {
                enmrNo = hs.OutPatients.Where(op => op.SNO == id).FirstOrDefault().ENMRNO;
            }
            return enmrNo;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string GetFullAddress(string address1, string address2, string city, string state, string zip)
        {
            return string.Format("{0} {1} {2} {3} {4}",
                string.IsNullOrEmpty(address1) ? string.Empty : address1 + ",",
                string.IsNullOrEmpty(address1) ? string.Empty : ","+address2,
                string.IsNullOrEmpty(city) ? string.Empty : ","+city,
                string.IsNullOrEmpty(state) ? string.Empty : state,
                string.IsNullOrEmpty(zip) ? string.Empty : "- " + zip);
        }
        public static string GetGender(int? gender)
        {
            if (gender != null)
            {
                if (gender == 0)
                    return "Male";
                else if (gender == 1)
                    return "Female";
                else return "Others";
            }
            else return string.Empty;
        }

        public static string GetMaritalStatus(int? status)
        {
            if (status != null)
            {
                if (status == 0)
                    return "Single";
                else if (status == 1)
                    return "Married";
                else return "Others";
            }
            else return string.Empty;       
        }

        //Fetch Brands from database
        public static List<Brand> GetBrands()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brands = (from b in dc.Brands
                              select new { b.BrandID, b.BrandName })
                              .OrderBy(b => b.BrandName).AsEnumerable()
                              .Select(x => new Brand { BrandID = x.BrandID, BrandName = x.BrandName }).ToList();
                return brands;
            }
        }

        //Fetch Brand categories from database
        public static List<BrandCategory> GetBrandCategories()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brandCategories = (from bc in dc.BrandCategories
                                       select new { bc.CategoryID, bc.Category, bc.BrandID })
                              .OrderBy(b => b.Category).AsEnumerable()
                              .Select(x => new BrandCategory { CategoryID = x.CategoryID, Category = x.Category, BrandID = x.BrandID }).ToList();
                return brandCategories;
            }
        }

        public static List<User> GetUsers()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var users = (from u in dc.Users
                             select new { u })
                              .OrderBy(b => b.u.UserName).AsEnumerable()
                              .Select(x => new User { UserID = x.u.UserID, NameDisplay = x.u.GetFullName() }).ToList();
                return users;
            }
        }

        public static List<User> GetDoctors()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var doctors = (from u in dc.Users
                               join ut in dc.UserTypes on u.UserTypeID equals ut.UserTypeID
                               where ut.UserTypeName.Equals("Doctor")
                               select new { u })
                               .OrderBy(b => b.u.UserName).AsEnumerable()
                               .Select(x => new User { UserID = x.u.UserID, NameDisplay = GetFullName(x.u.FirstName,x.u.MiddleName,x.u.LastName) }).ToList();
                return doctors;
            }
        }

        //Fetch Blood Groups from database
        public static List<BloodGroup> GetBloodGroups()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var bloodGroups = (from bg in dc.BloodGroups
                                   select new { bg.GroupID, bg.GroupName })
                              .OrderBy(b => b.GroupName).AsEnumerable()
                              .Select(x => new BloodGroup { GroupID = x.GroupID, GroupName = x.GroupName }).ToList();
                return bloodGroups;
            }
        }

        public static List<ConsultationType> GetConsultationTypes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var consultTypes = (from ct in dc.ConsultationTypes
                             select new {ct.ConsultTypeID, ct.ConsultType }).AsEnumerable()
                             .Select(x => new ConsultationType { ConsultTypeID = x.ConsultTypeID, ConsultType = x.ConsultType }).ToList();
                return consultTypes;
            }
        }

        public static List<IntakeFrequency> GetIntakes()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var intakes = (from ifs in dc.IntakeFrequencies
                                    select new { ifs.FrequencyID, ifs.Frequency }).AsEnumerable()
                             .Select(x => new IntakeFrequency { FrequencyID = x.FrequencyID,
                                 Frequency = x.Frequency }).ToList();
                return intakes;
            }
        }

        public static List<ConsultationFee> GetConsultationFees()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var consultationFees = (from ct in dc.ConsultationFees
                                    select new { ct }).AsEnumerable()
                             .Select(x => new ConsultationFee {
                                ConsultationID  = x.ct.ConsultationID,
                                 DoctorID = x.ct.DoctorID,
                                 ConsultTypeID = x.ct.ConsultTypeID,
                                 Fee = x.ct.Fee
                             }).ToList();
                return consultationFees;
            }
        }
        public static string GetSequencedEnmrNo()
        {
            string initial = ConfigurationManager.AppSettings["EnmrNoStartsWith"];
            string ipEnmr = "0", opEnmr = "0";
            InPatient in_Patient;
            OutPatient out_Patient;
            using (HISDBEntities db = new HISDBEntities())
            {
                in_Patient = db.InPatients.OrderByDescending(ip => ip.SNO).FirstOrDefault();
                out_Patient = db.OutPatients.OrderByDescending(ip => ip.SNO).FirstOrDefault();
            }

            if (in_Patient != null) ipEnmr = in_Patient.ENMRNO;
            if (out_Patient != null) opEnmr = out_Patient.ENMRNO;

            return string.Format("{0}-{1}", initial, GetNextEnmrNo(ipEnmr.Substring(ipEnmr.LastIndexOf('-')+1)
                , opEnmr.Substring(opEnmr.LastIndexOf('-')+1)).ToString().PadLeft(5,'0'));            
        }

        public static int GetNextEnmrNo(string ipEnmr, string opEnmr)
        {
            int enmrno = Math.Max(Convert.ToInt32(ipEnmr), Convert.ToInt32(opEnmr));
            return enmrno+1;
        }
    }
}