using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NCpfService
    {
        DCpfRepository dcpf = new DCpfRepository();

        //Consultar Cpf de Usuário que está se cadastrando pela primeira vez no sistema, de Empresa que tbm está se cadastrando agora
        public usuario_empresa ConsultarCpf(usuario_empresa obj)
        {
            return dcpf.ConsultarCpf(obj);
        }

        //Consultar o Cpf de Usuário que pretende se cadastrar em determinada Empresa que já existe no sistema
        public usuario_empresa ConsultarCpfEmDeterminadaEmpresa(usuario_empresa obj)
        {
            return dcpf.ConsultarCpfEmDeterminadaEmpresa(obj);
        }

        //Consultar Cpf de Usuário Profissional de Serviços que está se cadastrando pela primeira vez no sistema
        public profissional_usuario ConsultarCpfProfissional(profissional_usuario obj)
        {
            return dcpf.ConsultarCpfProfissional(obj);
        }

        //Consultar Cpf de Usuário Cotante que está se cadastrando pela primeira vez no sistema
        public usuario_cotante ConsultarCpfUsuarioCotante(usuario_cotante obj)
        {
            return dcpf.ConsultarCpfUsuarioCotante(obj);
        }
    }
}
