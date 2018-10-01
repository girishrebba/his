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
                                 select new
                                 {
                                     med,
                                     b.BrandName,
                                     bc.Category
                                 }).AsEnumerable().
                                           Select(x => new MedicineMaster
                                           {
                                               MMID = x.med.MMID,
                                               BrandName = x.BrandName,
                                               Category = x.Category,
                                               MedicineName = x.med.MedicineName,
                                               MedDose = x.med.MedDose,
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
                return View(new MedicineMaster());
            }
            else
            {
                var medicineInventory = GetMedicine(id);
                if (medicineInventory != null)
                {
                    ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName", medicineInventory.BrandID);
                    ViewBag.BrandCategories = new SelectList(BrandCategories, "CategoryID", "Category", medicineInventory.BrandCategoryID);
                    return View(medicineInventory);
                }
                else
                {
                    return HttpNotFound();
                }
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
                         where med.MMID.Equals(mmID)
                         select new
                         {
                             med,
                             b.BrandName,
                             bc.Category
                         }).FirstOrDefault();
                if (v != null)
                {
                    medicineMaster = v.med;
                    medicineMaster.BrandName = v.BrandName;
                    medicineMaster.Category = v.Category;
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
                    db.Entry(mm).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
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