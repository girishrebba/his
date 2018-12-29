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
    public class LabKitController : Controller
    {
        [His]
        [Description(" - Lab Kits view page.")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetLabKits()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var lKits = (from tt in hs.TestTypes
                             where tt.IsKit == true
                                    select new { tt.TestID, tt.TestName, tt.TestCost }).ToList();

                return Json(new { data = lKits }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Description(" - Lab Kits Add page.")]
        public ActionResult AddLabKits(int id = 0)
        {
            return View(new LabKitViewModel());
        }

        [HttpGet]
        [Description(" - Lab Kits Edit page.")]
        public ActionResult EditKitItems(int id = 0)
        {
            if (id > 0)
            {
                return View(GetLabKitItems(id));
            }
            else return View(new List<LabKitViewModel>());
        }

        [HttpGet]
        [Description(" - Lab Kit Items View page.")]
        public ActionResult ViewKitItems(int id = 0)
        {
            return View(GetLabKitItems(id));
        }

        public List<LabKitViewModel> GetLabKitItems(int lkitId)
        {
            List<LabKitViewModel> lKitItems = new List<LabKitViewModel>();
            using (HISDBEntities hs = new HISDBEntities())
            {
                string kitName = string.Empty;
                decimal kitCost = 0;
                var kitInfo = hs.TestTypes.Where(t => t.TestID == lkitId).FirstOrDefault();

                if(kitInfo != null)
                {
                    kitName = kitInfo.TestName;
                    kitCost = kitInfo.TestCost.HasValue ? kitInfo.TestCost.Value : 0;
                }

                bool hasItems = hs.LabKitItems.Where(li => li.LKitID == lkitId).Any();
                if(hasItems)
                {
                    lKitItems = (from lki in hs.LabKitItems
                                 join tt in hs.TestTypes on lki.TestID equals tt.TestID
                                 where lki.LKitID == lkitId
                                 select new { tt, lki }).AsEnumerable()
                             .Select(x => new LabKitViewModel
                             {
                                 LKitName = kitName,
                                 LKitCost = kitCost,
                                 TestID = x.tt.TestID,
                                 TestName = x.tt.TestName,
                                 LKitID = lkitId
                             }).ToList();
                }
                else
                {
                    lKitItems.Add(new LabKitViewModel() {
                        LKitID = lkitId,
                        LKitName = kitName,
                        LKitCost = kitCost,
                        TestID = 0,
                        TestName = string.Empty
                    });
                }
                
                return lKitItems;
            }
        }

        [HttpPost]
        public JsonResult SaveKitItems(List<LabKitViewModel> kitItems)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (kitItems != null && kitItems.Count() > 0)
                {
                    var kitID = kitItems[0].LKitID;
                    var items = db.LabKitItems.Where(x => x.LKitID == kitID).ToList();
                    

                    if (items.Count() == 0)
                    {
                        foreach (var kit in kitItems)
                        {
                            var labKitItem = new LabKitItem {
                                LKitID = kitID, TestID = kit.TestID
                            };
                            db.LabKitItems.Add(labKitItem);
                        }
                        db.SaveChanges();
                        return Json(new { success = true, message = "Lab Package created Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {   
                        db.LabKitItems.RemoveRange(items);

                        foreach (var kit in kitItems)
                        {
                            var labKitItem = new LabKitItem {
                                LKitID = kitID, TestID = kit.TestID
                            };
                            db.LabKitItems.Add(labKitItem);
                        }

                        TestType type = (from x in db.TestTypes
                                          where x.TestID == kitID
                                          select x).First();
                        type.TestCost = kitItems[0].LKitCost;
                        db.Entry(type).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Lab Package Updated Successfully" }, JsonRequestBehavior.AllowGet);
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