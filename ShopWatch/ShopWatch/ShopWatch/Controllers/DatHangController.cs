using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
namespace ShopWatch.Controllers
{
    public class DatHangController : AllController
    {
        DHEntities db = new DHEntities();
        // GET: DatHang

        public ActionResult DonHang()
        {
            int? khachhang = GetMaKH();
            var danhsachdonhang = db.HOADONs.Where(m => m.MAKHACHHANG == khachhang).ToList();
            return View(danhsachdonhang);

        }
        public ActionResult DatHang()
        {

            try
            {
                int? id_khachhang = GetMaKH();
                var khachhang = db.KHACHHANGs.Find(id_khachhang);
                HOADON newHoaDon = new HOADON
                {
                    NGAYMUA = DateTime.Today,
                    MAKHACHHANG = id_khachhang,
                    DIACHINHANHANG = khachhang.DIACHI,
                    TONGTIEN = 0,
                    PHUONGTHUCTHANHTOAN = "chuyển khoản",
                };
                db.HOADONs.Add(newHoaDon);
                db.SaveChanges();

                int id_hoadon = newHoaDon.MAHD;// lấy id hóa đơn để gán vào cthd 

                var id_giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == id_khachhang);// lấy id giỏ hàng qua id khách
                var danhsachgiohang = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == id_giohang.MAGIOHANG).ToList();// lấy ds sp qua id giohang
                foreach (var item in danhsachgiohang)
                {
                    CHITIETHOADON ctdhCreate = new CHITIETHOADON
                    {
                        MAHD = id_hoadon,
                        MAMATHANG = item.MAMATHANG,
                        SOLUONG = item.SOLUONGMUA,
                        GIABAN = item.DONGIA,
                       
                    };
                    db.CHITIETHOADONs.Add(ctdhCreate);

                }

                db.SaveChanges();
                db.CHITIETGIOHANGs.RemoveRange(danhsachgiohang);// xóa toàn bộ sp giỏ



                var tongtien = db.CHITIETHOADONs.Where(m => m.MAHD == id_hoadon)// cập  nhật tiền
                                                .Sum(m => m.GIABAN * m.SOLUONG);

                var donhang = db.HOADONs.Find(id_hoadon);
                donhang.TONGTIEN = tongtien;
                db.SaveChanges();
                return RedirectToAction("DonHang");


            }
            catch (DbUpdateException ex)
            {
                // In ra thông tin chi tiết về lỗi
                Console.WriteLine(ex.Message);

                // Nếu có inner exception, in ra thông tin chi tiết của nó
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
            return RedirectToAction("form_DatHang", "GioHang");
        }
    }
}