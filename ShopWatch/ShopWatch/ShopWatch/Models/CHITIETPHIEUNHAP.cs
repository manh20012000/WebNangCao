//------------------------------------------------------------------------------
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
    
    public partial class CHITIETPHIEUNHAP
    {
        public Nullable<int> MAMATHANG { get; set; }
        public int MACTPHIEUNHAP { get; set; }
        public Nullable<int> SOLUONG { get; set; }
        public Nullable<int> MANHAPHANG { get; set; }
        public Nullable<double> GIANHAP { get; set; }
        public Nullable<bool> TRANGTHAI { get; set; }
    
        public virtual MATHANG MATHANG { get; set; }
        public virtual NHAPHANG NHAPHANG { get; set; }
    }
}
