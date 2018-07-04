using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HIS.Controllers
{
    public class MedInventoryController : Controller
    {
        // GET: MedInventory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMedicines()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from med in hs.MedicineInventories
                                       join b in hs.Brands on med.BrandID equals b.BrandID
                                       join bc in hs.BrandCategories on med.BrandCategoryID equals bc.CategoryID
                                       select new {
                                           med.MedInventoryID,
                                           b.BrandName,
                                           bc.Category,
                                           med.MedicineName,
                                           med.ExpiryDate,
                                           med.BatchNo,
                                           med.LotNo,
                                           med.AvailableQty,
                                           med.PricePerItem,
                                           med.PricePerSheet
                                           }).ToList();

                return Json(new { data = medicines }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            List<Brand> Brands = GetBrands();
            List<BrandCategory> BrandCategories = GetBrandCategories();
            if (id == 0)
            {
                ViewBag.Brands = new SelectList(Brands, "BrandID", "BrandName");
                ViewBag.BrandCategories = new SelectList(BrandCategories, "CategoryID", "Category");
                return View(new MedicineInventory());
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
            List<BrandCategory> brandCategories = GetBrandCategories();
            if(id > 0)
            {
                return Json(brandCategories.Where(bc => bc.BrandID == id).ToList(), JsonRequestBehavior.AllowGet);               
            }
            return Json(brandCategories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddModify(MedicineInventory mi)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (mi.MedInventoryID == 0)
                {
                    db.MedicineInventories.Add(mi);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(mi).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //Get contact by ID
        public MedicineInventory GetMedicine(int inventoryID)
        {
            MedicineInventory medicineInventory = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from med in dc.MedicineInventories
                         join b in dc.Brands on med.BrandID equals b.BrandID
                         join bc in dc.BrandCategories on med.BrandCategoryID equals bc.CategoryID
                         where med.MedInventoryID.Equals(inventoryID)
                         select new
                         {
                             med,
                             b.BrandName,
                             bc.Category
                         }).FirstOrDefault();
                if (v != null)
                {
                    medicineInventory = v.med;
                    medicineInventory.BrandName = v.BrandName;
                    medicineInventory.Category = v.Category;
                }
                return medicineInventory;
            }
        }

        //Fetch Brands from database
        public List<Brand> GetBrands()
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

        //Fetch Brands from database
        public List<BrandCategory> GetBrandCategories()
        {
            using (HISDBEntities dc = new HISDBEntities())
            {
                var brandCategories = (from bc in dc.BrandCategories
                              select new { bc.CategoryID, bc.Category, bc.BrandID })
                              .OrderBy(b => b.Category).AsEnumerable()
                              .Select(x => new BrandCategory { CategoryID = x.CategoryID, Category = x.Category, BrandID= x.BrandID}).ToList();
                return brandCategories;
            }
        }

    }
}