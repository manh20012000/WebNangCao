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

namespace ShopWatch.Areas.NhanVien.Controllers
{
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
            if (Session["LoggedInUser"] != null)
            {
                return RedirectToAction("Index", "MATHANGs");
            }
            return View();

        }
        public ActionResult LoginUser(string email, string password)
        {

           
  
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
               
                return View();
            }

            TAIKHOAN taikhoan = db.TAIKHOANs.FirstOrDefault(tk => tk.EMAIL == email && tk.MATKHAU == password);
            if (taikhoan == null)
            {

                return View();
            }


            // Lưu thông tin vào Session
            Session["LoggedInUser"] = taikhoan;
            return RedirectToAction("Index", "MATHANGs");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                db.TAIKHOANs.Add(tAIKHOAN);
                db.SaveChanges();
                return RedirectToAction("/NhanVien/TAIKHOANs/Index");
            }

            return View();
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
