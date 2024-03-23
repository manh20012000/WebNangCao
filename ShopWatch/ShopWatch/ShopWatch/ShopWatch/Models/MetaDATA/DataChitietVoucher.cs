using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaDATA
{
    public class DataChitietVoucher
    {
        public int MAQUANLYVOUCHER { get; set; }
        public Nullable<int> MAKHACHHANG { get; set; }
        public Nullable<int> MAVOUCHER { get; set; }
        public string GHICHU { get; set; }
        public string NOIDUNG { get; set; }
        public Nullable<System.DateTime> NGAYBATDAU { get; set; }
        public Nullable<System.DateTime> NGAYKETTHUC { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DATHANG> DATHANGs { get; set; }
        public virtual KHACHHANG KHACHHANG { get; set; }
        public virtual VOUCHER VOUCHER { get; set; }
    }
}