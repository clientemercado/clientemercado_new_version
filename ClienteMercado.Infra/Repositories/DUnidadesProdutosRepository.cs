using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DUnidadesProdutosRepository : RepositoryBase<unidades_produtos>
    {
        //Buscar Unidades de peso e medida dos produtos da Cotação
        public List<unidades_produtos> ListaUnidadesProdutos()
        {
            return _contexto.unidades_produtos.OrderBy(m => m.DESCRICAO_UNIDADE_PRODUTO).ToList();
        }

        //Consultar dados da UNIDADE do PRODUTO da COTAÇÃO
        public unidades_produtos ConsultarDadosDaUnidadeDoProduto(int idUnidadeProduto)
        {
            unidades_produtos dadosDaUnidadeProduto =
                _contexto.unidades_produtos.FirstOrDefault(m => (m.ID_CODIGO_UNIDADE_PRODUTO.Equals(idUnidadeProduto)));

            return dadosDaUnidadeProduto;
        }

        //BUSCAR UNIDADE
        public string ConsultarDescricaoDaUnidadeDoProduto(int idUnidade)
        {
            unidades_produtos dadosDaUnidade =
                _contexto.unidades_produtos.FirstOrDefault(m => (m.ID_CODIGO_UNIDADE_PRODUTO == idUnidade));

            return dadosDaUnidade.DESCRICAO_UNIDADE_PRODUTO;
        }
    }
}
