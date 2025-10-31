using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCpfRepository
    {
        //Consultar Cpf de Usuário que está se cadastrando pela primeira vez no sistema, de Empresa que tbm está se cadastrando agora
        public usuario_empresa ConsultarCpf(usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_empresa cpf =
                    _contexto.usuario_empresa.FirstOrDefault(m => m.CPF_USUARIO_EMPRESA.Equals(obj.CPF_USUARIO_EMPRESA));

                return cpf;
            }
        }

        //Consultar o Cpf de Usuário que pretende se cadastrar em determinada Empresa que já existe no sistema
        public usuario_empresa ConsultarCpfEmDeterminadaEmpresa(usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_empresa cpf =
                    _contexto.usuario_empresa.FirstOrDefault(m => m.CPF_USUARIO_EMPRESA.Equals(obj.CPF_USUARIO_EMPRESA) && m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA));

                return cpf;
            }
        }

        //Consultar Cpf de Usuário Profissional de Serviços que está se cadastrando pela primeira vez no sistema
        public profissional_usuario ConsultarCpfProfissional(profissional_usuario obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                profissional_usuario cpf =
                    _contexto.profissional_usuario.FirstOrDefault(
                        m => m.CPF_PROFISSIONAL_USUARIO.Equals(obj.CPF_PROFISSIONAL_USUARIO));

                return cpf;
            }
        }

        //Consultar Cpf de Usuário Cotante que está se cadastrando pela primeira vez no sistema
        public usuario_cotante ConsultarCpfUsuarioCotante(usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                usuario_cotante cpf =
                    _contexto.usuario_cotante.FirstOrDefault(m => m.CPF_USUARIO_COTANTE.Equals(obj.CPF_USUARIO_COTANTE));

                return cpf;
            }
        }
    }
}
