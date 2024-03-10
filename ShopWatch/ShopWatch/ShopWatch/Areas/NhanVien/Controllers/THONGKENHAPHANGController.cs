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
    public class THONGKENHAPHANGController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/THONGKENHAPHANG
        public ActionResult Index()
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV KETOAN")
                {
                    return View(db.THONGKENHAPHANGs.ToList());
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
