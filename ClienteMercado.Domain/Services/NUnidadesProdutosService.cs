using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NUnidadesProdutosService
    {
        DUnidadesProdutosRepository dUnidadesprodutos = new DUnidadesProdutosRepository();

        //Buscar Unidades de peso e medida dos produtos da Cotação
        public List<unidades_produtos> ListaUnidadesProdutos()
        {
            return dUnidadesprodutos.ListaUnidadesProdutos();
        }

        //Consultar dados da UNIDADE do PRODUTO da COTAÇÃO
        public unidades_produtos ConsultarDadosDaUnidadeDoProduto(int idUnidadeProduto)
        {
            return dUnidadesprodutos.ConsultarDadosDaUnidadeDoProduto(idUnidadeProduto);
        }

        //BUSCAR UNIDADE
        public string ConsultarDescricaoDaUnidadeDoProduto(int idUnidade)
        {
            return dUnidadesprodutos.ConsultarDescricaoDaUnidadeDoProduto(idUnidade);
        }
    }
}
