using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class CHITIETPHIEUNHAPsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/CHITIETPHIEUNHAPs
        public ActionResult Index()
        {
            var cHITIETPHIEUNHAPs = db.CHITIETPHIEUNHAPs.Include(c => c.NHAPHANG).Include(c => c.NHANVIEN);
            return View(cHITIETPHIEUNHAPs.ToList());
        }
        // GET: NhanVien/CHITIETPHIEUNHAPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETPHIEUNHAP cHITIETPHIEUNHAP = db.CHITIETPHIEUNHAPs.Find(id);
            if (cHITIETPHIEUNHAP == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETPHIEUNHAP);
        }

        // GET: NhanVien/CHITIETPHIEUNHAPs/Create
        public ActionResult Create()
        {
            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG");
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV");
            return View();
        }

        // POST: NhanVien/CHITIETPHIEUNHAPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MACTPHIEUNHAP,MANHAPHANG,THANHTIEN,NGAYNHAP,MANV")] CHITIETPHIEUNHAP cHITIETPHIEUNHAP)
        {
            if (ModelState.IsValid)
            {
                db.CHITIETPHIEUNHAPs.Add(cHITIETPHIEUNHAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG", cHITIETPHIEUNHAP.MANHAPHANG);
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", cHITIETPHIEUNHAP.MANV);
            return View(cHITIETPHIEUNHAP);
        }

        // GET: NhanVien/CHITIETPHIEUNHAPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETPHIEUNHAP cHITIETPHIEUNHAP = db.CHITIETPHIEUNHAPs.Find(id);
            if (cHITIETPHIEUNHAP == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG", cHITIETPHIEUNHAP.MANHAPHANG);
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", cHITIETPHIEUNHAP.MANV);
            return View(cHITIETPHIEUNHAP);
        }

        // POST: NhanVien/CHITIETPHIEUNHAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MACTPHIEUNHAP,MANHAPHANG,THANHTIEN,NGAYNHAP,MANV")] CHITIETPHIEUNHAP cHITIETPHIEUNHAP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHITIETPHIEUNHAP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG", cHITIETPHIEUNHAP.MANHAPHANG);
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", cHITIETPHIEUNHAP.MANV);
            return View(cHITIETPHIEUNHAP);
        }

        // GET: NhanVien/CHITIETPHIEUNHAPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETPHIEUNHAP cHITIETPHIEUNHAP = db.CHITIETPHIEUNHAPs.Find(id);
            if (cHITIETPHIEUNHAP == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETPHIEUNHAP);
        }

        // POST: NhanVien/CHITIETPHIEUNHAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHITIETPHIEUNHAP cHITIETPHIEUNHAP = db.CHITIETPHIEUNHAPs.Find(id);
            db.CHITIETPHIEUNHAPs.Remove(cHITIETPHIEUNHAP);
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
