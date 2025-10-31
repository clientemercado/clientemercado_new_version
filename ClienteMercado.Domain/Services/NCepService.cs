using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NCepService
    {
        //Consultar CEP e afins, baseado no cep digitado pelo usuário
        public enderecos_empresa_usuario ConsultarCep(enderecos_empresa_usuario obj)
        {
            DCepRepository dcep = new DCepRepository();

            return dcep.ConsultarCep(obj);
        }

        //Consultar CEP e afins, baseado no id do cep informado pelo sistema
        public enderecos_empresa_usuario ConsultarCepPorId(enderecos_empresa_usuario obj)
        {
            DCepRepository dcep = new DCepRepository();

            return dcep.ConsultarCepPorId(obj);
        }
    }
}
