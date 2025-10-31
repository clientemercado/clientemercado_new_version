using System.Web.Mvc;
using System.Web.Routing;

namespace ClienteMercado
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Rota Alteração de Senha
            routes.MapRoute("AlterarSenha", "Login/ForgotPas/{tipoUsuario}/{login}/{senha}",
            new { controller = "Login", action = "ForgotPas" });

            //Rota de Confirmação de Cadastro Usuário / Empresa
            routes.MapRoute("ConfirmarUsuarioEmpresa", "UsuarioEmpresa/ConfirmarCadastroUsuario/{login}/{senha}",
            new { controller = "UsuarioEmpresa", action = "ConfirmarCadastroUsuario" });

            //Rota de Confirmação de Cadastro Usuário / Profissional de Serviços
            routes.MapRoute("ConfirmarUsuarioProfissional", "UsuarioProfissional/ConfirmarCadastroUsuario/{login}/{senha}",
            new { controller = "UsuarioProfissional", action = "ConfirmarCadastroUsuario" });

            //Rota de Confirmação de Cadastro Usuário / Cotante
            routes.MapRoute("ConfirmarUsuarioCotante", "UsuarioCotante/ConfirmarCadastroUsuario/{login}/{senha}",
            new { controller = "UsuarioCotante", action = "ConfirmarCadastroUsuario" });

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
                // caso use Areas definir o namespace
                , namespaces: new[]
                {
                    "ClienteMercado.UI.MVC.Controllers",
                    "Acesso.Controllers",
                    "CadastroEmpresa.Controllers",
                    "CadastroUsuario.Controllers",
                    "Company.Controllers",
                    "Contato.Controllers",
                    "Financeiro.Controllers",
                    "PerfilUsuario.Controllers",
                    "Utilitarios.Controllers"
                }
            );

        }
    }
}