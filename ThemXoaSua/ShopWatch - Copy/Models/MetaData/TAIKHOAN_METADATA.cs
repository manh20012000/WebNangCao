using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopWatch.Models.MetaData
{

    public  class TAIKHOAN_METADATA
    {   [Key]
        [Required(ErrorMessage = "Please enter the Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Please enter the password")]
        public string MATKHAU { get; set; }
        [Required(ErrorMessage = "Please enter the xac minh")]
        public string XACTHUC { get; set; }
    }
}