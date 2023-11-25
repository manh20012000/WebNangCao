using System.Web.Mvc;

namespace ShopWatch.Areas.NhanVien
{
    public class NhanVienAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NhanVien";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NhanVien_default",
                "NhanVien/{controller}/{action}/{id}",
                new { action = "LoginUser", id = UrlParameter.Optional },
                namespaces: new[] { "ShopWatch.Areas.NhanVien.Controllers" }
            );
        }
    }
}