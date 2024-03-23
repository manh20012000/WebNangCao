using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class THONGKENHAPHANGController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/THONGKENHAPHANG
        public ActionResult Index(int? month, int? year)
        {
            if (Session["UserEmail"] != null)
            {
                double Money = 0.0;
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV KETOAN")
                {
                    if (month.HasValue && year.HasValue)
                    {
                        var thongke = db.NHAPHANGs
                                    .Where(nh => nh.NGAYNHAP.Value.Month == month && nh.NGAYNHAP.Value.Year == year);
                        if (thongke != null)
                        {
                            
                            var thongkenhaphang = (
                                from ct in db.CHITIETPHIEUNHAPs
                                join nh in db.NHAPHANGs.Where(n => n.NGAYNHAP.Value.Month == month && n.NGAYNHAP.Value.Year == year)
                                    on ct.MANHAPHANG equals nh.MANHAPHANG
                                select new DataCHITIETNHAPHANG
                                {
                                    SOLUONG = ct.SOLUONG,
                                    TONGTIEN = ct.SOLUONG * ct.GIANHAP,
                                    NGAYTHONGKE =nh.NGAYNHAP,
                                    MANHAPHANG = nh.MANHAPHANG,
                                    TENHANG = ct.MATHANG.TENHANG,
                                    TENNHANVIEN=nh.NHANVIEN.TENNV,
                                }
                            ).ToList();
                           
                             foreach(var item in thongkenhaphang)
                            {
                                Money += item.TONGTIEN ?? 0;
                            }
                            ViewBag.Tongtien = Money;

                            return View(thongkenhaphang);
                        }
                    }
                    ViewBag.Tongtien = 0.0;
                    return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        // GET: NhanVien/THONGKENHAPHANG/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKENHAPHANG tHONGKENHAPHANG = db.THONGKENHAPHANGs.Find(id);
            if (tHONGKENHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKENHAPHANG);
        }

        // GET: NhanVien/THONGKENHAPHANG/Create
        public ActionResult Create() { 
         if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV KETOAN")
                {
            return View();
        }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }


        // POST: NhanVien/THONGKENHAPHANG/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATHONGKE,NGAYTHONGKE,TONGTIEN")] THONGKENHAPHANG tHONGKENHAPHANG)
        {
            if (ModelState.IsValid)
            {
                db.THONGKENHAPHANGs.Add(tHONGKENHAPHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHONGKENHAPHANG);
        }

        // GET: NhanVien/THONGKENHAPHANG/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKENHAPHANG tHONGKENHAPHANG = db.THONGKENHAPHANGs.Find(id);
            if (tHONGKENHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKENHAPHANG);
        }

        // POST: NhanVien/THONGKENHAPHANG/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATHONGKE,NGAYTHONGKE,TONGTIEN")] THONGKENHAPHANG tHONGKENHAPHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHONGKENHAPHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHONGKENHAPHANG);
        }

        // GET: NhanVien/THONGKENHAPHANG/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKENHAPHANG tHONGKENHAPHANG = db.THONGKENHAPHANGs.Find(id);
            if (tHONGKENHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKENHAPHANG);
        }

        // POST: NhanVien/THONGKENHAPHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            THONGKENHAPHANG tHONGKENHAPHANG = db.THONGKENHAPHANGs.Find(id);
            db.THONGKENHAPHANGs.Remove(tHONGKENHAPHANG);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
