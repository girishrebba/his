using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class MedicineMasterController : Controller
    {
        // GET: MedicineMaster
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMasterMedicines()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from med in hs.MedicineMasters
                                 join b in hs.Brands on med.BrandID equals b.BrandID
                                 join bc in hs.BrandCategories on med.BrandCategoryID equals bc.CategoryID
                                 join sp in hs.Suppliers on med.SupplierID equals sp.SupplierID into sup
                                 from sp in sup.DefaultIfEmpty()
                                 join bsc in hs.BrandSubCategories on med.SubCategoryID equals bsc.SubCategoryID into subcat
                                 from bsc in subcat.DefaultIfEmpty()
                                 select new
                                 {
                                     med,
                                     b.BrandName,
                                     bc.Category,
                                     sp.SupplierName,
                                     bsc.SubCategory
                                 }).AsEnumerable().
                                           Select(x => new MedicineMaster
                                           {
                                               MMID = x.med.MMID,
                                               BrandName = x.BrandName,
                                               Category = x.Category,
                                               MedicineName = x.med.MedicineName,
                                               MedDose = x.med.MedDose,
                                               SubCategory = x.SubCategory,
                                               SupplierName = x.SupplierName,
                                               TriggerQty = x.med.TriggerQty
                                           }).ToList();

                return Json(new { data = medicines }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<Brand> Brands = HtmlHelpers.HtmlHelpers.GetBrands();
            List<BrandCategory> BrandCategories = HtmlHelpers.HtmlHelpers.GetBrandCategories();
            if (id == 0)
            {
                ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName");
                ViewBag.BrandCategories = new SelectList(BrandCategories, "CategoryID", "Category");
                return View(new MedicineMaster { 
                    
                    SubCategories = HtmlHelpers.HtmlHelpers.GetSubCategories(),
                    Suppliers = HtmlHelpers.HtmlHelpers.GetSuppliers()
                });
            }
            else
            {
                var medicineInventory = GetMedicine(id);
                if (medicineInventory != null)
                {
                    ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName", medicineInventory.BrandID);
                    ViewBag.BrandCategories = new SelectList(BrandCategories, "CategoryID", "Category", medicineInventory.BrandCategoryID);
                    medicineInventory.SubCategories = HtmlHelpers.HtmlHelpers.GetSubCategories();
                    medicineInventory.Suppliers = HtmlHelpers.HtmlHelpers.GetSuppliers();
                    return View(medicineInventory);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public JsonResult GetSubCategories(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var subcategories = (from bsc in hs.BrandSubCategories
                                 join mm in hs.MedicineMasters on bsc.SubCategoryID equals mm.SubCategoryID
                                 where (bsc.SubCategory.StartsWith(Prefix))
                                 select new { bsc }).AsEnumerable()
                                 .Select(m => new MedicineMaster
                                 {
                                     SubCategoryID = m.bsc.SubCategoryID,
                                     SubCategory = m.bsc.SubCategory
                                 }).ToList();
                return Json(subcategories, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSuppliers(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var suppliers = (from sp in hs.Suppliers
                                     join mm in hs.MedicineMasters on sp.SupplierID equals mm.SupplierID
                                     where (sp.SupplierName.StartsWith(Prefix))
                                     select new { sp }).AsEnumerable()
                                 .Select(m => new MedicineMaster
                                 {
                                     SupplierID = m.sp.SupplierID,
                                     SupplierName = m.sp.SupplierName
                                 }).ToList();
                return Json(suppliers, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetCategories(int id)
        {
            List<BrandCategory> brandCategories = HtmlHelpers.HtmlHelpers.GetBrandCategories();
            if (id > 0)
            {
                return Json(brandCategories.Where(bc => bc.BrandID == id).ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(brandCategories, JsonRequestBehavior.AllowGet);
        }

        //Get contact by ID
        public MedicineMaster GetMedicine(int mmID)
        {
            MedicineMaster medicineMaster = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from med in dc.MedicineMasters
                         join b in dc.Brands on med.BrandID equals b.BrandID
                         join bc in dc.BrandCategories on med.BrandCategoryID equals bc.CategoryID
                         join sp in dc.Suppliers on med.SupplierID equals sp.SupplierID into sup
                         from sp in sup.DefaultIfEmpty()
                         join bsc in dc.BrandSubCategories on med.SubCategoryID equals bsc.SubCategoryID into subcat
                         from bsc in subcat.DefaultIfEmpty()
                         where med.MMID.Equals(mmID)
                         select new
                         {
                             med,
                             b.BrandName,
                             bc.Category,
                             sp.SupplierName,
                             bsc.SubCategory
                         }).FirstOrDefault();
                if (v != null)
                {
                    medicineMaster = v.med;
                    medicineMaster.BrandName = v.BrandName;
                    medicineMaster.Category = v.Category;
                    medicineMaster.SubCategory = v.SubCategory;
                    medicineMaster.SupplierName = v.SupplierName;
                }
                return medicineMaster;
            }
        }

        [HttpPost]
        public ActionResult AddModify(MedicineMaster mm)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (mm.MMID == 0)
                {
                    //AddSupplierCategories(mm);
                    db.MedicineMasters.Add(mm);
                    MedicineInventory mi = new MedicineInventory();
                    mi.MedicineID = mm.MMID;
                    mi.AvailableQty = 0;
                    db.MedicineInventories.Add(mi);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //AddSupplierCategories(mm);
                    db.Entry(mm).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private void AddSupplierCategories(MedicineMaster mm)
        {
            if (mm != null && mm.SupplierID == 0 && !string.IsNullOrEmpty(mm.SupplierName))
            {
                mm.SupplierID = AddSuppliers(mm.SupplierName);
            }

            if (mm.SubCategoryID == 0 && !string.IsNullOrEmpty(mm.SubCategory))
            {
                mm.SubCategoryID = AddSubCategories(mm.SubCategory);
            }
        }

        private int AddSubCategories(string subCategory)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter subcatOut = new System.Data.Entity.Core.Objects.ObjectParameter("SubCategoryID", typeof(Int32));

                db.AddSubCategory(subCategory, subcatOut);
                return Convert.ToInt32(subcatOut.Value);
            }
        }

        private int AddSuppliers(string supplierName)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter supplierOut = new System.Data.Entity.Core.Objects.ObjectParameter("SupplierID", typeof(Int32));

                db.AddSupplier(supplierName, supplierOut);
                return Convert.ToInt32(supplierOut.Value);
            }
        }

        

        private void UpdateMedInventory(MedicineMaster mm)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                
            }
        }
    }
}