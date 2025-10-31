using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DTiposContratosServicosRepository
    {
        //Traz a lista de Contratos de Serviços
        public List<tipos_contratos_servicos> ListaDeTiposContratosDeServicos()
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                return _contexto.tipos_contratos_servicos.ToList();
            }
        }

        //Busca detalhes do Tipo de Contrato de Serviços
        public tipos_contratos_servicos BuscarDadosDoTipoDeContratoDeServicos(tipos_contratos_servicos obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                tipos_contratos_servicos dadosDoTipoDeContrato =
                    _contexto.tipos_contratos_servicos.FirstOrDefault(
                        m => m.ID_CODIGO_TIPO_CONTRATO_COTADA.Equals(obj.ID_CODIGO_TIPO_CONTRATO_COTADA));

                return dadosDoTipoDeContrato;
            }
        }
    }
}
