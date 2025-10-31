using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DUsuarioCotanteRepository
    {
        //Gravar dados do USUÁRIO COTANTE
        public usuario_cotante GravarUsuarioCotante(usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_cotante usuarioCotante = _contexto.usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return usuarioCotante;
            }
        }

        //Consultar e-mail do USUÁRIO COTANTE
        public usuario_cotante_logins ConsultarEmailUsuarioCotante(usuario_cotante_logins obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_cotante_logins email =
                    _contexto.usuario_cotante_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

                return email;
            }
        }

        //Confirmar o Cadastro do Usuario Cotante
        public usuario_cotante ConfirmarCadastroUsuarioCotante(usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_cotante confirmaCadastroUsuarioCotante =
                             _contexto.usuario_cotante.Find(obj.ID_CODIGO_USUARIO_COTANTE);
                confirmaCadastroUsuarioCotante.CADASTRO_CONFIRMADO = true;
                _contexto.SaveChanges();

                return confirmaCadastroUsuarioCotante;
            }
        }

        //Consultar dados diversos do USUÁRIO COTANTE
        public usuario_cotante ConsultarDadosDoUsuarioCotante(usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_cotante dadosDoUsuarioCotante =
                    _contexto.usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_USUARIO_COTANTE.Equals(obj.ID_CODIGO_USUARIO_COTANTE)));

                return dadosDoUsuarioCotante;
            }
        }
    }
}
