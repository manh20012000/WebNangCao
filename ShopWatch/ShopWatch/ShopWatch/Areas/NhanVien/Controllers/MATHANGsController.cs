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
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    [Authorize]
    public class MATHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: NhanVien/MATHANGs

        public ActionResult Product(string searchValue, int page = 1)
        {
            int pageSize = 6;
            if (Session["UserEmail"] != null)
            {
                string userEmail = Session["UserEmail"] as string;
                NHANVIEN nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == userEmail);
                Session["MaNV"] =nhanvien.MANV;
                Session["Avatar"] = nhanvien.AVATAR;
                var items = db.MATHANGs.Where(m => m.TRANGTHAI != true).AsQueryable();
                if (!string.IsNullOrEmpty(searchValue))
                {
                    items = items.Where(x =>
                     SqlFunctions.StringConvert((double)x.GIAHANG).Contains(searchValue) ||
                     x.TENHANG.Contains(searchValue) ||
                     SqlFunctions.PatIndex("%" + searchValue + "%", SqlFunctions.StringConvert((double)x.GIAHANG)) > 0 ||
                     SqlFunctions.PatIndex("%" + searchValue + "%", x.TENHANG) > 0
                 );
                }
                var pagedData = items.ToList().ToPagedList(page, pageSize);
                return View(pagedData);
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult CreateMatHang()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMatHang(MATHANG mathang)
        {
            if (ModelState.IsValid)
            {     
                try
                {
                    var fImage = Request.Files["imageFile"];
                    if (fImage != null && fImage.ContentLength > 0)
                    {
                        string fileName = fImage.FileName;
                        string folderName = Server.MapPath("~/assets/Upload/" + fileName);
                        fImage.SaveAs(folderName);
                        mathang.TRANGTHAI = false;
                        mathang.ANHSANPHAM = "/assets/Upload/" + fileName;
                        db.MATHANGs.Add(mathang);
                        db.SaveChanges();
                        return RedirectToAction("Product");
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} - Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
            return View(mathang);
        }
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mathang = db.MATHANGs.Find(id);
            if (mathang == null)
            {
                return HttpNotFound();
            }
            return View(mathang);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(MATHANG mathang)
        {
            Console.WriteLine(mathang.MAMATHANG);
            var product = db.MATHANGs.Find(mathang.MAMATHANG);
            if (ModelState.IsValid)
            {
                var fImage = Request.Files["imageFile"];
                if (fImage != null && fImage.ContentLength > 0)
                {
                    string fileName = fImage.FileName;
                    string folderName = Server.MapPath("~/assets/Upload/" + fileName);
                    fImage.SaveAs(folderName);
                    product.ANHSANPHAM = "/assets/Upload/" + fileName;
                 }
                //db.Entry(mathang).State = EntityState.Modified;
                // Find theo id, cập nhật từng field của obj
                   product.GIAHANG = mathang.GIAHANG;
                   product.KICHTHUOC = mathang.KICHTHUOC;
                   product.LOAI = mathang.LOAI;
                   product.NGAYSANXUAT = mathang.NGAYSANXUAT;
                   product.TENHANG = mathang.TENHANG;
                   product.TENHANGSANXUAT = mathang.TENHANGSANXUAT;
                   product.BAOHANH = mathang.BAOHANH;
                   db.SaveChanges();
                   TempData["SuccessMessage"] = "Thêm sản phẩm thành công";
                   return RedirectToAction("Product", "MATHANGs");
            }
            return View(product);
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
            MATHANG mathang = db.MATHANGs.Find(id);
            mathang.TRANGTHAI = true;
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
