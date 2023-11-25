using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaData
{
   
    public class NHAPHANG_METADATA
    {
        
        public int MANHAPHANG { get; set; }
        public Nullable<int> MAMATHANG { get; set; }
        public Nullable<int> SOLUONGNHAP { get; set; }
        public Nullable<double> GIANHAP { get; set; }
        public virtual ICollection<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        public virtual MATHANG MATHANG { get; set; }
    }
}