using System;
using System.Web;

namespace ClienteMercado.Utils.Net
{
    public class Sessao
    {
        /*
        OBS: Não criar mais Sessions. Na dúvida sobre minha orientação, faça uma pesquisa sobre 'escalabilidade' e o 'prejuízo no uso de Sessions'
        */

        //Session que carrega o ID do usuário logado
        public static int? IdUsuarioLogado
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["IdUsuarioLogado"]); }
            set { HttpContext.Current.Session.Add("idUsuarioLogado", value); } //CRIAR UM MÉTODO SÓ PARA OS ADD´s E COMENTAR ESSES SET´s --> Ver com o Juranda como era isso mesmo:)
        }

        //Session que carrega o ID da empresa logada
        public static int? IdEmpresaUsuario
        {
            get { return Convert.ToInt32(HttpContext.Current.Session["IdEmpresaUsuario"]); }
            set { HttpContext.Current.Session.Add("IdEmpresaUsuario", value); }
        }

        //Session usada no armazenamento do objeto 'novaEmpresa', que carrega consigo os dados da empresa a ser cadastrada em conjunto com o usuário master (Ver se posso substituir)
        public static object tipoDeUsuario
        {
            get { return HttpContext.Current.Session["tipoDeUsuario"]; }
            set { HttpContext.Current.Session.Add("tipoDeUsuario", value); }
        }

        //Session usada no armazenamento do objeto 'novaEmpresa', que carrega consigo os dados da empresa a ser cadastrada em conjunto com o usuário master (Ver se posso substituir)
        public static bool empAdmSoft
        {
            get { return (bool)HttpContext.Current.Session["empAdmSoft"]; }
            set { HttpContext.Current.Session.Add("empAdmSoft", value); }
        }

    }
}
