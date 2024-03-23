using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
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
            var user = Session["EmailClient"] as string;

            if (user != null)
            {
                var khachhang = db.KHACHHANGs.FirstOrDefault(m => m.EMAIL == user);

                if (khachhang != null)
                {
                    var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == khachhang.MAKHACHHANG);

                    if (giohang != null)
                    {
                        var items = from chiteitgiohang in db.CHITIETGIOHANGs

                                    where chiteitgiohang.MAGIOHANG == giohang.MAGIOHANG
                                    select new ChitietgiohangViewModel
                                    {
                                         MACHITIETGIOHANG=chiteitgiohang.MACHITIETGIOHANG,
                                         SOLUONGMUA=chiteitgiohang.SOLUONGMUA,
                                         GIOHANG=chiteitgiohang.GIOHANG,
                                         MATHANG=new MathangViewModel
                                         {
                                           ANHSANPHAM=chiteitgiohang.MATHANG.ANHSANPHAM,
                                            MAMATHANG = chiteitgiohang.MATHANG.MAMATHANG,
                                            TENHANG = chiteitgiohang.MATHANG.TENHANG,
                                            GIAHANG = chiteitgiohang.MATHANG.GIAHANG,
                                            NGAYSANXUAT = chiteitgiohang.MATHANG.NGAYSANXUAT,
                                            TENHANGSANXUAT = chiteitgiohang.MATHANG.TENHANGSANXUAT,
                                            BAOHANH = chiteitgiohang.MATHANG.BAOHANH,
                                            LOAI = chiteitgiohang.MATHANG.LOAI,
                                            KICHTHUOC = chiteitgiohang.MATHANG.KICHTHUOC,
                                            SALE = chiteitgiohang.MATHANG.SALE.TRANGTHAI == false ? chiteitgiohang.MATHANG.SALE : null
                                        },
                                    };
                      
                        return View(items.ToList());
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
            if (ViewBag.id_khachhang != null)
            {
                return View(db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang).ToList());
            }
            return View();
        }
        
        public ActionResult AddToCart(String sanpham)
        {
            var user = Session["EmailClient"] as string;

            if (user != null)
            {
                var Khachhang = db.KHACHHANGs.FirstOrDefault(m => m.EMAIL == user);
                var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == Khachhang.MAKHACHHANG);
                var checksp = db.CHITIETGIOHANGs.FirstOrDefault(ms => ms.MAGIOHANG == giohang.MAGIOHANG && ms.MAMATHANG == sanpham);
                if (checksp!=null)
                {
                    return RedirectToAction("Index", "GioHang");
                }
                        db.CHITIETGIOHANGs.Add(new CHITIETGIOHANG
                        {
                            MAGIOHANG = giohang.MAGIOHANG,
                            MAMATHANG = sanpham,
                            SOLUONGMUA = 1,
                           
                        });
                   
            }
            db.SaveChanges();
            return RedirectToAction("Index","GioHang");
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
        public ActionResult form_DatHang(string  selectedItemsData, double giatien)
        {
            if (Session["EmailClient"] != null)
            {
                string decodedData = HttpUtility.UrlDecode(selectedItemsData);
                // Chuyển đổi chuỗi JSON thành mảng đối tượng
                var itemsArray = JsonConvert.DeserializeObject<List<CHITIETDATHANG>>(decodedData);
                var email = Session["EmailClient"] as string;
                Session["SelectedItemsData"] = itemsArray;
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var NGAYMUA = DateTime.Now;
                var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == user.MAKHACHHANG).ToList();
                Random random = new Random();
                int Numrd = random.Next(1000000, 9000000);
                var quanLyVoucherList = db.QUANLYVOUCHERs
                      .Where(quanLyVoucher => quanLyVoucher.MAKHACHHANG == user.MAKHACHHANG &&
                            quanLyVoucher.TRANGTHAI == true &&
                            quanLyVoucher.NGAYKETTHUC >= DateTime.Today)
                            .Include(quanLyVoucher => quanLyVoucher.KHACHHANG)
                            .Include(quanLyVoucher => quanLyVoucher.VOUCHER)
                            .ToList();

                ViewBag.VOUCHER = new SelectList(quanLyVoucherList.ToList(), "VOUCHER");
                ViewBag.danhsachGH = selectedItemsData;
                ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG).ToList(),"MADIADIEM");
                DatHangMetaData dathang = new DatHangMetaData
                {
                    NGAYMUA = NGAYMUA,
                    KHACHHANG = user,
                    TONGTIEN = giatien,
                    MAVANDON = Numrd,
                };
                
                return View("form_DatHang", dathang);


            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");
        }

        // đăt hàng
        /* public ActionResult form_DatHang()
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
         }*/
       /* [HttpPost]
        public ActionResult form_DatHang(KHACHHANG model)
        {
            var update = db.KHACHHANGs.Find(model.MAKHACHHANG);
            update.TENKHACHHANG = model.TENKHACHHANG;
            update.SDT = model.SDT;
   *//*         update.DIACHI = model.DIACHI;*//*
            db.SaveChanges();

            var giohang = db.GIOHANGs.FirstOrDefault(m => m.MAKHACHHANG == model.MAKHACHHANG);
            if (giohang != null)
            {
                ViewBag.danhsachGH = db.CHITIETGIOHANGs.Where(ctgh => ctgh.MAGIOHANG == giohang.MAGIOHANG).ToList();
            }
            return View(model);
        }*/

    }
}