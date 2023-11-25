using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ShopWatch.Models.MetaData
{
    public class MATHANG_METADATA { 

public int MAMATHANG { get; set; } 

     [Required(ErrorMessage = "Please enter the Name")]
     public string TENHANG { get; set; }
        [Required(ErrorMessage = "Please enter the đơn giá")]
      public Nullable<double> DONGIA { get; set; }
      public Nullable<System.DateTime> NGAYSANXUAT { get; set; }
      public string ANHSANPHAM { get; set; }
        public string TENHANGSANXUAT { get; set; }
      
    }
}