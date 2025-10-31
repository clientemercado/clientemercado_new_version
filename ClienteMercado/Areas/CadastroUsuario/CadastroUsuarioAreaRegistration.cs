using System.Web.Mvc;

namespace ClienteMercado.Areas.CadastroUsuario
{
    public class CadastroUsuarioAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CadastroUsuario";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CadastroUsuario_default",
                "CadastroUsuario/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
