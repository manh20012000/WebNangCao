using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
using ShopWatch.Models.MetaDATA;
namespace ShopWatch.Controllers
{
   
    public class HomeController : Controller
    {
        private DHEntities db = new DHEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult homeIndex(string searchValue)
        {
            if (Session["EmailClient"] != null)
            {
                var items = from mathang in db.MATHANGs
                            where mathang.TRANGTHAI == false
                            select new MathangViewModel
                            {
                                MAMATHANG = mathang.MAMATHANG,
                                TENHANG = mathang.TENHANG,
                                GIAHANG = mathang.GIAHANG,
                                NGAYSANXUAT = mathang.NGAYSANXUAT,
                                TENHANGSANXUAT = mathang.TENHANGSANXUAT,
                                BAOHANH = mathang.BAOHANH,
                                LOAI = mathang.LOAI,
                                KICHTHUOC = mathang.KICHTHUOC,
                                ANHSANPHAM = mathang.ANHSANPHAM,
                                SALE = mathang.SALE.TRANGTHAI == false ? mathang.SALE : null
                            };
                if (!string.IsNullOrEmpty(searchValue))
            {
                items = items.Where(x =>
                       SqlFunctions.StringConvert((double)x.GIAHANG).Contains(searchValue) ||
                       x.TENHANG.Contains(searchValue) ||
                       SqlFunctions.PatIndex("%" + searchValue + "%", SqlFunctions.StringConvert((double)x.GIAHANG)) > 0 ||
                       SqlFunctions.PatIndex("%" + searchValue + "%", x.TENHANG) > 0
                   );
            }
            else
            {
                var pagedData = items.ToList();
                return View(pagedData);
            }

            return View(items.ToList());

            }
            return RedirectToAction("Dangnhap", "TAIKHOANs");
               
        }
      
       
       public ActionResult danhsach()
        {
            return View();
        }
    }
}