using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DProfissionalUsuarioRepository
    {
        //Gravar dados do Usuário Profissional de Serviços
        public profissional_usuario GravarProfissionalUsuario(profissional_usuario obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                profissional_usuario profissionalUsuario = _contexto.profissional_usuario.Add(obj);
                _contexto.SaveChanges();

                return profissionalUsuario;
            }
        }

        //Consultar se já existe o e-mail registrado do Usuário Profissional de Serviços
        public profissional_usuario_logins ConsultarEmailUsuarioProfissional(profissional_usuario_logins obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                profissional_usuario_logins email =
                    _contexto.profissional_usuario_logins.FirstOrDefault(
                        m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

                return email;
            }
        }

        //Confirmar o Cadastro do Profissional de Serviços
        public usuario_profissional ConfirmarCadastroUsuarioProfissional(usuario_profissional obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_profissional confirmaCadastroUsuarioProfissional =
                            _contexto.usuario_profissional.Find(obj.ID_CODIGO_PROFISSIONAL_USUARIO);
                confirmaCadastroUsuarioProfissional.CADASTRO_CONFIRMADO = true;
                _contexto.SaveChanges();

                return confirmaCadastroUsuarioProfissional;
            }
        }
    }
}
