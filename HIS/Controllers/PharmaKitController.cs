using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.Action_Filters;
using System.ComponentModel;

namespace HIS.Controllers
{
    [SessionActionFilter]
    public class PharmaKitController : Controller
    {
        [His]
        [Description(" - Pharma Kits view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPharmaKits()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var pKits = (from pk in hs.PharmaKits
                             select new { pk.PKitID, pk.PKitName, pk.PKitCost }).ToList();

                return Json(new { data = pKits }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Pharma Kits Add/Edit page.")]
        public ActionResult AddModifyKitItems(int id = 0)
        {
           return View(new PharmaKitViewModel()); 
        }

        [HttpGet]
        [Description(" - Pharma Kits Add/Edit page.")]
        public ActionResult EditKitItems(int id = 0)
        {
            if (id > 0)
            {
                return View(GetPharmaKitItems(id));
            }
            else return View(new List<PharmaKitViewModel>());       
        }

        [HttpGet]
        [Description(" - Pharma Kit Items View page.")]
        public ActionResult ViewKitItems(int id = 0)
        { 
           return View(GetPharmaKitItems(id));   
        }

        public List<PharmaKitViewModel> GetPharmaKitItems(int pkitId)
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var pKitItems = (from pki in hs.PharmaKitItems
                                 join pk in hs.PharmaKits on pki.PKitID equals pk.PKitID
                                 join mi in hs.MedicineMasters on pki.MedicineID equals mi.MMID
                                 where  pki.PKitID == pkitId
                                 select new { pk, mi, pki }).AsEnumerable()
                             .Select(x => new PharmaKitViewModel
                             {
                                 PKitName = x.pk.PKitName,
                                 PKitID = x.pk.PKitID,
                                 PKitCost = x.pk.PKitCost,
                                 MedicineID = x.mi.MMID,
                                 MedicineWithDose = HtmlHelpers.HtmlHelpers.GetMedicineWithDose(x.mi.MedicineName, x.mi.MedDose),
                                 Quantity = x.pki.Quantity
                             }).ToList();
                return pKitItems;
            }
        }

        [HttpPost]
        public JsonResult AddModifyKitItems(List<PharmaKitViewModel> kitItems)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if(kitItems != null && kitItems.Count() > 0)
                {
                    var kitID = kitItems[0].PKitID;

                    if (kitID == 0)
                    {
                        System.Data.Entity.Core.Objects.ObjectParameter pkitidOut = new System.Data.Entity.Core.Objects.ObjectParameter("PKitID", typeof(Int32));

                        db.CreateMasterPharmaKit(kitItems[0].PKitName, kitItems[0].PKitCost, pkitidOut);
                        kitID = Convert.ToInt32(pkitidOut.Value);

                        foreach (var kit in kitItems)
                        {
                            var pharmaKitItem = new PharmaKitItem { PKitID = kitID, MedicineID = kit.MedicineID, Quantity = kit.Quantity };
                            db.PharmaKitItems.Add(pharmaKitItem);
                        }
                        db.SaveChanges();
                        return Json(new { success = true, message = "Pharma Package created Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var items = db.PharmaKitItems.Where(x => x.PKitID == kitID).ToList();
                        db.PharmaKitItems.RemoveRange(items);

                        foreach (var kit in kitItems)
                        {
                            var pharmaKitItem = new PharmaKitItem { PKitID = kitID, MedicineID = kit.MedicineID, Quantity = kit.Quantity };
                            db.PharmaKitItems.Add(pharmaKitItem);
                        }

                        PharmaKit pkit = (from x in db.PharmaKits
                                      where x.PKitID == kitID
                                      select x).First();
                        pkit.PKitCost = kitItems[0].PKitCost;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Pharma Package Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Error Found!!" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}