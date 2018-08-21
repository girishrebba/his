using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using HIS.HtmlHelpers;

namespace HIS.Controllers
{
    public class LabController : Controller
    {
        // GET: Lab
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTestTypes()
        {
            using (HISDBEntities hs = new HISDBEntities())
            {
                var testtypes = (from tt in hs.TestTypes
                                        select new
                                        {
                                            tt                                           
                                        }).AsEnumerable()
                                        .Select(x => new TestType
                                        {
                                            TestName = x.tt.TestName,
                                            TestCost = x.tt.TestCost,
                                            TestID = x.tt.TestID
                                        })
                                        .ToList();

                return Json(new { data = testtypes }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddModify(int id = 0)
        {
            //List<User> Users = HtmlHelpers.HtmlHelpers.GetUsers();
            List<TestType> TestTypes = HtmlHelpers.HtmlHelpers.GetTestTypes();
            if (id == 0)
            {
                ViewBag.TestTypes = new SelectList(TestTypes, "TestID", "TestType");
                return View(new TestType());
            }
            else
            {
                var testType = GetTestType(id);
                if (testType != null)
                {
                    ViewBag.TestTypes = new SelectList(TestTypes, "TestID", "TestType", testType.TestID);
                    return View(testType);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        //Get contact by ID
        public TestType GetTestType(int TestID)
        {
            TestType testType = new TestType();
            using (HISDBEntities dc = new HISDBEntities())
            {
                var v = (from cf in dc.TestTypes
                         where cf.TestID.Equals(TestID)
                         select new
                         {
                             cf.TestName,
                             cf.TestCost
                         }).FirstOrDefault();
                if (v != null)
                {
                    testType.TestName = v.TestName;
                    testType.TestCost = v.TestCost;
                }
                return testType;
            }
        }

        [HttpPost]
        public ActionResult AddModify(TestType tt)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                if (tt.TestID == 0)
                {
                    db.TestTypes.Add(tt);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(tt).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (HISDBEntities db = new HISDBEntities())
            {
                TestType tt = db.TestTypes.Where(x => x.TestID == id)
                    .FirstOrDefault<TestType>();
                db.TestTypes.Remove(tt);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}