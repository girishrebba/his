using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.HtmlHelpers;

namespace HIS.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPoNumbers()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var PoNumbers = (from po in hs.PurchaseOrders
                                 select new { po }).DistinctBy(x => x.po.PONumber).OrderByDescending(x => x.po.OrderID)
                                 .AsEnumerable()
                                 .Select(x => new PurchaseOrder { PONumber = x.po.PONumber, ApprovedStatus = x.po.ApprovedStatus, OrderDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.OrderedDate) }).ToList();

                return Json(new { data = PoNumbers }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetShippedMedicines(string poNumber)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var shippedMedicines = (from po in hs.PurchaseOrders
                                        join mm in hs.MedicineMasters on po.MedicineID equals mm.MMID
                                        where po.PONumber.Equals(poNumber)
                                        select new
                                        {
                                            po,
                                            mm.MedicineName,
                                            mm.MedDose
                                        }).OrderByDescending(p => p.po.OrderID)
                                        .AsEnumerable().
                                           Select(x => new PurchaseOrder
                                           {
                                               OrderID = x.po.OrderID,
                                               PONumber = x.po.PONumber,
                                               MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.MedicineName, x.MedDose),
                                               OrderedQty = x.po.OrderedQty,
                                               OrderDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.OrderedDate),
                                               ExpiryDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.ExpiryDate),
                                               ApprovedStatus = x.po.ApprovedStatus,
                                               PricePerItem = x.po.PricePerItem,
                                               PricePerSheet = x.po.PricePerSheet,
                                               BatchNo = x.po.BatchNo,
                                               LotNo = x.po.LotNo
                                           }).ToList();
                return Json(new { data = shippedMedicines }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Approve(string poNumber)
        {
            ViewBag.PoNumber = poNumber;
            return View();
        }

        [ActionName("Approve")]
        [HttpPost]
        public ActionResult Approve_Post(string poNumber)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var poItems = hs.PurchaseOrders.Where(i => i.PONumber.Equals(poNumber)).ToList();
                foreach(PurchaseOrder po in poItems)
                {
                    po.ApprovedStatus = true;
                    po.MedicineWithDose = "text"; 
                    hs.Entry(po).State = EntityState.Modified;
                }
                hs.SaveChanges();
                UpdateMedicineInventory(poItems);
                return Json(new { success = true, message = string.Format("PO - {0} approved successfully!!", poNumber) }, JsonRequestBehavior.AllowGet);

            }
        }

        private void UpdateMedicineInventory(List<PurchaseOrder> poItems)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                foreach (PurchaseOrder po in poItems)
                {
                    var minventory = hs.MedicineInventories.Where(mi => mi.MedicineID == po.MedicineID).ToList();
                   if (minventory != null && minventory.Count() > 0)
                    {
                        var m = minventory[0];
                        m.AvailableQty = m.AvailableQty + po.OrderedQty;
                        hs.Entry(m).State = EntityState.Modified;
                        
                    }
                    else
                    {
                        MedicineInventory newM = new MedicineInventory();
                        newM.MedicineID = po.MedicineID;
                        newM.AvailableQty = po.OrderedQty;
                        hs.MedicineInventories.Add(newM);
                    }
                }
                hs.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult DeletePO(string poNumber)
        {
            ViewBag.PoNumber = poNumber;
            return View();
        }

        [ActionName("DeletePO")]
        [HttpPost]
        public ActionResult Delete_Post(string poNumber)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var poItems = hs.PurchaseOrders.Where(i => i.PONumber.Equals(poNumber)).ToList();
                foreach (PurchaseOrder po in poItems)
                {
                    hs.PurchaseOrders.Remove(po);
                }
                hs.SaveChanges();
                return Json(new { success = true, message = string.Format("PO - {0} deleted successfully!!", poNumber) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetShippedMedicines()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var shippedMedicines = (from po in hs.PurchaseOrders
                                 join mm in hs.MedicineMasters on po.MedicineID equals mm.MMID
                                 select new
                                 {
                                     po,
                                     mm.MedicineName,
                                     mm.MedDose
                                 }).AsEnumerable().
                                           Select(x => new PurchaseOrder
                                           {
                                               OrderID = x.po.OrderID,
                                               PONumber = x.po.PONumber,
                                               MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.MedicineName, x.MedDose),
                                               OrderedQty = x.po.OrderedQty,
                                               OrderDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.OrderedDate),
                                               ExpiryDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.ExpiryDate),
                                               ApprovedStatus = x.po.ApprovedStatus,
                                               PricePerItem = x.po.PricePerItem,
                                               PricePerSheet = x.po.PricePerSheet,
                                               BatchNo = x.po.BatchNo,
                                               LotNo = x.po.LotNo
                                           }).ToList();

                return Json(new { data = shippedMedicines }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            if (id == 0)
            {
                return View(new PurchaseOrder());
            }
            else
            {
                var shippedMedicine = GetShipmentInfo(id);
                if (shippedMedicine != null)
                {
                  return View(shippedMedicine);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public JsonResult GetMedicines(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from mm in hs.MedicineMasters
                                 where mm.MedicineName.StartsWith(Prefix)
                                 select new { mm }).AsEnumerable()
                                 .Select(m => new MedicineMaster { MMID = m.mm.MMID,
                                 MedicineDisplay = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(m.mm.MedicineName, m.mm.MedDose)}).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }         
        }
        
        [HttpPost]
        public ActionResult AddModify(PurchaseOrder po)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (po.OrderID == 0)
                {
                    db.PurchaseOrders.Add(po);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(po).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public PurchaseOrder GetShipmentInfo(int orderID)
        {
            PurchaseOrder shippedMedicine = null;
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from po in dc.PurchaseOrders
                         join mm in dc.MedicineMasters on po.MedicineID equals mm.MMID
                         where po.OrderID.Equals(orderID)
                         select new
                         {
                             po,
                             mm.MedicineName,
                             mm.MedDose
                         }).FirstOrDefault();
                if (v != null)
                {
                    shippedMedicine = v.po;
                    shippedMedicine.MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(v.MedicineName, v.MedDose);
                }
                return shippedMedicine;
            }
        }

        public ActionResult CreatePO()
        {               
            return View(new PurchaseOrder
            {
                PONumber = string.Empty,
                MedicineID = 0,
                OrderedQty = 0,
                OrderedDate = DateTime.MinValue,
                PricePerItem = 0,
                PricePerSheet = 0,
                BatchNo = string.Empty,
                LotNo = string.Empty,
                ExpiryDate = DateTime.MinValue

            });
        }

        [HttpPost]
        public ActionResult CreatePO(IList<PurchaseOrder> poItems)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if(poItems != null && poItems.Count() > 0)
                {
                    foreach(PurchaseOrder po in poItems)
                    {
                        db.PurchaseOrders.Add(po);
                    }
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Purchase Order - {0} created Successfully", poItems[0].PONumber)}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured"}, JsonRequestBehavior.AllowGet);
                }
            }
            
        }
    }
}