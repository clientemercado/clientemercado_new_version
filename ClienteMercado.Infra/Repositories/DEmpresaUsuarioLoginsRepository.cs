using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresaUsuarioLoginsRepository : RepositoryBase<empresa_usuario_logins>
    {
        //CONSULTAR DADOS de CONTATO do USUÁRIO
        public empresa_usuario_logins ConsultarDadosDeContatoDoUsuario(int idUsuario)
        {
            empresa_usuario_logins dadosDeContato = _contexto.empresa_usuario_logins.FirstOrDefault(m => (m.ID_CODIGO_USUARIO == idUsuario));

            return dadosDeContato;
        }
    }
}
