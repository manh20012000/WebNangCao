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
    public class KHACHHANGsController : AllController
    {
        private DHEntities db = new DHEntities();

        // GET: KHACHHANGs
        public ActionResult Index()
        {
            var kHACHHANGs = db.KHACHHANGs.Include(k => k.TAIKHOAN);
            return View(kHACHHANGs.ToList());
        }

        // GET: KHACHHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Create
        public ActionResult Create()
        {
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU");
            return View();
        }

        // POST: KHACHHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAKHACHHANG,TENKHACHHANG,DIACHI,SDT,EMAIL,AVATAR")] KHACHHANG kHACHHANG)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.Add(kHACHHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", kHACHHANG.EMAIL);
            return View(kHACHHANG);
        }

        // GET: KHACHHANGs/Edit/5
        public ActionResult Edit()
        {
            try
            {
                int? makhachhang = GetMaKH();
                if (makhachhang == null)
                {
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
                var kHACHHANG = db.KHACHHANGs.Find(makhachhang);
                return View(kHACHHANG);
            }
            catch(Exception ex)
            {
                Console.WriteLine("loi" + ex.Message);
            }
            return RedirectToAction("homeIndex","Home");
   
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KHACHHANG model)
        {
            var update = db.KHACHHANGs.Find(model.MAKHACHHANG);
            update.TENKHACHHANG = model.TENKHACHHANG;
            update.SDT = model.SDT;
           /* update.DIACHI = model.DIACHI;*/
            var kt = db.SaveChanges();
            if(kt > 0)
            {
                return View(model);
            }
            return View(model);
        }

        // GET: KHACHHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            if (kHACHHANG == null)
            {
                return HttpNotFound();
            }
            return View(kHACHHANG);
        }

        // POST: KHACHHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANG kHACHHANG = db.KHACHHANGs.Find(id);
            db.KHACHHANGs.Remove(kHACHHANG);
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
