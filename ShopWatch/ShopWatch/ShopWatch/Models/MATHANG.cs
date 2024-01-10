﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopWatch.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MATHANG()
        {
            this.CHITIETGIOHANGs = new HashSet<CHITIETGIOHANG>();
            this.CHITIETHOADONs = new HashSet<CHITIETHOADON>();
            this.CHITIETPHIEUNHAPs = new HashSet<CHITIETPHIEUNHAP>();
        }


        public int MAMATHANG { get; set; }
        [Required(ErrorMessage = "Please Enter Name Product")]
        public string TENHANG { get; set; }

        public string ANHSANPHAM { get; set; }
        [Required(ErrorMessage = "Please Enter date")]
        public Nullable<System.DateTime> NGAYSANXUAT { get; set; }
        [Required(ErrorMessage = "Please Enter TenHang")]
        public string TENHANGSANXUAT { get; set; }
        [Required(ErrorMessage = "Please Enter GiaHang")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Vui lòng chỉ nhập số.")]
        [DataType(DataType.Currency, ErrorMessage = "Vui lòng nhập giá trị tiền tệ.")]
        public Nullable<decimal> GIAHANG { get; set; }
        [Required(ErrorMessage = "Please Enter BaoHanh")]
        public string BAOHANH { get; set; }
        [Required(ErrorMessage = "Please Enter Loai")]
        public string LOAI { get; set; }
        [Required(ErrorMessage = "Please Enter KichThuoc")]
        public Nullable<int> KICHTHUOC { get; set; }

        public Nullable<int> SALE { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETGIOHANG> CHITIETGIOHANGs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETHOADON> CHITIETHOADONs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
    }
}
