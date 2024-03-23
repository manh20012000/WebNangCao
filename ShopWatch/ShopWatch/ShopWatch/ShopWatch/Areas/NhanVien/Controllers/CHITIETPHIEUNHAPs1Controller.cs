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
    public class CHITIETPHIEUNHAPs1Controller : Controller
    {
        private DHEntities db = new DHEntities();


        public ActionResult Index(string searchValue)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    var items = db.CHITIETPHIEUNHAPs.AsQueryable(); // Ensure it's queryable

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        // Use ToString() to convert GIANHAP to string for comparison
                        items = items.Where(x => x.GIANHAP.ToString().Contains(searchValue));
                    }

                    var pagedData = items.ToList();
                    return View(pagedData);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        public ActionResult Create(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    
                db.Configuration.ProxyCreationEnabled = false;

                ViewBag.NHAPHANG = db.NHAPHANGs.Find(id);
                ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG");

                return View();
                    }
                    return RedirectToAction("Index", "BackToPemission");
                }
                return RedirectToAction("LoginUser", "TAIKHOANs");
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CHITIETPHIEUNHAP chitietphieunhap)
        {
            string phanquyen = Session["phanquyen"] as string;
            if (phanquyen == "NV NHAPHANG") { 
             
                if (ModelState.IsValid)
                {
                    db.CHITIETPHIEUNHAPs.Add(chitietphieunhap);
                    db.SaveChanges();
                    var upadateNhapHang = db.NHAPHANGs.Find(chitietphieunhap.MANHAPHANG);
                    upadateNhapHang.THANHTIEN = chitietphieunhap.GIANHAP + upadateNhapHang.THANHTIEN;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(chitietphieunhap.MANHAPHANG);
            }
            return RedirectToAction("Index", "BackToPemission");
        }
        public ActionResult CreateMulti(int? id,int page = 1)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    db.Configuration.ProxyCreationEnabled = false;

                ViewBag.NHAPHANG = db.NHAPHANGs.Find(id);
                ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG");
                int pageSize = 6;
                var items = db.MATHANGs.Where(m => m.TRANGTHAI != true).AsQueryable();
                db.Configuration.ProxyCreationEnabled = false;

                ViewBag.NHAPHANG = db.NHAPHANGs.Find(id);
                ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG");
                return View(items.ToList().ToPagedList(page, pageSize));
            
            }
            return RedirectToAction("Index", "BackToPemission");
        }
            return RedirectToAction("LoginUser", "TAIKHOANs");
    }
        [HttpPost]
            public ActionResult CreateMulti(List<CHITIETPHIEUNHAP> listInput)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                        var THANHTIEN = 0;
                        int idNhapHang = 1;
                        if (listInput != null && listInput.Any())
                        {
                            foreach (var chitietphieunhap in listInput)
                            {  
                                THANHTIEN = (int)(double)(THANHTIEN + chitietphieunhap.GIANHAP * chitietphieunhap.SOLUONG);
                                idNhapHang = (int)chitietphieunhap.MANHAPHANG;
                                chitietphieunhap.GIANHAP *= chitietphieunhap.SOLUONG;
                                Console.WriteLine(chitietphieunhap + "ch tiet phiếu nhập");
                                db.CHITIETPHIEUNHAPs.Add(chitietphieunhap);
                            }
                            var nhahang = db.NHAPHANGs.Find(idNhapHang);
                            nhahang.THANHTIEN = nhahang.THANHTIEN + THANHTIEN;

                            db.SaveChanges();
                        }
                    return RedirectToAction("Index", "CHITIETPHIEUNHAPs1");
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult DetailReceipt(int? id)
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
                CHITIETPHIEUNHAP ctphieunhap = db.CHITIETPHIEUNHAPs.Find(id);
                if (ctphieunhap == null)
                {
                    return HttpNotFound();
                }
                var product = db.MATHANGs.Find(ctphieunhap.MAMATHANG);

                ViewBag.product = product;
                var Receipt = db.NHAPHANGs.Find(ctphieunhap.MANHAPHANG);

                ViewBag.Receipt = Receipt;
                return View(ctphieunhap);
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        // GET: NhanVien/CHITIETPHIEUNHAPs1/Delete/5
         

        public ActionResult deleteReceipt(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    Console.WriteLine(id);
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
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        [HttpPost, ActionName("deleteReceipt")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    CHITIETPHIEUNHAP phieunhap = db.CHITIETPHIEUNHAPs.Find(id);

                if (phieunhap != null)
                {
                    var Receipt = db.NHAPHANGs.Find(phieunhap.MANHAPHANG);
                    Receipt.THANHTIEN = Receipt.THANHTIEN - phieunhap.GIANHAP;
                    db.CHITIETPHIEUNHAPs.Remove(phieunhap);
                    db.SaveChanges();

                }
                return RedirectToAction("Index");
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
       
    


    public ActionResult EditDetail(int? id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    db.Configuration.ProxyCreationEnabled = false;
                var CTPhieuNhap = db.CHITIETPHIEUNHAPs.Find(id);
                ViewBag.phieunhap = CTPhieuNhap;
                ViewBag.NHAPHANG = db.NHAPHANGs.Find(CTPhieuNhap.MANHAPHANG);
                ViewBag.MAMATHANG = db.MATHANGs.Find(CTPhieuNhap.MAMATHANG);
                return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditDetail(CHITIETPHIEUNHAP cHITIETPHIEUNHAP)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    try
                    {
                        var nhaphang = db.NHAPHANGs.Find(cHITIETPHIEUNHAP.MANHAPHANG);
                        var ctphieunhap = db.CHITIETPHIEUNHAPs.Find(cHITIETPHIEUNHAP.MACTPHIEUNHAP);
                        var mathang = db.MATHANGs.Find(cHITIETPHIEUNHAP.MAMATHANG);
                        if (ModelState.IsValid)
                        {
                            if (nhaphang == null || ctphieunhap == null)
                            {
                                return View();
                            }
                            if (cHITIETPHIEUNHAP.SOLUONG < ctphieunhap.SOLUONG)
                            {

                                nhaphang.THANHTIEN -= mathang.GIAHANG * (ctphieunhap.SOLUONG - cHITIETPHIEUNHAP.SOLUONG);
                            }
                            else if (cHITIETPHIEUNHAP.SOLUONG > ctphieunhap.SOLUONG)
                            {

                                nhaphang.THANHTIEN += mathang.GIAHANG * (cHITIETPHIEUNHAP.SOLUONG - ctphieunhap.SOLUONG);
                            }
                            else if (cHITIETPHIEUNHAP.SOLUONG == ctphieunhap.SOLUONG)
                            {
                                nhaphang.THANHTIEN = nhaphang.THANHTIEN;
                            }
                            ctphieunhap.SOLUONG = cHITIETPHIEUNHAP.SOLUONG;
                            ctphieunhap.GIANHAP = cHITIETPHIEUNHAP.GIANHAP;
                            /* db.Entry(cHITIETPHIEUNHAP).State = EntityState.Modified;*/
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    catch { }

                    return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult PromissonDetails(int id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var PromisissorDetails = db.NHAPHANGs.Find(id);
                    return Json(PromisissorDetails, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex);
                }
                return View();
                }
                return RedirectToAction("Index", "BackToPemission");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");

        }
        public ActionResult ProductDetails(String id)
        {
            if (Session["UserEmail"] != null)
            {
                string phanquyen = Session["phanquyen"] as string;
                if (phanquyen == "NV NHAPHANG")
                {
                    try
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var PromisissorDetails = db.MATHANGs.Find(id);
                    return Json(PromisissorDetails, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex);
                }
                return View();
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
/*public ActionResult EditDetailPromissoryNote(int? id)
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
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG", cHITIETPHIEUNHAP.MAMATHANG);
            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG", cHITIETPHIEUNHAP.MANHAPHANG);
            return View(cHITIETPHIEUNHAP);
        }
        // POST: NhanVien/CHITIETPHIEUNHAPs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetailPromissoryNote(CHITIETPHIEUNHAP EditDetailPromissoryNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(EditDetailPromissoryNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAMATHANG = new SelectList(db.MATHANGs, "MAMATHANG", "TENHANG", EditDetailPromissoryNote.MAMATHANG);
            ViewBag.MANHAPHANG = new SelectList(db.NHAPHANGs, "MANHAPHANG", "MANHAPHANG", EditDetailPromissoryNote.MANHAPHANG);
            return View(EditDetailPromissoryNote);
        }
*/