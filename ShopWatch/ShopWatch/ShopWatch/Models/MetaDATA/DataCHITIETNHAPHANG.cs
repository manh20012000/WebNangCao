using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
    public class DataCHITIETNHAPHANG
    {
        public string MAMATHANG { get; set; }
        public int MACTPHIEUNHAP { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<int> MANHAPHANG { get; set; }
        public Nullable<double> TONGTIEN { get; set; }
        public Nullable<System.DateTime> NGAYTHONGKE { get; set; }
        public String TENNHANVIEN { get; set; }
        public  NHAPHANG NHAPHANG { get; set; }
        public  String  TENHANG { get; set; }
    }
}