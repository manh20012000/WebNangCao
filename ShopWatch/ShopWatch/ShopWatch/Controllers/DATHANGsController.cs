using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
namespace ShopWatch.Controllers
{
    [Route("api")]
    public class DATHANGsController : Controller
    {
        private DHEntities db = new DHEntities();

        // GET: DATHANGs
        public ActionResult Index(List<CHITIETDATHANG> selectedItemsData, double giatien)
        {
            if (Session["UserEmail"] != null)
            {
                Session["SelectedItemsData"] = selectedItemsData;
                var email = Session["UserEmail"] as string;
                KHACHHANG user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);
                var NGAYMUA = DateTime.Now;
                /*   var danhsachdonhang = db.DATHANGs.Where(m => m.MAKHACHHANG == user.MAKHACHHANG).ToList();*/
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
                return View(dathang);


            }
            return View();
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
        public ActionResult AccepDonhang(double giatien)
        {
            if (Session["UserEmail"] != null)
            {
                int length = 6;
                string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                string randomString = GenerateRandomString(length, characters);
                var email = Session["UserEmail"] as string;
                var ItemsData = Session["SelectedItemsData"] as List<CHITIETDATHANG>;
                var user = db.KHACHHANGs.FirstOrDefault(u => u.EMAIL == email);

                ViewBag.DIACHI = new SelectList(db.DIADIEMs.Where(dd => dd.MAKHACHHANG == user.MAKHACHHANG), "MAMATHANG", "TENHANG");
                if (ItemsData != null && ItemsData.Any())
                {

                    DATHANG newHoaDon = new DATHANG
                    {
                        MADH = randomString,
                        NGAYMUA = DateTime.Today,
                        MAKHACHHANG = user.MAKHACHHANG,
                        TONGTIEN = giatien,
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

                string id_hoadon = " ";// lấy id hóa đơn để gán vào cthd 

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

        private int? GetMaKH()
        {
            throw new NotImplementedException();
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
// GET: DATHANGs/Details/5
/*     public ActionResult Details(string id)
 {
     if (id == null)
     {
         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
     }
     DATHANG dATHANG = db.DATHANGs.Find(id);
     if (dATHANG == null)
     {
         return HttpNotFound();
     }
     return View(dATHANG);
 }

 // GET: DATHANGs/Create
 public ActionResult Create()
 {
     ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI");
     ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG");
     ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU");
     ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN");
     ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI");
     return View();
 }

 // POST: DATHANGs/Create
 // To protect from overposting attacks, enable the specific properties you want to bind to, for 
 // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
 [HttpPost]
 [ValidateAntiForgeryToken]
 public ActionResult Create([Bind(Include = "MADH,NGAYMUA,TONGTIEN,MAKHACHHANG,TRANGTHAI,MADIADIEM,MAQUANLYVOUCHER,MATHANHTOAN,MAVANDON")] DATHANG dATHANG)
 {
     if (ModelState.IsValid)
     {
         db.DATHANGs.Add(dATHANG);
         db.SaveChanges();
         return RedirectToAction("Index");
     }

     ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI", dATHANG.MADIADIEM);
     ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dATHANG.MAKHACHHANG);
     ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU", dATHANG.MAQUANLYVOUCHER);
     ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN", dATHANG.MATHANHTOAN);
     ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI", dATHANG.MAVANDON);
     return View(dATHANG);
 }

 // GET: DATHANGs/Edit/5
 public ActionResult Edit(string id)
 {
     if (id == null)
     {
         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
     }
     DATHANG dATHANG = db.DATHANGs.Find(id);
     if (dATHANG == null)
     {
         return HttpNotFound();
     }
     ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI", dATHANG.MADIADIEM);
     ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dATHANG.MAKHACHHANG);
     ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU", dATHANG.MAQUANLYVOUCHER);
     ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN", dATHANG.MATHANHTOAN);
     ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI", dATHANG.MAVANDON);
     return View(dATHANG);
 }

 // POST: DATHANGs/Edit/5
 // To protect from overposting attacks, enable the specific properties you want to bind to, for 
 // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
 [HttpPost]
 [ValidateAntiForgeryToken]
 public ActionResult Edit([Bind(Include = "MADH,NGAYMUA,TONGTIEN,MAKHACHHANG,TRANGTHAI,MADIADIEM,MAQUANLYVOUCHER,MATHANHTOAN,MAVANDON")] DATHANG dATHANG)
 {
     if (ModelState.IsValid)
     {
         db.Entry(dATHANG).State = EntityState.Modified;
         db.SaveChanges();
         return RedirectToAction("Index");
     }
     ViewBag.MADIADIEM = new SelectList(db.DIADIEMs, "MADIADIEM", "TENDIACHI", dATHANG.MADIADIEM);
     ViewBag.MAKHACHHANG = new SelectList(db.KHACHHANGs, "MAKHACHHANG", "TENKHACHHANG", dATHANG.MAKHACHHANG);
     ViewBag.MAQUANLYVOUCHER = new SelectList(db.QUANLYVOUCHERs, "MAQUANLYVOUCHER", "GHICHU", dATHANG.MAQUANLYVOUCHER);
     ViewBag.MATHANHTOAN = new SelectList(db.THANHTOANs, "MATHANHTOAN", "PHUONGTHUCTHANHTOAN", dATHANG.MATHANHTOAN);
     ViewBag.MAVANDON = new SelectList(db.TRANGTHAIGIAOHANGs, "MAVANDON", "VITRI", dATHANG.MAVANDON);
     return View(dATHANG);
 }

 // GET: DATHANGs/Delete/5
 public ActionResult Delete(string id)
 {
     if (id == null)
     {
         return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
     }
     DATHANG dATHANG = db.DATHANGs.Find(id);
     if (dATHANG == null)
     {
         return HttpNotFound();
     }
     return View(dATHANG);
 }

 // POST: DATHANGs/Delete/5
 [HttpPost, ActionName("Delete")]
 [ValidateAntiForgeryToken]
 public ActionResult DeleteConfirmed(string id)
 {
     DATHANG dATHANG = db.DATHANGs.Find(id);
     db.DATHANGs.Remove(dATHANG);
     db.SaveChanges();
     return RedirectToAction("Index");
 }


}
}
*/