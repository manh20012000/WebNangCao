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
    
    public partial class CHITIETGIOHANG
    {
        public int MACHITIETGIOHANG { get; set; }
        public Nullable<int> MAMATHANG { get; set; }
        public Nullable<int> MAGIOHANG { get; set; }
        public Nullable<int> SOLUONGMUA { get; set; }
        public Nullable<decimal> DONGIA { get; set; }
    
        public virtual GIOHANG GIOHANG { get; set; }
        public virtual MATHANG MATHANG { get; set; }
    }
}