using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensPedidoCentralComprasService
    {
        DItensPedidoCentralComprasRepository repositoryItensPedidoCC = new DItensPedidoCentralComprasRepository();

        //GRAVAR ITEM do PEDIDO
        public int GravarItemDoPedido(itens_pedido_central_compras obj)
        {
            return repositoryItensPedidoCC.GravarItemDoPedido(obj);
        }

        //VERIFICAR SE O FORNECEDOR RECEBEU PEDIDO DESTE PRODUTO
        public bool ConsultarSeOFornecedorRecebeuPedidoParaEsteProduto(int idItemCotadoAoFornecedor)
        {
            return repositoryItensPedidoCC.ConsultarSeOFornecedorRecebeuPedidoParaEsteProduto(idItemCotadoAoFornecedor);
        }

        //EXCLUIR o ITEM do PEDIDO
        public bool ExcluirItemDoPedido(int idItemASerExcluido, int idPedido)
        {
            return repositoryItensPedidoCC.ExcluirItemDoPedido(idItemASerExcluido, idPedido);
        }

        //CONSULTAR se o PRODUTO JÁ foi PEDIDO
        public itens_pedido_central_compras ConsultarSeOProdutoJahFoiPedido(int idItemIndividual)
        {
            return repositoryItensPedidoCC.ConsultarSeOProdutoJahFoiPedido(idItemIndividual);
        }

        //BUSCAR LISTA de ITENS PEDIDOS
        public List<itens_pedido_central_compras> ConsultarListaDeItensJahPedidos(string idsPedidos)
        {
            return repositoryItensPedidoCC.ConsultarListaDeItensJahPedidos(idsPedidos);
        }

        //EXCLUIR TODOS os ITENS do PEDIDO
        public bool ExcluirTodosOsItensDoPedido(int idPedido)
        {
            return repositoryItensPedidoCC.ExcluirTodosOsItensDoPedido(idPedido);
        }

        //BAIXAR ITEM(s) DO PEDIDO
        public List<itens_pedido_central_compras> InformarRecebimentoDoItemDoPedido(int idPedidoABaixar, string dataEntrega)
        {
            return repositoryItensPedidoCC.InformarRecebimentoDoItemDoPedido(idPedidoABaixar, dataEntrega);
        }
    }
}
