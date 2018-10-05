using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.HtmlHelpers;
using HIS.Action_Filters;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        [His]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPoNumbers()
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
        public JsonResult GetShippedMedicines(string poNumber)
        {       
                List<PurchaseOrderViewModel> shippedMedicines = GetPOItems(poNumber);
                return Json(new { data = shippedMedicines }, JsonRequestBehavior.AllowGet);       
        }

        public List<PurchaseOrderViewModel> GetPOItems(string poNumber)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                return (from po in hs.PurchaseOrders
                        join mm in hs.MedicineMasters on po.MedicineID equals mm.MMID
                        where po.PONumber.Equals(poNumber)
                        select new
                        {
                            po,
                            mm.MedicineName,
                            mm.MedDose
                        }).OrderByDescending(p => p.po.OrderID)
                                                    .AsEnumerable().
                                                       Select(x => new PurchaseOrderViewModel
                                                       {
                                                           OrderID = x.po.OrderID,
                                                           PONumber = x.po.PONumber,
                                                           MedicineID = x.po.MedicineID,
                                                           MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.MedicineName, x.MedDose),
                                                           OrderedQty = x.po.OrderedQty,
                                                           OrderDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.OrderedDate),
                                                           OrderedDate = x.po.OrderedDate,
                                                           ExpiryDateDisplay = HtmlHelpers.HtmlHelpers.DateFormat(x.po.ExpiryDate),
                                                           ExpiryDate = x.po.ExpiryDate,
                                                           ApprovedStatus = x.po.ApprovedStatus,
                                                           PricePerItem = x.po.PricePerItem,
                                                           PricePerSheet = x.po.PricePerSheet,
                                                           BatchNo = x.po.BatchNo,
                                                           LotNo = x.po.LotNo
                                                       }).ToList();
            }
        }

        public List<OrderRequest> GetRequestedItems()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var order = hs.OrderMasters.OrderByDescending(o => o.OrderNO).FirstOrDefault();

                if (order != null && order.Status == false)
                {
                    return (from om in hs.OrderMasters
                            join or in hs.OrderRequests on om.OMID equals or.OMID
                            join mm in hs.MedicineMasters on or.MedicineID equals mm.MMID
                            where om.OrderNO == order.OrderNO
                            select new
                            {
                                om,
                                or,
                                mm.MedicineName,
                                mm.MedDose
                            }).OrderByDescending(p => p.om.OMID)
                                                        .AsEnumerable().
                                                           Select(x => new OrderRequest
                                                           {
                                                               OrderNo = x.om.OrderNO,
                                                               MedicineID = x.or.MedicineID,
                                                               MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.MedicineName, x.MedDose),
                                                               Quantity = x.or.Quantity,
                                                               PlacedQty = x.or.Quantity,
                                                               OrderDate = DateTime.Now.ToString("MM/dd/yyyy") 

                                                           }).ToList();
                }
                else
                {
                    return new List<OrderRequest>();
                }
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
                        m.PricePerItem = po.PricePerItem;
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

        [HttpGet]
        public ActionResult ViewPO(string poNumber)
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

        public JsonResult GetShippedMedicines()
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

        //[HttpGet]
        //public ActionResult AddModify(int id = 0)
        //{
        //    if (id == 0)
        //    {
        //        return View(new PurchaseOrder());
        //    }
        //    else
        //    {
        //        var shippedMedicine = GetShipmentInfo(id);
        //        if (shippedMedicine != null)
        //        {
        //          return View(shippedMedicine);
        //        }
        //        else
        //        {
        //            return HttpNotFound();
        //        }
        //    }
        //}

        [HttpPost]
        public JsonResult GetMedicines(string Prefix)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var medicines = (from mm in hs.MedicineMasters
                                 where mm.MedicineName.StartsWith(Prefix)
                                 select new { mm }).AsEnumerable()
                                 .Select(m => new MedicineMaster { MMID = m.mm.MMID,
                                 MedicineDisplay = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(m.mm.MedicineName, m.mm.MedDose),
                                 }).ToList();
                return Json(medicines, JsonRequestBehavior.AllowGet);
            }         
        }
        
        //[HttpPost]
        //public ActionResult AddModify(PurchaseOrder po)
        //{
        //    using (HISDBEntities db = new HISDBEntities())
        //    {
        //        if (po.OrderID == 0)
        //        {
        //            db.PurchaseOrders.Add(po);
        //            db.SaveChanges();
        //            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            db.Entry(po).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

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
            return EmptyPurchaseOrder();
        }

        private ActionResult EmptyPurchaseOrder()
        {
            return View(new PurchaseOrderViewModel
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
        
        [HttpGet]
        public ActionResult EditPO(string poNumber)
        {
            List<PurchaseOrderViewModel> shippedMedicines = GetPOItems(poNumber);
            return View(shippedMedicines);
        }

        [HttpGet]
        public ActionResult PlaceOrder()
        {
            List<OrderRequest> reqMedicines = GetRequestedItems();
            return View(reqMedicines);
        }

        [HttpPost]
        public ActionResult PlaceOrder(List<OrderRequest> items)
        {
            bool hasItems = true;
            string orderNo = string.Empty;
            using (HISDBEntities db = new HISDBEntities())
            {
                if (items != null && items.Count() > 0)
                {
                    orderNo = items[0].OrderNo;
                    if(items.Count() == 1 && items[0].MedicineID == 0)
                    {
                        hasItems = false;
                    }

                    if (hasItems)
                    {
                        foreach (OrderRequest or in items)
                        {
                            var po = new PurchaseOrder()
                            {
                                PONumber = or.OrderNo,
                                OrderedDate = DateTime.Now,
                                OrderedQty = or.PlacedQty.HasValue ? or.PlacedQty.Value : 0,
                                MedicineID = or.MedicineID
                            };
                            db.PurchaseOrders.Add(po);
                        }
                    }
                    var reqOrder = db.OrderMasters.Where(o => o.OrderNO == orderNo).FirstOrDefault();
                    if(reqOrder != null)
                    {
                        reqOrder.Status = true;
                        db.Entry(reqOrder).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json(new { success = true, message = string.Format("Order# - {0} placed Successfully", items[0].OrderNo) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        [HttpPost]
        public ActionResult EditPO(List<PurchaseOrder> items)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (items != null && items.Count() > 0)
                {
                    foreach (PurchaseOrder po in items)
                    {
                        if (po.OrderID == 0)
                        {
                            db.PurchaseOrders.Add(po);
                        }
                        else
                        {
                            db.Entry(po).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    DeletePurchaseOrderItems(items);
                    return Json(new { success = true, message = string.Format("PO# - {0} adjusted Successfully", items[0].PONumber) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Error occured" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        private static void DeletePurchaseOrderItems(List<PurchaseOrder> items)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                string poNumber = items[0].PONumber;
                var poItems = hs.PurchaseOrders.Where(poi => poi.PONumber == poNumber).ToList();

                var itemsToBeDelete = poItems.Where(p => !items.Any(p2 => p2.OrderID == p.OrderID));
                // Delete the items from purchase order
                if (itemsToBeDelete != null && itemsToBeDelete.Count() > 0)
                {
                    hs.PurchaseOrders.RemoveRange(itemsToBeDelete);
                    hs.SaveChanges();
                }
            }
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