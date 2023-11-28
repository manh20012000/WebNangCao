using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopWatch.Controllers
{
    public class AllController : Controller
    {
        // GET: All
        protected void SetMaKH(int makhachhang)
        {
            Session["makhachhang"] = makhachhang;
        }

        protected int? GetMaKH()
        {
            return Session["makhachhang"] as int?;
        }
    }
}