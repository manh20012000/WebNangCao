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
    public class MATHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/MATHANGs
        public ActionResult Index()
        {/*
            if (Session["LoggedInUser"] == null)
            {
                return RedirectToAction("LoginUser", "TAIKHOANs");
            }*/
            return View(db.MATHANGs.ToList());
        }
       

        // GET: NhanVien/MATHANGs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }
        /* [HttpGet]
        // GET: NhanVien/MATHANGs/Create
        public ActionResult Create()
        {
            return View();
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MATHANG mathang)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fImage = Request.Files["imageFile"];
                    Console.WriteLine(fImage);

                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = fImage.FileName;
                        string folderName = Server.MapPath("~/assets/Upload/" + fileName);
                        fImage.SaveAs(folderName);
                        mathang.ANHSANPHAM = "/assets/Upload/" + fileName;
                    }

                    db.MATHANGs.Add(mathang);
                    db.SaveChanges();

                    // Lấy danh sách sản phẩm sau khi thêm thành công
                    var productList = db.MATHANGs.ToList();

                    return RedirectToAction("Index", "MATHANGs");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            // Nếu không thành công, hiển thị biểu mẫu để tạo sản phẩm mới
            return View(mathang);
        }

        // GET: NhanVien/MATHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }

        // POST: NhanVien/MATHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAMATHANG,TENHANG,ANHSANPHAM,NGAYSANXUAT,TENHANGSANXUAT,DONGIA")] MATHANG mATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mATHANG);
        }

        // GET: NhanVien/MATHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }

        // POST: NhanVien/MATHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MATHANG mATHANG = db.MATHANGs.Find(id);
            db.MATHANGs.Remove(mATHANG);
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
