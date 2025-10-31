using System.Web.Mvc;

namespace ClienteMercado.Areas.Acesso
{
    public class AcessoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Acesso";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Acesso_default",
                "Acesso/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
