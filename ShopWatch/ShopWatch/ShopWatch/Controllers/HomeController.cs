using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWatch.Models;
namespace ShopWatch.Controllers
{
   
    public class HomeController : Controller
    {
        private DHEntities db = new DHEntities();
        public ActionResult homeIndex(string searchValue)
        {
            var items = db.MATHANGs.Where(m => m.TRANGTHAI != true).AsQueryable();
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
      
       
       public ActionResult danhsach()
        {
            return View();
        }
    }
}