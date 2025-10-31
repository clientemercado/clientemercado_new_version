using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoFilhaNegociacaoCentralDeComprasRepository : RepositoryBase<itens_cotacao_filha_negociacao_central_compras>
    {
        //CARREGAR ITENS da COTAÇÃO ENVIADA
        public List<itens_cotacao_filha_negociacao_central_compras> CarregarOsItensDeUmaCotacaoEnviada(int idCotacaoFilha)
        {
            List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacao =
                _contexto.itens_cotacao_filha_negociacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha)).OrderBy(m => m.DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS).ToList();

            return listaDeItensDaCotacao;
        }

        //GRAVAR ITENS da COTAÇÃO FILHA da CENTRAL de COMPRAS
        public itens_cotacao_filha_negociacao_central_compras GravarItensDaCotacaoFilhaDaCentralCompras(itens_cotacao_filha_negociacao_central_compras obj)
        {
            itens_cotacao_filha_negociacao_central_compras gravarItensCotacaoFIlhaDaCentralDeCompras =
                _contexto.itens_cotacao_filha_negociacao_central_compras.Add(obj);
            _contexto.SaveChanges();

            return gravarItensCotacaoFIlhaDaCentralDeCompras;
        }

        //BUSCAR MENOR VALOR entre as respostas desta COTAÇÃO
        public decimal BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(int idItemDaCotacaoIndividual)
        {
            //var query = " SELECT Min(PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS) AS menorPrecoDoitemNaCotacao " +
            //            " FROM itens_cotacao_filha_negociacao_central_compras " +
            //            " WHERE ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + idItemDaCotacaoIndividual;

            var query = " SELECT COALESCE(Min(PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS),0) AS menorPrecoDoitemNaCotacao " +
                        " FROM itens_cotacao_filha_negociacao_central_compras " +
                        " WHERE ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + idItemDaCotacaoIndividual +
                        " AND PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS > 0";

            var result = _contexto.Database.SqlQuery<PrecosDaCotacaoAnalisadaViewModel>(query).FirstOrDefault();

            return result.menorPrecoDoitemNaCotacao;
        }

        //LIMPAR CAMPOS com o MENOR VALOR APURADO UTILIZADO como VALOR de CONTRA-PROPOSTA
        public void LimparCamposComValorDeContraPropostaAoFornecedor(int idCotacaoFilha)
        {
            List<itens_cotacao_filha_negociacao_central_compras> listaDeItensDaCotacao =
                _contexto.itens_cotacao_filha_negociacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha)).ToList();

            for (int i = 0; i < listaDeItensDaCotacao.Count; i++)
            {
                listaDeItensDaCotacao[i].PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS = 0;
                _contexto.SaveChanges();
            }
        }

        //CARREGAR DADOS DE APENAS 1 PRODUTO COTADO PARA TODAS AS EMPRESAS NESSA COTAÇÃO
        public List<ListaProdutoUnicoCotadoPorEmpresaViewModel> CarregarDadosDeUmProdutoCotadoEmTodasAsEmpresasFornecedoras(int[] listaIdsCotacaoFilhas,
            int idItemDaCotacaoIndividual)
        {
            var listaIdsCF = String.Join(", ", listaIdsCotacaoFilhas);

            var query = " SELECT * FROM " +
                        " itens_cotacao_filha_negociacao_central_compras " +
                        " WHERE ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS IN (" + listaIdsCF + ")" +
                        " AND ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + idItemDaCotacaoIndividual;
            var result = _contexto.Database.SqlQuery<ListaProdutoUnicoCotadoPorEmpresaViewModel>(query).ToList();

            return result;
        }

        //GRAVAR o MENOR VALOR APURADO na consulta como VALOR de CONTRA-PROPOSTA
        public void GravarValorDaContraProposta(int idCotacaoFilha, int idItemDaCotacaoIndividual, decimal menorPreco)
        {
            itens_cotacao_filha_negociacao_central_compras itemQueTeraOValorDeContraPropostaAtualizado =
                _contexto.itens_cotacao_filha_negociacao_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha)
                && (m.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS == idItemDaCotacaoIndividual)));

            if (itemQueTeraOValorDeContraPropostaAtualizado != null)
            {
                itemQueTeraOValorDeContraPropostaAtualizado.PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS = menorPreco;
                _contexto.SaveChanges();
            }
        }

        //CARREGAR ITENS da COTAÇÃO RECEBIDA
        public List<ListaDeItensCotadosViewModel> CarregarOsItensDeUmaCotacaoRecebida(int idCotacaoFilha)
        {
            var query = "SELECT ICC.*, EFM.DESCRICAO_EMPRESA_FABRICANTE_MARCAS as marcaProdutoCotado, EPE.DESCRICAO_PRODUTO_EMBALAGEM as produtoEmbalagem, " +
                        "UP.DESCRICAO_UNIDADE_PRODUTO as unidadeProdutoCotado, ICI.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS as codigoProduto " +
                        "FROM itens_cotacao_filha_negociacao_central_compras ICC " +
                        "INNER JOIN itens_cotacao_individual_empresa_central_compras ICI " +
                        "ON(ICI.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = ICC.ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS) " +
                        "INNER JOIN empresas_fabricantes_marcas EFM ON(EFM.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS = ICI.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS) " +
                        "INNER JOIN empresas_produtos_embalagens EPE ON(EPE.ID_EMPRESAS_PRODUTOS_EMBALAGENS = ICI.ID_EMPRESAS_PRODUTOS_EMBALAGENS) " +
                        "INNER JOIN unidades_produtos UP ON(UP.ID_CODIGO_UNIDADE_PRODUTO = ICI.ID_CODIGO_UNIDADE_PRODUTO) " +
                        "WHERE ICC.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS = " + idCotacaoFilha + " " +
                        "ORDER BY ICC.DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS";

            var result = _contexto.Database.SqlQuery<ListaDeItensCotadosViewModel>(query).ToList();
            return result;
        }

        //SALVAR os VALORES RESPONDIDOS pelo FORNECEDOR
        public bool GravarValoresRespondidosParaACotacao(int idCotacaoFilha, int idItemNegociado, decimal valorTabelaItemNegociado, decimal valorDiferenciadoRespondido)
        {
            itens_cotacao_filha_negociacao_central_compras dadosDoItemRespondido =
                _contexto.itens_cotacao_filha_negociacao_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS == idItemNegociado)
                && (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha)));

            if (dadosDoItemRespondido != null)
            {
                dadosDoItemRespondido.PRECO_UNITARIO_ITENS_TABELA_COTACAO_CENTRAL_COMPRAS = valorTabelaItemNegociado;
                dadosDoItemRespondido.PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS = valorDiferenciadoRespondido;
                dadosDoItemRespondido.PRODUTO_COTADO_CENTRAL_COMPRAS = true;

                //if (valorDiferenciadoRespondido > 0)
                //{
                //    dadosDoItemRespondido.PRODUTO_COTADO_CENTRAL_COMPRAS = true;
                //}
                //else
                //{
                //    dadosDoItemRespondido.PRODUTO_COTADO_CENTRAL_COMPRAS = false;
                //}

                _contexto.SaveChanges();

                return true;
            }

            return false;
        }

        //CONSULTAR LISTA de ITENS da COTACAO FILHA
        public List<itens_cotacao_filha_negociacao_central_compras> ConsultarItensDaCotacaoDaCC(int iCCF)
        {
            List<itens_cotacao_filha_negociacao_central_compras> listaDeitensDaCotacao =
                _contexto.itens_cotacao_filha_negociacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF)).ToList();

            return listaDeitensDaCotacao;
        }

        //DESFAZER MARCAÇÃO de ITEM de COTAÇÃO RESPONDIDO
        public void DesfazerMarcacaoDeItemDeCotacaoRespondido(int iCCF)
        {
            List<itens_cotacao_filha_negociacao_central_compras> listaDeitensDaCotacao =
                _contexto.itens_cotacao_filha_negociacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF)).ToList();

            for (int i = 0; i < listaDeitensDaCotacao.Count; i++)
            {
                listaDeitensDaCotacao[i].PRODUTO_COTADO_CENTRAL_COMPRAS = false;
                _contexto.SaveChanges();
            }
        }
    }
}
