using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NTiposContratosServicosService
    {
        DTiposContratosServicosRepository dtipocontratosservicos = new DTiposContratosServicosRepository();

        //Traz a lista de Contratos de Serviços
        public List<tipos_contratos_servicos> ListaDeTiposContratosDeServicos()
        {
            return dtipocontratosservicos.ListaDeTiposContratosDeServicos();
        }

        //Busca detalhes do Tipo de Contrato de Serviços
        public tipos_contratos_servicos BuscarDadosDoTipoDeContratoDeServicos(tipos_contratos_servicos obj)
        {
            return dtipocontratosservicos.BuscarDadosDoTipoDeContratoDeServicos(obj);
        }
    }
}
