using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopWatch.Payment.ZaloPayment.Confige
{
    public class ZalopayConfige
    {
        public static string ConfigName => "ZaloPay";
        public string AppUser { get; set; } = string.Empty;
        public  string PayMentUrl { get; set; }=string.Empty;
        public  string RedirectUrl { get; set; }= string.Empty;
        public string IpnUrl { get; set; }= string.Empty;
        public string AppId { get; set; }= string.Empty;

        public  string Key1 { get; set; }= string.Empty;
        public static string Key2 { get; set; }= string.Empty;




    }
}