using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NLembreteService
    {
        DLembreteRepository dlembrete = new DLembreteRepository();

        public empresa_usuario_logins ConsultarEmailEmpresaUsuario(empresa_usuario_logins obj)
        {
            return dlembrete.ConsultarEmailEmpresaUsuario(obj);
        }

        public profissional_usuario_logins ConsultarEmailProfissionalUsuario(profissional_usuario_logins obj)
        {
            return dlembrete.ConsultarEmailProfissionalUsuario(obj);
        }

        public usuario_cotante_logins ConsultarEmailUsuarioCotante(usuario_cotante_logins obj)
        {
            return dlembrete.ConsultarEmailUsuarioCotante(obj);
        }
    }
}
