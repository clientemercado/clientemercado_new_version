using System.Web.Mvc;

namespace ClienteMercado.Areas.Contato
{
    public class ContatoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contato";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Contato_default",
                "Contato/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
