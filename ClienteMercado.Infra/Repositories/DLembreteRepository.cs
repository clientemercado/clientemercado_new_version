using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DLembreteRepository
    {
        public empresa_usuario_logins ConsultarEmailEmpresaUsuario(empresa_usuario_logins obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            empresa_usuario_logins email =
                _contexto.empresa_usuario_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

            return email;

            //Comentei o bloco de código abaixo devido ao não funcionamento no envio de e-mail de recuperação com esse
            //bloco do using ativo
            //using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            //{
            //    empresa_usuario_logins email =
            //        _contexto.empresa_usuario_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

            //    return email;
            //}
        }

        public profissional_usuario_logins ConsultarEmailProfissionalUsuario(profissional_usuario_logins obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            profissional_usuario_logins email =
                _contexto.profissional_usuario_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

            return email;
        }

        public usuario_cotante_logins ConsultarEmailUsuarioCotante(usuario_cotante_logins obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            usuario_cotante_logins email =
                _contexto.usuario_cotante_logins.FirstOrDefault(m => m.EMAIL1_USUARIO.Equals(obj.EMAIL1_USUARIO));

            return email;
        }
    }
}
