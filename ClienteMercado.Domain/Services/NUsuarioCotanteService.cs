using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NUsuarioCotanteService
    {
        DUsuarioCotanteRepository dusuariocotante = new DUsuarioCotanteRepository();

        //Gravar dados do USUÁRIO COTANTE
        public usuario_cotante GravarUsuarioCotante(usuario_cotante obj)
        {
            return dusuariocotante.GravarUsuarioCotante(obj);
        }

        //Consultar e-mail do USUÁRIO COTANTE
        public usuario_cotante_logins ConsultarEmailUsuarioCotante(usuario_cotante_logins obj)
        {
            return dusuariocotante.ConsultarEmailUsuarioCotante(obj);
        }

        //Confirmar o Cadastro do USUÁRIO COTANTE
        public usuario_cotante ConfirmarCadastroUsuarioCotante(usuario_cotante obj)
        {
            return dusuariocotante.ConfirmarCadastroUsuarioCotante(obj);
        }

        //Consultar dados diversos do USUÁRIO COTANTE
        public usuario_cotante ConsultarDadosDoUsuarioCotante(usuario_cotante obj)
        {
            return dusuariocotante.ConsultarDadosDoUsuarioCotante(obj);
        }

    }
}
