using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
    public class ChitietgiohangViewModel
    {
        public int MACHITIETGIOHANG { get; set; }
        
        public Nullable<int> SOLUONGMUA { get; set; }
        public GIOHANG GIOHANG { get; set; }
        public  MathangViewModel MATHANG { get; set; }
    }
}