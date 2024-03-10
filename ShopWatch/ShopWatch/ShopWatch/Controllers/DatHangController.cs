using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
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
            var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == khachhang).ToList();
            return View(danhsachdonhang);

        }
       

      
    

    static string GenerateRandomString(int length, string characters)
    {
        Random random = new Random();
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            // Chọn một ký tự ngẫu nhiên từ chuỗi characters
            int index = random.Next(characters.Length);
            char randomChar = characters[index];

            // Thêm ký tự ngẫu nhiên vào chuỗi kết quả
            result.Append(randomChar);
        }

        return result.ToString();
    }

     [HttpPost]
        public ActionResult PhieuDatHang(List<CHITIETDATHANG> selectedItemsData, double giatien)
        {

            if (Session["UserEmail"] != null)
            { int length = 6;
              string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string randomString = GenerateRandomString(length, characters);
                    var email=  Session["UserEmail"] as string;
                var user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
               
                    if (selectedItemsData != null && selectedItemsData.Any())
                    {   
                       
                        DATHANG newHoaDon = new DATHANG
                        {   MADH = randomString,
                            NGAYMUA = DateTime.Today,
                            MAKHACHHANG = user.MAKHACHHANG,
                            TONGTIEN = giatien,
                        };
                    
                        foreach (var chitietdathang in selectedItemsData)
                        {
                            CHITIETDATHANG ctdhCreate = new CHITIETDATHANG
                       {
                         MADH = randomString,
                         MAMATHANG = chitietdathang.MAMATHANG,
                         SOLUONG = chitietdathang.SOLUONG,
                         GIABAN = chitietdathang.GIABAN,

                         };
                        db.CHITIETDATHANGs.Add(ctdhCreate);
                        }

                        db.DATHANGs.Add(newHoaDon);
                   
                        db.SaveChanges();
                      return RedirectToAction("DonHang", newHoaDon);
                      }
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        public ActionResult DatHang()
        {
            try
            {
                int? id_khachhang = GetMaKH();
                var khachhang = db.KHACHHANGs.Find(id_khachhang);
                DATHANG newHoaDon = new DATHANG
                {
                    NGAYMUA = DateTime.Today,
                    MAKHACHHANG = id_khachhang,
                   /* DIACHINHANHANG = khachhang.DIACHI,*/
                    TONGTIEN = 0,
                  /*  PHUONGTHUCTHANHTOAN = "chuyển khoản",*/
                };
                db.DATHANGs.Add(newHoaDon);
                db.SaveChanges();

               string id_hoadon =" ";// lấy id hóa đơn để gán vào cthd 

                var id_giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == id_khachhang);// lấy id giỏ hàng qua id khách
                var danhsachgiohang = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == id_giohang.MAGIOHANG).ToList();// lấy ds sp qua id giohang
                foreach (var item in danhsachgiohang)
                {
                    /*CHITIETHOADON ctdhCreate = new CHITIETHOADON
                    {
                        MAHD = id_hoadon,
                        MAMATHANG = item.MAMATHANG,
                        SOLUONG = item.SOLUONGMUA,
                        GIABAN = item.DONGIA,
                       
                    };
                    db.CHITIETHOADONs.Add(ctdhCreate);
*/
                }

                db.SaveChanges();
                db.CHITIETGIOHANGs.RemoveRange(danhsachgiohang);// xóa toàn bộ sp giỏ



                var tongtien = db.CHITIETDATHANGs.Where(m => m.MADH == id_hoadon)// cập  nhật tiền
                                                .Sum(m => m.GIABAN * m.SOLUONG);

                var donhang = db.DATHANGs.Find(id_hoadon);
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