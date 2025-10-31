using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NProfissionalUsuarioService
    {
        DProfissionalUsuarioRepository dprofissionalusuario = new DProfissionalUsuarioRepository();

        //Gravar dados do Usuário Profissional de Serviços
        public profissional_usuario GravarProfissionalUsuario(profissional_usuario obj)
        {
            return dprofissionalusuario.GravarProfissionalUsuario(obj);
        }

        //Consultar se já existe o e-mail registrado do Usuário Profissional de Serviços
        public profissional_usuario_logins ConsultarEmailUsuarioProfissional(profissional_usuario_logins obj)
        {
            return dprofissionalusuario.ConsultarEmailUsuarioProfissional(obj);
        }

        //Confirmar o Cadastro do Usuário Profissional de Serviços
        public usuario_profissional ConfirmarCadastroUsuarioProfissional(usuario_profissional obj)
        {
            return dprofissionalusuario.ConfirmarCadastroUsuarioProfissional(obj);
        }
    }
}
