﻿using System;
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
    [Authorize]
    public class THONGKEsController : Controller
    {
        private DHEntities db = new DHEntities();


        public ActionResult Index(int?month, int?year)
        {


            if (month.HasValue && year.HasValue)
            {
                var tongTien = db.HOADONs
                    .Where(hd => hd.NGAYMUA.Value.Month == month && hd.NGAYMUA.Value.Year == year)
                    .Sum(hd => hd.TONGTIEN);
                var existingThongKe = db.THONGKEs.FirstOrDefault(tk => tk.NGAYTHONGKE.Value.Month == month && tk.NGAYTHONGKE.Value.Year == year);

                var maHoaDons = db.HOADONs
               .Where(hd => hd.NGAYMUA.Value.Month == month && hd.NGAYMUA.Value.Year == year)
               .Select(hd => hd.MAHD)
               .ToList();

                var maMatHangBanNhieuNhat = db.CHITIETHOADONs
                    .Where(ct => maHoaDons.Contains(ct.MAHD.Value)) // Sử dụng Contains trên List
                    .GroupBy(ct => ct.MAMATHANG)
                    .OrderByDescending(g => g.Sum(ct => ct.SOLUONG))
                    .Select(g => g.Key)
                    .FirstOrDefault();


                var soSanPhamBanRa = db.CHITIETHOADONs
                    .Where(ct => maHoaDons.Contains(ct.MAHD.Value))
                    .Sum(ct => ct.SOLUONG);
                var tongSoSanPhamchay = db.CHITIETHOADONs
                  .Where(ct => ct.MAMATHANG == maMatHangBanNhieuNhat && maHoaDons.Contains(ct.MAHD.Value))
                     .Sum(ct => ct.SOLUONG);
                var mathang = db.MATHANGs.Find(maMatHangBanNhieuNhat);
                if (existingThongKe != null)
                {

                    existingThongKe.TONGTIEN = tongTien;
                    existingThongKe.SOLUONGBANRA = soSanPhamBanRa;
                    existingThongKe.MASPBANCHAY = maMatHangBanNhieuNhat + "";
                    existingThongKe.SANPHAMBANCHAY = mathang.TENHANG;
                    existingThongKe.SOLUONGBANCHAY = tongSoSanPhamchay;

                    db.SaveChanges();
                }
                else
                {
                    THONGKE thongke = new THONGKE();
                    thongke.NGAYTHONGKE = DateTime.Now;
                    thongke.TONGTIEN = tongTien;
                    thongke.SOLUONGBANRA = soSanPhamBanRa;
                    thongke.MASPBANCHAY = maMatHangBanNhieuNhat + "";
                    thongke.SANPHAMBANCHAY = mathang.TENHANG;
                    thongke.SOLUONGBANCHAY = tongSoSanPhamchay;
                    db.THONGKEs.Add(thongke);
                }


                db.SaveChanges();


            }


            return View(db.THONGKEs.ToList());
        }

        // GET: NhanVien/THONGKEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKE tHONGKE = db.THONGKEs.Find(id);
            if (tHONGKE == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKE);
        }

        // GET: NhanVien/THONGKEs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhanVien/THONGKEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATHONGKE,NGAYTHONGKE,TONGTIEN,SANPHAMBANCHAY,SOLUONGBANRA,DHCO,DHDIENTU,SMATWATCH,DHTHOITRANG,DHCAOCAP")] THONGKE tHONGKE)
        {
            if (ModelState.IsValid)
            {
                db.THONGKEs.Add(tHONGKE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tHONGKE);
        }

        // GET: NhanVien/THONGKEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKE tHONGKE = db.THONGKEs.Find(id);
            if (tHONGKE == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKE);
        }

        // POST: NhanVien/THONGKEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATHONGKE,NGAYTHONGKE,TONGTIEN,SANPHAMBANCHAY,SOLUONGBANRA,DHCO,DHDIENTU,SMATWATCH,DHTHOITRANG,DHCAOCAP")] THONGKE tHONGKE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHONGKE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHONGKE);
        }

        // GET: NhanVien/THONGKEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGKE tHONGKE = db.THONGKEs.Find(id);
            if (tHONGKE == null)
            {
                return HttpNotFound();
            }
            return View(tHONGKE);
        }

        // POST: NhanVien/THONGKEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            THONGKE tHONGKE = db.THONGKEs.Find(id);
            db.THONGKEs.Remove(tHONGKE);
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
