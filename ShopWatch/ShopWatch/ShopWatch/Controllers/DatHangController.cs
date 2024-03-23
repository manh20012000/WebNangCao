using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
using ShopWatch.Payment;

namespace ShopWatch.Controllers
{
    [Route("api")]
    public class DatHangController : AllController
    {
        DHEntities db = new DHEntities();
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
        public ActionResult AccepDonhang( double giatien)
        {
            if (Session["EmailClient"] != null)
            { int length = 6;
              string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string randomString = GenerateRandomString(length, characters);
                    var email=  Session["EmailClient"] as string;
                var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                var user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var quanLyVoucherList = db.QUANLYVOUCHERs
                     .Where(quanLyVoucher => quanLyVoucher.MAKHACHHANG == user.MAKHACHHANG)
                     .Include(quanLyVoucher => quanLyVoucher.KHACHHANG)
                     .Include(quanLyVoucher => quanLyVoucher.VOUCHER);
                    
                ViewBag.VOUCHER = new SelectList(quanLyVoucherList);
                ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG));
                if (ItemsData != null && ItemsData.Any())
                    {

                    DATHANG newHoaDon = new DATHANG
                    { MADH = randomString,
                        NGAYMUA = DateTime.Today,
                        MAKHACHHANG = user.MAKHACHHANG,
                        TONGTIEN = giatien,
                        KHACHHANG = user,
                        };
                    
                        foreach (var chitietdathang in ItemsData)
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
        // thanh toán với payment vnpay
       /* public string btnPay_Click(object sender, EventArgs e)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            //Save order to db

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (bankcode_Vnpayqr.Checked == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (bankcode_Vnbank.Checked == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (bankcode_Intcard.Checked == true)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

            if (locale_Vn.Checked == true)
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            else if (locale_En.Checked == true)
            {
                vnpay.AddRequestData("vnp_Locale", "en");
            }
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            Response.Redirect(paymentUrl);
        }*/

        public ActionResult DatHang(DATHANG dathang)
        {
            try
            {
                int? id_khachhang = GetMaKH();
                var khachhang = db.KHACHHANGs.Find(id_khachhang);
                DATHANG newHoaDon = new DATHANG
                {
                    NGAYMUA = DateTime.Today,
                    MAKHACHHANG = id_khachhang,
                    TRANGTHAI=dathang.TRANGTHAI,
                    TONGTIEN = dathang.TONGTIEN,
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} /* public ActionResult DonHang(List<CHITIETDATHANG> selectedItemsData, double giatien)
        {
            if (Session["UserEmail"] != null)
            {
                Session["SelectedItemsData"] = selectedItemsData;
                var email = Session["UserEmail"] as string;
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var NGAYMUA = DateTime.Now;
                var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == user.MAKHACHHANG).ToList();
                Random random = new Random();
                int Numrd = random.Next(1000000, 9000000);
                ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG), "MAMATHANG", "TENHANG");
                DatHangMetaData dathang = new DatHangMetaData
                {
                    NGAYMUA = NGAYMUA,
                    KHACHHANG = user,
                    TONGTIEN = giatien,
                    MAVANDON = Numrd,
                };
                var datHangList = new List<DatHangMetaData> { dathang };
                return View("DonHang", datHangList);


            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");


        }*/