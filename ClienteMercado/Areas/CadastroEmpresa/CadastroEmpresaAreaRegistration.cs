using System.Web.Mvc;

namespace ClienteMercado.Areas.CadastroEmpresa
{
    public class CadastroEmpresaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CadastroEmpresa";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CadastroEmpresa_default",
                "CadastroEmpresa/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
