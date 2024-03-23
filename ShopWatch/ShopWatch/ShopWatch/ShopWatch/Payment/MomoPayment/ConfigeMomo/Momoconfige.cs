using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Payment.MomoPayment.ConfigeMomo
{
    public class Momoconfige
    {
        public static string ConfigeName => "Momo";
        public static string Partnercode { get; set; } = string.Empty;
        public static string RetturnUlr { get; set; } = string.Empty;
        public static string IpnUrl { get; set; } = string.Empty;
        public  static string AccessKey { get; set; } = string.Empty;
        public  static string SceretKey { get; set; } = string.Empty;
        public static string PaymentUrl { get; set; } = string.Empty;
    }
}