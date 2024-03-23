using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
namespace ShopWatch.Payment.ZaloPayment.Request
{
    public class CreateZaloPayRequest
    {
        public CreateZaloPayRequest(int appId, string appUser, long appTime, long amount, string appTransId, string bankCode, string description)
        {
            AppId = appId;
            AppUser = appUser;
            AppTime = appTime;
            Amount = amount;
            AppTransId = appTransId;
            BankCode = bankCode;
            Description = description;

        }

        public int AppId { get; set; }
        public string AppUser { get; set; } = string.Empty;
        public long AppTime { get; set; }
        public long Amount { get; set; }
        public string AppTransId { get; set; } = string.Empty;
        public string ReturnUrl { get; }
        public string EmbedData { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
      
        public void MakeSignature(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(key);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            };
            this.Mac = byte2String;

        }
        public Dictionary<string, string> GetConten()
        {

            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("appid",AppId.ToString());
            keyValuePairs.Add("appuser", AppUser.ToString());
            keyValuePairs.Add("apptime", AppTime.ToString());
            keyValuePairs.Add("amount", Amount.ToString());
            keyValuePairs.Add("apptransid", AppTransId);
            keyValuePairs.Add("description",Description);
            keyValuePairs.Add("bankcode","zalopayapp" );
            keyValuePairs.Add("mac", Mac);
            return keyValuePairs;
        }
    /*    public (bool, string) GetLink(string paymentUrl);*/
    }
}