using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoIndividualEmpresaCentralComprasRepository : RepositoryBase<itens_cotacao_individual_empresa_central_compras>
    {
        //GRAVAR ITENS da COTAÇÃO INDIVIDUAL
        public itens_cotacao_individual_empresa_central_compras GravarItemNaCotacaoIndividualDaEmpresa(itens_cotacao_individual_empresa_central_compras obj)
        {
            itens_cotacao_individual_empresa_central_compras itemJaAdicionadoNestaCotacao =
                _contexto.itens_cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == obj.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS)
                && (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS == obj.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS)
                && (m.ID_CODIGO_UNIDADE_PRODUTO == obj.ID_CODIGO_UNIDADE_PRODUTO)
                && (m.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS == obj.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS)
                && (m.ID_EMPRESAS_PRODUTOS_EMBALAGENS == obj.ID_EMPRESAS_PRODUTOS_EMBALAGENS)));

            if (itemJaAdicionadoNestaCotacao == null)
            {
                itens_cotacao_individual_empresa_central_compras itemAdicionado =
                    _contexto.itens_cotacao_individual_empresa_central_compras.Add(obj);
                _contexto.SaveChanges();

                return itemAdicionado;
            }

            return null;
        }

        //CARREGAR LISTA de ITENS que compoem a COTAÇÂO INDIVUAL
        public List<ListaDeItensDaCotacaoIndividualViewModel> CarregarListaDeItensDaCotacaoIndividual(int iCI)
        {
            //var query = "SELECT * FROM itens_cotacao_individual_empresa_central_compras WHERE ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + iCI;

            var query = "SELECT IC.*, CI.ID_COTACAO_MASTER_CENTRAL_COMPRAS " +
                        "FROM itens_cotacao_individual_empresa_central_compras IC " +
                        "INNER JOIN cotacao_individual_empresa_central_compras CI ON(CI.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = IC.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS) " +
                        "WHERE IC.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + iCI;

            var result = _contexto.Database.SqlQuery<ListaDeItensDaCotacaoIndividualViewModel>(query).ToList();
            return result;
        }

        //EXCLUIR ITEM da COTAÇÃO
        public void ExcluirItemDaCotacao(int codItemCotacao, int iCM)
        {
            itens_cotacao_individual_empresa_central_compras itemASerExcluido =
                _contexto.itens_cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == codItemCotacao) && (m.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == iCM)));

            if (itemASerExcluido != null)
            {
                _contexto.itens_cotacao_individual_empresa_central_compras.Remove(itemASerExcluido);
                _contexto.SaveChanges();
            }
        }

        //EXCLUIR TODOS os ITEMS da COTAÇÃO
        public void ExcluirTodosOsItemsDaCotacao(int iCI)
        {
            List<itens_cotacao_individual_empresa_central_compras> itemsASeremExcluidos =
                _contexto.itens_cotacao_individual_empresa_central_compras.Where(m => (m.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == iCI)).ToList();

            if (itemsASeremExcluidos != null)
            {
                for (int i = 0; i < itemsASeremExcluidos.Count; i++)
                {
                    _contexto.itens_cotacao_individual_empresa_central_compras.Remove(itemsASeremExcluidos[i]);
                    _contexto.SaveChanges();
                }
            }
        }

        //GRAVAR DADOS EDITADOS do ITEM
        public void GravarItemEditadoNaCotacaoIndividualEmpresa(itens_cotacao_individual_empresa_central_compras obj)
        {
            itens_cotacao_individual_empresa_central_compras itemASerEditado =
                            _contexto.itens_cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == obj.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS)
                            && (m.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == obj.ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS)));

            if (itemASerEditado != null)
            {
                itemASerEditado.ID_CODIGO_UNIDADE_PRODUTO = obj.ID_CODIGO_UNIDADE_PRODUTO;
                itemASerEditado.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = obj.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS;
                itemASerEditado.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS = obj.QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS;
                itemASerEditado.ID_EMPRESAS_PRODUTOS_EMBALAGENS = obj.ID_EMPRESAS_PRODUTOS_EMBALAGENS;

                _contexto.SaveChanges();
            }
        }

        //BUSCAR ITENS da COTAÇÃO INDIVIDUAL - AGRUPADOS
        public List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel> CarregarListaDeItensAgrupadosDasCotacoesIndividuais(string listaIdsCotacoesIndividuais)
        {
            var query = "SELECT * FROM itens_cotacao_individual_empresa_central_compras " +
                        "WHERE ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS IN(" + listaIdsCotacoesIndividuais + ")";

            var result = _contexto.Database.SqlQuery<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel>(query).ToList();
            return result;
        }

        //SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO e quem é o FORNECEDOR
        public void SetarItemComoPedido(int idItemPedido, int idFornecedor, int idPedidoGeradoCC)
        {
            itens_cotacao_individual_empresa_central_compras itemASerEditado = 
                _contexto.itens_cotacao_individual_empresa_central_compras
                .FirstOrDefault(m => ((m.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == idItemPedido)));

            if (itemASerEditado != null)
            {
                itemASerEditado.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = idPedidoGeradoCC;
                itemASerEditado.ID_EMPRESA_FORNECEDORA_PEDIDO = idFornecedor;

                _contexto.SaveChanges();
            }
        }

        //DESFAZER SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO
        public itens_cotacao_individual_empresa_central_compras DesfazimentoDeItemComoPedido(int idItemPedido, int idPedido)
        {
            itens_cotacao_individual_empresa_central_compras itemASerEditado = 
                _contexto.itens_cotacao_individual_empresa_central_compras
                .FirstOrDefault(m => ((m.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == idItemPedido) 
                && (m.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS == idPedido)));

            if (itemASerEditado != null)
            {
                itemASerEditado.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = 0;
                itemASerEditado.ID_EMPRESA_FORNECEDORA_PEDIDO = null;

                _contexto.SaveChanges();
            }

            return itemASerEditado;
        }

        //DESFAZER SETAR TODOS os PRODUTOS da COTAÇÃO INDIVIDUAL como PEDIDO
        public void DesfazimentoDeTodosOsItensComoPedido(int idPedido)
        {
            List<itens_cotacao_individual_empresa_central_compras> listaItensASeremEditados = 
                _contexto.itens_cotacao_individual_empresa_central_compras.Where(m => (m.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS == idPedido)).ToList();

            for (int i = 0; i < listaItensASeremEditados.Count; i++)
            {
                listaItensASeremEditados[i].ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = 0;
                listaItensASeremEditados[i].ID_EMPRESA_FORNECEDORA_PEDIDO = null;
                _contexto.SaveChanges();
            }
        }
    }
}
