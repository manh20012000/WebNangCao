using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
    public class DatHangMetaData
    {
        public string MADH { get; set; }
        public Nullable<System.DateTime> NGAYMUA { get; set; }
        public Nullable<double> TONGTIEN { get; set; }
        public KHACHHANG KHACHHANG { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }
        public Nullable<int> MADIADIEM { get; set; }
        public Nullable<int> MAQUANLYVOUCHER { get; set; }
        public Nullable<int> MATHANHTOAN { get; set; }
        public Nullable<int> MAVANDON { get; set; }
    }
}