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
/*using ShopWatch.Payment;*/
namespace ShopWatch.Controllers
{
    [Route("api")]
    public class DatHangController : AllController
    {
        DHEntities db = new DHEntities();
        public ActionResult DonHang()
        {
            var email = Session["EmailClient"] as string;
            KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
            int? khachhang = GetMaKH();
            var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == user.MAKHACHHANG).ToList();
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
        public ActionResult AccepDonhang(DATHANG dATHANG)
        {
            if (Session["EmailClient"] != null)
            {
                int length = 6;
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string randomString = GenerateRandomString(length, characters);
                var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                 if (dATHANG.HINHTHUCTHANHTOAN == false) // Kiểm tra hình thức thanh toán
                 {
                     var diadiem = db.DIADIEMs.Find(dATHANG.MADIADIEM);
                     TRANGTHAIGIAOHANG giaohang = new TRANGTHAIGIAOHANG
                     {
                         MAVANDON = (int)dATHANG.MAVANDON,
                         VITRI = diadiem.TENDIACHI,
                         THOIGIANGIAOHANG = DateTime.Now,
                     };
                     db.TRANGTHAIGIAOHANGs.Add(giaohang);
                     DATHANG dathang = new DATHANG
                     {
                         HINHTHUCTHANHTOAN = false,
                         TONGTIEN = dATHANG.TONGTIEN,
                         MAKHACHHANG = dATHANG.KHACHHANG.MAKHACHHANG,
                         MADIADIEM = dATHANG.MADIADIEM,
                         MAQUANLYVOUCHER = dATHANG.MAQUANLYVOUCHER,
                         NGAYMUA = dATHANG.NGAYMUA,
                         MAVANDON = dATHANG.MAVANDON,
                         TRANGTHAI = false,
                         MADH = randomString,
                     };
                     db.DATHANGs.Add(dathang);


                     foreach (var item in ItemsData)
                     {
                         CHITIETDATHANG ctdh = new CHITIETDATHANG
                         {
                             MAMATHANG = item.MAMATHANG,
                             SOLUONG = item.SOLUONG,
                             GIABAN = item.GIABAN,
                             MADH = randomString,
                         };
                         db.CHITIETDATHANGs.Add(ctdh);
                     }
                  if(dATHANG.MAQUANLYVOUCHER!=null){

                     var voucher = db.QUANLYVOUCHERs.Find(dATHANG.MAQUANLYVOUCHER);
                     voucher.TRANGTHAI = false;
                }
                     db.SaveChanges();
             }
                return RedirectToAction("DonHang");
            }
            return RedirectToAction("LoginUser", "TAIKHOANs");
        }

