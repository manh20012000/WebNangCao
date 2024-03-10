using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;

namespace ShopWatch.Controllers
{
    public class CHITIETDATHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: CHITIETDATHANGs
        public ActionResult Index()
        {
            var cHITIETDATHANGs = db.CHITIETDATHANGs.Include(c => c.DATHANG).Include(c => c.MATHANG);
            return View(cHITIETDATHANGs.ToList());
        }

        // GET: CHITIETDATHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDATHANG cHITIETDATHANG = db.CHITIETDATHANGs.Find(id);
            if (cHITIETDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETDATHANG);
        }

        // GET: CHITIETDATHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "MADH");
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG");
            return View();
        }

        // POST: CHITIETDATHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MADH,MAMATHANG,SOLUONG,GIABAN,MACTDH")] CHITIETDATHANG cHITIETDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.CHITIETDATHANGs.Add(cHITIETDATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "MADH", cHITIETDATHANG.MADH);
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG", cHITIETDATHANG.MAMATHANG);
            return View(cHITIETDATHANG);
        }

        // GET: CHITIETDATHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDATHANG cHITIETDATHANG = db.CHITIETDATHANGs.Find(id);
            if (cHITIETDATHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "MADH", cHITIETDATHANG.MADH);
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG", cHITIETDATHANG.MAMATHANG);
            return View(cHITIETDATHANG);
        }

        // POST: CHITIETDATHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MADH,MAMATHANG,SOLUONG,GIABAN,MACTDH")] CHITIETDATHANG cHITIETDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cHITIETDATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MADH = new SelectList(db.DATHANGs, "MADH", "MADH", cHITIETDATHANG.MADH);
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG", cHITIETDATHANG.MAMATHANG);
            return View(cHITIETDATHANG);
        }

        // GET: CHITIETDATHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CHITIETDATHANG cHITIETDATHANG = db.CHITIETDATHANGs.Find(id);
            if (cHITIETDATHANG == null)
            {
                return HttpNotFound();
            }
            return View(cHITIETDATHANG);
        }

        // POST: CHITIETDATHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CHITIETDATHANG cHITIETDATHANG = db.CHITIETDATHANGs.Find(id);
            db.CHITIETDATHANGs.Remove(cHITIETDATHANG);
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
