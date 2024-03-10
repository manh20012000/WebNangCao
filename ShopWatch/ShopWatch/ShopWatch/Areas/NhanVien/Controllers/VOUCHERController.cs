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
    public class VOUCHERController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/VOUCHER

        public ActionResult VoucherIndex(int page = 1)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    int pageSize = 20;
                    var voucher = db.VOUCHERs;
                    return View(voucher.ToList().ToPagedList(page, pageSize));

                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

            // GET: NhanVien/VOUCHER/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VOUCHER vOUCHER = db.VOUCHERs.Find(id);
            if (vOUCHER == null)
            {
                return HttpNotFound();
            }
            return View(vOUCHER);
        }

        // GET: NhanVien/VOUCHER/Create
        public ActionResult Create()
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/VOUCHER/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAVOUCHER,NGAYBATDAU,NGAYKETTHUC,PHANTRAMGIAMGIA,DIEUKIEN,TRANGTHAI")] VOUCHER vOUCHER)
        {
            if (ModelState.IsValid)
            {
                db.VOUCHERs.Add(vOUCHER);
                db.SaveChanges();
                return RedirectToAction("VoucherIndex","VOUCHER");
            }

            return View(vOUCHER);
        }

        // GET: NhanVien/VOUCHER/Edit/5
        public ActionResult Edit(int? id)
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
                    VOUCHER vOUCHER = db.VOUCHERs.Find(id);
                    if (vOUCHER == null)
                    {
                        return HttpNotFound();
                    }
                    return View(vOUCHER);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/VOUCHER/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAVOUCHER,NGAYBATDAU,NGAYKETTHUC,PHANTRAMGIAMGIA,DIEUKIEN,TRANGTHAI")] VOUCHER vOUCHER)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    if (ModelState.IsValid)
            {
                db.Entry(vOUCHER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VoucherIndex");
            }
            return View(vOUCHER);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        // GET: NhanVien/VOUCHER/Delete/5
        public ActionResult Delete(int? id)
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
                    VOUCHER vOUCHER = db.VOUCHERs.Find(id);
                    if (vOUCHER == null)
                    {
                        return HttpNotFound();
                    }
                    return View(vOUCHER);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
            // POST: NhanVien/VOUCHER/Delete/5
            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    VOUCHER vOUCHER = db.VOUCHERs.Find(id);
            vOUCHER.TRANGTHAI = true;
        
            db.SaveChanges();
            return RedirectToAction("VoucherIndex");
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
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
