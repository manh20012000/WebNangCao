using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopWatch.Models;

namespace ShopWatch.Areas.NhanVien.Controllers
{
 
    public class NHAPHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/NHAPHANGs
        public ActionResult Index(string searchValue)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    var items = db.NHAPHANGs.Where(m => m.TRANGTHAI != true).AsQueryable();
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        items = items.Where(x =>
                              SqlFunctions.StringConvert((double)x.THANHTIEN).Contains(searchValue) ||
                              SqlFunctions.DatePart("dd", x.NGAYNHAP).ToString().Contains(searchValue) ||
                              SqlFunctions.DatePart("MM", x.NGAYNHAP).ToString().Contains(searchValue) ||
                              SqlFunctions.DatePart("yyyy", x.NGAYNHAP).ToString().Contains(searchValue)
      );
                    }
                    return View(items.ToList());
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        // GET: NhanVien/NHAPHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANG nHAPHANG = db.NHAPHANGs.Find(id);
            if (nHAPHANG == null)
            {
                return HttpNotFound();
            }
                    return View(nHAPHANG);
                }

                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        // GET: NhanVien/NHAPHANGs/Create
        public ActionResult Create()
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    var newReceipt = new NHAPHANG
                    {
                        NGAYNHAP = DateTime.Today // Gán ngày hiện tại cho trường NGAYNHAP
                    };
                    ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV");
                    return View(newReceipt);
                }
            
            return RedirectToAction("Index", "BackToPemission");
        }
            return RedirectToAction("LoginUser", "TAIKHOANs");
    }

        // POST: NhanVien/NHAPHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NHAPHANG nhaphang)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    if (ModelState.IsValid)
            {
                nhaphang.THANHTIEN = 0;
                nhaphang.TRANGTHAI = false;
                nhaphang.MANV = (int?)Session["MaNV"];
                nhaphang.MANV = (int?)Session["MaNV"];
                db.NHAPHANGs.Add(nhaphang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", nhaphang.MANV);
            return View(nhaphang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
       

        // GET: NhanVien/NHAPHANGs/Edit/5
        public ActionResult EditNhapHang(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANG nHAPHANG = db.NHAPHANGs.Find(id);
            if (nHAPHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", nHAPHANG.MANV);
            return View(nHAPHANG);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNhapHang(NHAPHANG nhaphang)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    if (ModelState.IsValid)
            {
                db.Entry(nhaphang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", nhaphang.MANV);
            return View(nhaphang);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

      /*  public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANG nHAPHANG = db.NHAPHANGs.Find(id);
            if (nHAPHANG == null)
            {
                return HttpNotFound();
            }
            return View(nHAPHANG);
        }
      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            NHAPHANG nHAPHANG = db.NHAPHANGs.Find(id);
            if (nHAPHANG != null)
            {
            
                var chiTietPhieuNhaps = db.CHITIETPHIEUNHAPs.Where(ct => ct.MANHAPHANG == id);
                foreach (var chiTiet in chiTietPhieuNhaps)
                {
                    db.CHITIETPHIEUNHAPs.Remove(chiTiet);
                }
 
                db.NHAPHANGs.Remove(nHAPHANG);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }*/

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
