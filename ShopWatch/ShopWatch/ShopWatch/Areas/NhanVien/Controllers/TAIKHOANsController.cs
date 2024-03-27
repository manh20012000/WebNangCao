using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShopWatch.Models;

using System.Security.Cryptography;
using System.Text;

namespace ShopWatch.Areas.NhanVien.Controllers
{
    /* [Authorize]*/
    public class TAIKHOANsController : Controller
    {
        private DHEntities db = new DHEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginUser()
        {

            return View();

        }
        [HttpPost]

        public ActionResult LoginUser(TAIKHOAN taikhoan)
        {
            string f_password = GetMD5(taikhoan.MATKHAU);

            try
            {
                TAIKHOAN check = db.TAIKHOANs.FirstOrDefault(tk => tk.EMAIL == taikhoan.EMAIL && tk.MATKHAU == f_password);

                if (check == null)
                {
                    ModelState.AddModelError("", "tài khoản hoặc mật khẩu không chính xác");
                    return View();
                }

                /*FormsAuthentication.SetAuthCookie(check.EMAIL, false);*/
                NHANVIEN nhanvien = db.NHANVIENs.FirstOrDefault(nv => nv.EMAIL == check.EMAIL);
                if (nhanvien != null)
                {
                    Session["UserEmail"] = check.EMAIL;
                    Session["TenHienThi"] = check.TENDANGNHAP;
                    Session["phanquyen"] = nhanvien.TENCHUCNANG;
                    Session["MANV"] = nhanvien.MANV;
                    Session["Avatar"] = nhanvien.AVATAR;
                    if (nhanvien.TENCHUCNANG == "NV QUANLYSP")
                    {
                        return RedirectToAction("Product", "MATHANGs");
                    }
                    else if (nhanvien.TENCHUCNANG == "NV NHAPHANG")
                    {
                        return RedirectToAction("index", "NHAPHANGs");
                    }
                    else
                    {
                        return RedirectToAction("index", "THONGKEs");
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng nhập"+ ex.InnerException?.Message);
            }
            return View();
        }




        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TAIKHOAN taikhoan)
        {
            if (ModelState.IsValid)
            {
                TAIKHOAN check = db.TAIKHOANs.FirstOrDefault(tk => tk.EMAIL == taikhoan.EMAIL);
                if (check == null)
                {
                    taikhoan.MATKHAU = GetMD5(taikhoan.MATKHAU);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.TAIKHOANs.Add(taikhoan);
                    db.SaveChanges();
                    var nhanVien = new NHANVIEN
                    {
                        // Gán các giá trị từ tAIKHOAN
                        EMAIL = taikhoan.EMAIL,
                        TENCHUCNANG = taikhoan.PHANQUYEN
                        // Các thuộc tính khác của NHANVIEN
                    };
                    db.NHANVIENs.Add(nhanVien);
                    db.SaveChanges();
                    return RedirectToAction("LoginUser", "TAIKHOANs");
                }
                else
                {
                    ModelState.AddModelError("", "Email này đã tồn tại");
                    return View();

                }

            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }
        public ActionResult getMail()
        {

            return View();
        }

        [HttpPost, ActionName("getMail")]
        [ValidateAntiForgeryToken]
        public ActionResult getMail(string email)
        {

            int[] a = new int[6];
            TAIKHOAN taikhoan = db.TAIKHOANs.Find(email);

            if (taikhoan != null)
            {
                Session["Email"] = taikhoan.EMAIL;
                return RedirectToAction("Confirm", "TAIKHOANs");
            }
            return View();
        }

        public ActionResult Confirm()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("LoginUser", "TAIKHOANs");
            }
            int[] a = new int[6];
            Random rn = new Random();
            for (int j = 0; j < 6; j++)
            {
                a[j] = rn.Next(10);
            }
            string alertMessage = "Mã đăng nhập: ";
            foreach (var item in a)
            {
                alertMessage += item.ToString() + " ";
            }

            Session["CheckPass"] = a;
            Session["Pass"] = alertMessage;
            return View();
        }
        [HttpPost, ActionName("Confirm")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(TAIKHOAN tk, List<int> digits)
        {

            if (string.IsNullOrEmpty(tk.MATKHAU) || digits == null || digits.Count != 6)
            {
                TempData["AlertMessage"] = "Dữ liệu không hợp lệ!";
                return View();
            }
            else
            {
                var user = Session["Email"] as string;
                int[] generatedDigits = Session["CheckPass"] as int[];

                if (generatedDigits.SequenceEqual(digits))
                {
                    TAIKHOAN taikhoan = db.TAIKHOANs.Find(user);

                    if (taikhoan != null)
                    {
                        taikhoan.MATKHAU = GetMD5(tk.MATKHAU) + "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        return RedirectToAction("LoginUser", "TAIKHOANs");
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Không tìm thấy người dùng!";
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "Email hoặc mã xác thực không đúng!";
                }
            }


            return View();
        }
        private string GetMD5(string mATKHAU)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(mATKHAU);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;

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
/* public ActionResult LoginUser(TAIKHOAN taikhoan)
        {
           // if (ModelState.IsValid)
            {*//*
                try
            {
              string f_password = GetMD5(taikhoan.MATKHAU);

                TAIKHOAN check = db.TAIKHOANs.FirstOrDefault(tk => tk.EMAIL == taikhoan.EMAIL &&  tk.MATKHAU == f_password);
                    if (check == null)
                    {
                        ModelState.AddModelError("", "tài khoản hoặc mật khẩu không chính xác");
                        return View();
                    }
                Session["UserEmail"] = check.EMAIL;
                Session["TenHienThi"] = check.TENDANGNHAP;
                TempData["AlertMessage"] = "đăng nhập thành công";
                return RedirectToAction("Product", "MATHANGs");
                }
                catch
                {       
            }
            return View();
        }*/
