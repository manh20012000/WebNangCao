using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;

namespace ShopWatch.Controllers
{
    public class TAIKHOANsController : AllController
    {
        private DHEntities db = new DHEntities();

        // GET: TAIKHOANs
        public ActionResult Index()
        {
            return View(db.TAIKHOANs.ToList());
        }
        public ActionResult Register()
        {
            return View();
        }
        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                var check = db.TAIKHOANs.FirstOrDefault(s => s.EMAIL == tAIKHOAN.EMAIL);
                if (check == null)
                {
                    tAIKHOAN.MATKHAU = GetMD5(tAIKHOAN.MATKHAU);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.TAIKHOANs.Add(tAIKHOAN);
                    db.SaveChanges();
                    var Khachhang = new KHACHHANG
                    {
                        // Gán các giá trị từ tAIKHOAN
                        EMAIL = tAIKHOAN.EMAIL,
                        // Các thuộc tính khác của NHANVIEN
                    };
                    db.KHACHHANGs.Add(Khachhang);
                    db.SaveChanges();
                    return RedirectToAction("Dangnhap", "TAIKHOANs");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();


        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(tAIKHOAN.MATKHAU);
                var data = db.TAIKHOANs.Where(s => s.EMAIL.Equals(tAIKHOAN.EMAIL) && s.MATKHAU.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    var data_khachhang = db.KHACHHANGs.Where(s => s.EMAIL.Equals(tAIKHOAN.EMAIL)).FirstOrDefault();
                    if (data_khachhang != null)
                    {
                        SetMaKH(data_khachhang.MAKHACHHANG);
                        return RedirectToAction("homeIndex", "Home");
                    }
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View("Login");
                }
            }
            return View();
        }

        // GET: TAIKHOANs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TAIKHOANs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMAIL,MATKHAU,TENHIENTHI")] TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANs.Add(tAIKHOAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // POST: TAIKHOANs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMAIL,MATKHAU,TENHIENTHI")] TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAIKHOAN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tAIKHOAN);
        }

        // GET: TAIKHOANs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }

        // POST: TAIKHOANs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            db.TAIKHOANs.Remove(tAIKHOAN);
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
