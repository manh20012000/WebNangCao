using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
    public class MathangViewModel
    {
        public string MAMATHANG { get; set; }
        public string TENHANG { get; set; }
        public DateTime? NGAYSANXUAT { get; set; }
        public string TENHANGSANXUAT { get; set; }
        public string ANHSANPHAM { get; set; }
        public double? GIAHANG { get; set; }
        public string BAOHANH { get; set; }
        public string LOAI { get; set; }
        public int? KICHTHUOC { get; set; }
        public bool? TRANGTHAI { get; set; }
        public SALE SALE { get; set; }
    }
}