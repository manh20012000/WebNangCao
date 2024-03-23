using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using PagedList;
namespace ShopWatch.Areas.NhanVien.Controllers
{
    public class SALEsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/SALEs
        public ActionResult Index(int page = 1)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    int pageSize = 20;
                    var validSales = db.SALEs
                       .Where(s => s.NGAYHETHAN >= DateTime.Now && s.TRANGTHAI != true)
                        .ToList();
                    return View(validSales.ToList().ToPagedList(page, pageSize));
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // GET: NhanVien/SALEs/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SALE sALE = db.SALEs.Find(id);
            if (sALE == null)
            {
                return HttpNotFound();
            }
            return View(sALE);
        }

        // GET: NhanVien/SALEs/Create
        public ActionResult Create()
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/SALEs/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASALE,PHANTRAMSALE,NGAYBATDAU,NGAYHETHAN")] SALE sALE)
        {
            if (ModelState.IsValid)
            {
                sALE.TRANGTHAI = false;
                db.SALEs.Add(sALE);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sALE);
        }

        // GET: NhanVien/SALEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    SALE sALE = db.SALEs.Find(id);
                    if (sALE == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sALE);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/SALEs/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASALE,PHANTRAMSALE,NGAYBATDAU,NGAYHETHAN")] SALE sALE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sALE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sALE);
        }

        // GET: NhanVien/SALEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV QUANLYSP")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    SALE sALE = db.SALEs.Find(id);

                    if (sALE == null)
                    {
                        return HttpNotFound();
                    }
                    return View(sALE);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/SALEs/Delete/5
            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SALE sALE = db.SALEs.Find(id);
           /* db.SALEs.Remove(sALE);*/
            sALE.TRANGTHAI = true;
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
