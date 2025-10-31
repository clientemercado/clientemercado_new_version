using System.Web.Mvc;

namespace ClienteMercado.Areas.PerfilUsuario
{
    public class PerfilUsuarioAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PerfilUsuario";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PerfilUsuario_default",
                "PerfilUsuario/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