        [HttpPost]
        public ActionResult PayOnline(DATHANG Dathang, int TypePaymentVN)
        {
            var code = new { Success = false, Code = Dathang.HINHTHUCTHANHTOAN, Url = "" };
            if (ModelState.IsValid)
            {
                if (Session["EmailClient"] != null)
                {
                    int length = 6;
                    string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    string randomString = GenerateRandomString(length, characters);
                    var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                    if (Dathang.HINHTHUCTHANHTOAN == false) // Kiểm tra hình thức thanh toán
                    {
                        var diadiem = db.DIADIEMs.Find(Dathang.MADIADIEM);
                        TRANGTHAIGIAOHANG giaohang = new TRANGTHAIGIAOHANG
                        {
                            MAVANDON = (int)Dathang.MAVANDON,
                            VITRI = diadiem.TENDIACHI,
                            THOIGIANGIAOHANG = DateTime.Now,
                        };
                        db.TRANGTHAIGIAOHANGs.Add(giaohang);
                        DATHANG dathang = new DATHANG
                        {
                            HINHTHUCTHANHTOAN = false,
                            TONGTIEN = Dathang.TONGTIEN,
                            MAKHACHHANG = Dathang.KHACHHANG.MAKHACHHANG,
                            MADIADIEM = Dathang.MADIADIEM,
                            MAQUANLYVOUCHER = Dathang.MAQUANLYVOUCHER,
                            NGAYMUA = Dathang.NGAYMUA,
                            MAVANDON = Dathang.MAVANDON,
                            TRANGTHAI = false,
                            MADH = randomString,
                        };
                        db.DATHANGs.Add(dathang);


                        foreach (var item in ItemsData)
                        {
                            CHITIETDATHANG ctdh = new CHITIETDATHANG
                            {
                                MAMATHANG = item.MAMATHANG,
                                SOLUONG = item.SOLUONG,
                                GIABAN = item.GIABAN,
                                MADH = randomString,
                            };
                            db.CHITIETDATHANGs.Add(ctdh);
                        }
                        if (Dathang.MAQUANLYVOUCHER != null)
                        {

                            var voucher = db.QUANLYVOUCHERs.Find(Dathang.MAQUANLYVOUCHER);
                            voucher.TRANGTHAI = false;
                        }
                     /*   db.SaveChanges();*/
                    }
                    /*return RedirectToAction("DonHang");*/
                }
                var strSanPham = "";
                var thanhtien = decimal.Zero;
                var TongTien = decimal.Zero;
              /*  foreach (var sp in cart.Items)
                {
                    strSanPham += "<tr>";
                    strSanPham += "<td>" + sp.ProductName + "</td>";
                    strSanPham += "<td>" + sp.Quantity + "</td>";
                    strSanPham += "<td>" + WebBanHangOnline.Common.Common.FormatNumber(sp.TotalPrice, 0) + "</td>";
                    strSanPham += "</tr>";
                    thanhtien += sp.Price * sp.Quantity;
                }*/
                TongTien = thanhtien;
                string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}", Dathang.MADH);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", Dathang.KHACHHANG.TENKHACHHANG);
                contentCustomer = contentCustomer.Replace("{{Phone}}", Dathang.DIADIEM.SDT);
                contentCustomer = contentCustomer.Replace("{{Email}}", Dathang.KHACHHANG.EMAIL);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", Dathang.DIADIEM.TENDIACHI);
                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Dathang.TONGTIEN.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", Dathang.TONGTIEN.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopWatch.Common.Common.FormatNumber(TongTien, 0));
               /* ShopWatch.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);*/
                string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                contentCustomer = contentCustomer.Replace("{{MaDon}}", Dathang.MADH);
                contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", Dathang.KHACHHANG.TENKHACHHANG);
                contentCustomer = contentCustomer.Replace("{{Phone}}", Dathang.DIADIEM.SDT);
                contentCustomer = contentCustomer.Replace("{{Email}}", Dathang.KHACHHANG.EMAIL);
                contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", Dathang.DIADIEM.TENDIACHI);
                contentCustomer = contentCustomer.Replace("{{ThanhTien}}", Dathang.TONGTIEN.ToString());
                contentCustomer = contentCustomer.Replace("{{TongTien}}", Dathang.TONGTIEN.ToString());
                /*ShopWatch.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);*/
                code = new { Success = true, Code = Dathang.HINHTHUCTHANHTOAN, Url = "" };
                //var url = "";

                if (Dathang.HINHTHUCTHANHTOAN == true)
                {
                    var url = UrlPayment(Dathang, TypePaymentVN);
                    code = new { Success = true, Code = Dathang.HINHTHUCTHANHTOAN, Url = url };
                }


                //code = new { Success = true, Code = 1, Url = url };
                //return RedirectToAction("CheckOutSuccess");

            }

            return Json(code);
            /* Response.Redirect(code);*/
            /*return RedirectToAction("DonHang", "DatHang");*/
        }


        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        // thanh toán với payment vnpay
        public string  UrlPayment(DATHANG dathang, int typementVn )
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input

          

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (dathang.TONGTIEN*100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (typementVn == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (typementVn == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (typementVn == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" +dathang.MADH);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef",dathang.MADH.ToString());
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }

        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.DATHANGs.FirstOrDefault(x => x.MADH == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.TRANGTHAI = true;//đã thanh toán
                            db.DATHANGs.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
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
/*
public ActionResult AccepDonhang(double giatien)
{
    if (Session["EmailClient"] != null)
    {
        int length = 6;
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        string randomString = GenerateRandomString(length, characters);
        var email = Session["EmailClient"] as string;
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
            {
                MADH = randomString,
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
}*/  /* public ActionResult DatHang(DATHANG dathang)
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

                 };
                 db.DATHANGs.Add(newHoaDon);
                 db.SaveChanges();

                string id_hoadon =" ";// lấy id hóa đơn để gán vào cthd 

                 var id_giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == id_khachhang);// lấy id giỏ hàng qua id khách
                 var danhsachgiohang = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == id_giohang.MAGIOHANG).ToList();// lấy ds sp qua id giohang
                 foreach (var item in danhsachgiohang)
                 {
                     *//*CHITIETHOADON ctdhCreate = new CHITIETHOADON
                     {
                         MAHD = id_hoadon,
                         MAMATHANG = item.MAMATHANG,
                         SOLUONG = item.SOLUONGMUA,
                         GIABAN = item.DONGIA,

                     };
                     db.CHITIETHOADONs.Add(ctdhCreate);
 *//*
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
         }*/