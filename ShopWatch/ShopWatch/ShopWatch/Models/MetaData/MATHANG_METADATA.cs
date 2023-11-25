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
        public string ANHSANPHAM { get; set; }
        [Required(ErrorMessage = "Please Enter date")]
        public Nullable<System.DateTime> NGAYSANXUAT { get; set; }
        [Required(ErrorMessage = "Please Enter Tenhangsx")]

        public string TENHANGSANXUAT { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá trị.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Vui lòng chỉ nhập số.")]
        public Nullable<double> GIAHANG { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập LOAI.")]
        public string LOAI { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập BẢO HÀNH")]
        public string BAOHANH { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Kích thước.")]
        public string KICHTHUOC { get; set; }
        public string TRANGTHAI { get; set; }

    }
}