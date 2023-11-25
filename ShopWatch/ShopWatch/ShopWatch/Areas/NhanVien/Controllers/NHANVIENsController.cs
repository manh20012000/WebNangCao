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
    public class NHANVIENsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/NHANVIENs
        public ActionResult Index()
        {

            string userEmail = Session["UserEmail"] as string;

            if (userEmail == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == userEmail);
            ViewBag.Avatar = nhanvien.AVATAR;
            if (nhanvien == null)
            {
                return HttpNotFound();
            }
            // Truyền thông tin nhân viên vào view
            return View(nhanvien);
        }

        // GET: NhanVien/NHANVIENs/Details/5
        public ActionResult Details()
        {
            // Lấy email từ session
            string userEmail = Session["UserEmail"] as string;

            if (userEmail == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nHANVIEN = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == userEmail);

            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }

       
            return View(nHANVIEN);
        }

        public ActionResult Create()
        {
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANV,TENNV,NGAYSINH,SDT,EMAIL")] NHANVIEN nhanvien)
        {
            if (ModelState.IsValid)
            {
                db.NHANVIENs.Add(nhanvien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", nhanvien.EMAIL);
            return View(nhanvien);
        }
        public ActionResult EditProfile(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanvien = db.NHANVIENs.Find(id);
            if ( nhanvien== null)
            {
                return HttpNotFound();
            }
            ViewBag.MANV = new SelectList(db.NHANVIENs, "MANV", "TENNV", nhanvien.MANV);
            return View(nhanvien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(NHANVIEN nhanvien)
        {
            var User = db.NHANVIENs.Find(nhanvien.MANV);
            if (ModelState.IsValid)
            { var fImage = Request.Files["imageFile"];
            if (fImage != null && fImage.ContentLength > 0)
            {  
                string fileName = fImage.FileName;
                string folderName = Server.MapPath("~/assets/Upload/" + fileName);
                   fImage.SaveAs(folderName);
                    User.AVATAR = "/assets/Upload/" + fileName;
            }
                User.NGAYSINH = nhanvien.NGAYSINH;
                User.SDT = nhanvien.SDT;
                User.TENNV = nhanvien.TENNV;
                   db.SaveChanges(); 
                ViewBag.Avatar = nhanvien.AVATAR;
                TempData["SuccessMessage"] = "cập nhât thành công";
                return RedirectToAction("Product", "MATHANGs");
            }
            ViewBag.EMAIL = new SelectList(db.TAIKHOANs, "EMAIL", "MATKHAU", nhanvien.EMAIL);
            return View(nhanvien);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nHANVIEN = db.NHANVIENs.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHANVIEN);
        }

        // POST: NhanVien/NHANVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NHANVIEN nHANVIEN = db.NHANVIENs.Find(id);
            db.NHANVIENs.Remove(nHANVIEN);
            db.SaveChanges();
            return RedirectToAction("Product");
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
