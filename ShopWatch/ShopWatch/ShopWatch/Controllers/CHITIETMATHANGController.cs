using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
namespace ShopWatch.Controllers
{
    public class CHITIETMATHANGController : Controller
    {
        private DHEntities db = new DHEntities();
        // GET: CHITIETMATHANG
        
        public ActionResult Index(int? id_sanpham)
        {

            var sanpham = db.MATHANGs.Find(id_sanpham);

            if (sanpham != null)
            {
                return View(sanpham);
            }
            else
            {
                return RedirectToAction("homeIndex", "Home");
            }
            
        }

    }
}