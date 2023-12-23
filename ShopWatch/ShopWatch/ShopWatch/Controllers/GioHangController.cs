using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
namespace ShopWatch.Controllers
{
    public class GioHangController : AllController
    {
        DHEntities db = new DHEntities();
        public ActionResult GH()
        {
            return View(db.CHITIETGIOHANGs.ToList());
        }
        // GET: GioHang
        [HttpGet]
        public ActionResult Index()// cái này có tác dụng để xem ( tức ng dùng ko ấn vào add to cart mà ấn vào giỏ hnagf ngay 
        {

            var user = Session["UserEmail"] as string;
            if (user != null)
            {
                   var khachhang = db.KHACHHANGs.FirstOrDefault(m => m.EMAIL == user);
            // int? id_khachhang = GetMaKH();
          
            if (khachhang != null)
            {
                var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == khachhang.MAKHACHHANG);

                if (giohang != null)
                {
                    return View(db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang.MAGIOHANG).ToList());
                }
            }
            return View();
            }
            else
            {
                return RedirectToAction("Dangnhap", "TAIKHOANs");
            }
         
        }
        [HttpPost]
        public ActionResult Index(int giohang)//lấy id giỏ hàng truy vấn nhanh hơn 
        {
            ViewBag.id_khachhang = GetMaKH();
            if ( ViewBag.id_khachhang != null)
            {
                return View(db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang).ToList());
            }
            return View();
        }

        public ActionResult AddToCart(int? sanpham)
        {
            int? khachhang = GetMaKH();
            
            int id_giohang = check_giohang(khachhang);
            var find_sp = db.MATHANGs.Find(sanpham);
            var check_sp = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == id_giohang && ctgh.MAMATHANG == sanpham).FirstOrDefault();
            if(check_sp == null)
            {
                db.CHITIETGIOHANGs.Add(new CHITIETGIOHANG
                {
                    MAGIOHANG = id_giohang,
                    MAMATHANG = sanpham,
                    SOLUONGMUA = 1,
                    DONGIA = find_sp.GIAHANG
                });
            }
            else
            {
                check_sp.SOLUONGMUA += 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index", new { giohang = id_giohang });
        }
        public ActionResult up_number(int id_ctgh, int number)
        {
            var chiTietGioHang = db.CHITIETGIOHANGs.Find(id_ctgh);

            if (chiTietGioHang != null)
            {
                // Đảm bảo số lượng không âm
                chiTietGioHang.SOLUONGMUA += number;
                if (chiTietGioHang.SOLUONGMUA > 0)
                {
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", new { id_giohang = chiTietGioHang.MAGIOHANG });
        }

        private int check_giohang(int? khachhang)
        {
            var GioHang = db.GIOHANGs.FirstOrDefault(g => g.MAKHACHHANG == khachhang);
            if(GioHang == null)
            {
                var newCart = new GIOHANG { MAKHACHHANG = khachhang };
                db.GIOHANGs.Add(newCart);
                db.SaveChanges();

                return newCart.MAGIOHANG;
            }
            return GioHang.MAGIOHANG;
        }
        public ActionResult xoa_sanpham(int? id_ctgh)
        {
            var find_ctgh = db.CHITIETGIOHANGs.Find(id_ctgh);
            if (find_ctgh != null)
            {
                db.CHITIETGIOHANGs.Remove(find_ctgh);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
      
        // đăt hàng
        public ActionResult form_DatHang()
        {
            int? khachhang = GetMaKH();
            if (khachhang != null)
            {
                var thongtinkh = db.KHACHHANGs.Find(khachhang);
                var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == khachhang);
                if (giohang != null)
                {
                    ViewBag.danhsachGH = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang.MAGIOHANG).ToList();
                }
                return View(thongtinkh);
            }
            return View();
        }
        [HttpPost]
        public ActionResult form_DatHang(KHACHHANG model)
        {
            var update = db.KHACHHANGs.Find(model.MAKHACHHANG);
            update.TENKHACHHANG = model.TENKHACHHANG;
            update.SDT = model.SDT;
            update.DIACHI = model.DIACHI;
            db.SaveChanges();

            var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == model.MAKHACHHANG);
            if (giohang != null)
            {
                ViewBag.danhsachGH = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang.MAGIOHANG).ToList();
            }
            return View(model);
        }

    }
}