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
        public ActionResult Index()
        {
            //int? id_khachhang = GetMaKH();
            int id_khachhang = 4;
            if (id_khachhang != null)
            {
                var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == id_khachhang);
                if (giohang != null)
                {
                    return View(db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang.MAGIOHANG).ToList());
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(int giohang)
        {
            return View(db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang).ToList());
        }

        public ActionResult AddToCart(int? sanpham)
        {
            //int? khachhang = GetMaKH();
            int khachhang = 4;
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
      

    }
}