using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoFilhaNegociacaoCentralDeComprasService
    {
        DItensCotacaoFilhaNegociacaoCentralDeComprasRepository dRepository = new DItensCotacaoFilhaNegociacaoCentralDeComprasRepository();

        //CARREGAR ITENS da COTAÇÃO ENVIADA
        public List<itens_cotacao_filha_negociacao_central_compras> CarregarOsItensDeUmaCotacaoEnviada(int idCotacaoFilha)
        {
            return dRepository.CarregarOsItensDeUmaCotacaoEnviada(idCotacaoFilha);
        }

        //GRAVAR ITENS da COTAÇÃO FILHA da CENTRAL de COMPRAS
        public itens_cotacao_filha_negociacao_central_compras GravarItensDaCotacaoFilhaDaCentralCompras(itens_cotacao_filha_negociacao_central_compras obj)
        {
            return dRepository.GravarItensDaCotacaoFilhaDaCentralCompras(obj);
        }

        //BUSCAR MENOR VALOR entre as respostas desta COTAÇÃO
        public decimal BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(int idItemDaCotacaoIndividual)
        {
            return dRepository.BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(idItemDaCotacaoIndividual);
        }

        //GRAVAR o MENOR VALOR APURADO na consulta como VALOR de CONTRA-PROPOSTA
        public void GravarValorDaContraProposta(int idCotacaoFilha, int idItemDaCotacaoIndividual, decimal menorPreco)
        {
            dRepository.GravarValorDaContraProposta(idCotacaoFilha, idItemDaCotacaoIndividual, menorPreco);
        }

        //LIMPAR CAMPOS com o MENOR VALOR APURADO UTILIZADO como VALOR de CONTRA-PROPOSTA
        public void LimparCamposComValorDeContraPropostaAoFornecedor(int idCotacaoFilha)
        {
            dRepository.LimparCamposComValorDeContraPropostaAoFornecedor(idCotacaoFilha);
        }

        //CARREGAR DADOS DE APENAS 1 PRODUTO COTADO PARA TODAS AS EMPRESAS NESSA COTAÇÃO
        public List<ListaProdutoUnicoCotadoPorEmpresaViewModel> CarregarDadosDeUmProdutoCotadoEmTodasAsEmpresasFornecedoras(int[] listaIdsCotacaoFilhas,
            int idItemDaCotacaoIndividual)
        {
            int temMenorPreco = 0;
            decimal menorPreco = 0;
            decimal totalMenorPrecoVsQuantidade = 0;

            DItensCotacaoFilhaNegociacaoCentralDeComprasRepository dRepositoryItensCotacao = new DItensCotacaoFilhaNegociacaoCentralDeComprasRepository();

            List<ListaProdutoUnicoCotadoPorEmpresaViewModel> ListaUmProdutoCotadoEmTodasAsEmpresas =
                dRepository.CarregarDadosDeUmProdutoCotadoEmTodasAsEmpresasFornecedoras(listaIdsCotacaoFilhas, idItemDaCotacaoIndividual);

            for (int i = 0; i < ListaUmProdutoCotadoEmTodasAsEmpresas.Count; i++)
            {
                menorPreco =
                    dRepositoryItensCotacao.BuscarMenorPrecoDeUmProdutoEntreAsRespostaDeUmaCotacao(ListaUmProdutoCotadoEmTodasAsEmpresas[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS);

                for (int a = 0; a < ListaUmProdutoCotadoEmTodasAsEmpresas.Count; a++)
                {
                    if (ListaUmProdutoCotadoEmTodasAsEmpresas[a].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS == menorPreco)
                    {
                        temMenorPreco += 1;
                    }
                }

                if (temMenorPreco < ListaUmProdutoCotadoEmTodasAsEmpresas.Count)
                {
                    //QUANDO TEM O MENOR PREÇO na LISTA
                    if (ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS == menorPreco)
                    {
                        ListaUmProdutoCotadoEmTodasAsEmpresas[i].menorPreco = "sim";
                        ListaUmProdutoCotadoEmTodasAsEmpresas[i].precoMenor = ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");

                        totalMenorPrecoVsQuantidade =
                            (ListaUmProdutoCotadoEmTodasAsEmpresas[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS * ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS);

                        if (ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS > 0)
                        {
                            ListaUmProdutoCotadoEmTodasAsEmpresas[i].valorTotalUnitarioVsQuantidade = totalMenorPrecoVsQuantidade.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                        }
                        else
                        {
                            ListaUmProdutoCotadoEmTodasAsEmpresas[i].valorTotalUnitarioVsQuantidade = "Não Cotou";
                        }
                    }
                }
                else
                {
                    //QUANDO NÃO TEM O MENOR PREÇO na LISTA
                    ListaUmProdutoCotadoEmTodasAsEmpresas[i].menorPreco = "nao";

                    totalMenorPrecoVsQuantidade =
                        (ListaUmProdutoCotadoEmTodasAsEmpresas[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS * ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS);

                    ListaUmProdutoCotadoEmTodasAsEmpresas[i].valorTotalUnitarioVsQuantidade = totalMenorPrecoVsQuantidade.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                }

                if (ListaUmProdutoCotadoEmTodasAsEmpresas[i].PRODUTO_COTADO_CENTRAL_COMPRAS)
                {
                    ListaUmProdutoCotadoEmTodasAsEmpresas[i].produtoCotadoSimNao = "sim";
                }
                else
                {
                    ListaUmProdutoCotadoEmTodasAsEmpresas[i].produtoCotadoSimNao = "nao";
                }

                ListaUmProdutoCotadoEmTodasAsEmpresas[i].subTotalMenorPreco = totalMenorPrecoVsQuantidade;

                temMenorPreco = 0;
            }

            return ListaUmProdutoCotadoEmTodasAsEmpresas;
        }

        //CARREGAR ITENS da COTAÇÃO RECEBIDA
        public List<ListaDeItensCotadosViewModel> CarregarOsItensDeUmaCotacaoRecebida(int idCotacaoFilha)
        {
            try
            {
                bool itemPedidoCC = false;

                DCotacaoFilhaCentralDeComprasRepository dRepositoryCotacaoFilhaRecebida = new DCotacaoFilhaCentralDeComprasRepository();
                NItensPedidoCentralComprasService negociosItensPedidoCC = new NItensPedidoCentralComprasService();

                cotacao_filha_central_compras dadosDaCotacaoFilha =
                    dRepositoryCotacaoFilhaRecebida.ConsultarDadosDaCotacaoFilhaCCPeloIdCotacao(idCotacaoFilha);

                List<ListaDeItensCotadosViewModel> listaDeItensDaCotacao = dRepository.CarregarOsItensDeUmaCotacaoRecebida(idCotacaoFilha);

                //POPULAR CAMPOS ESSENCIAIS PARA EXIBIÇÃO DE ITENS COTADO
                for (int i = 0; i < listaDeItensDaCotacao.Count; i++)
                {
                    listaDeItensDaCotacao[i].idItemCotado = listaDeItensDaCotacao[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS;
                    listaDeItensDaCotacao[i].produtoCotado = listaDeItensDaCotacao[i].DESCRICAO_PRODUTO_EDITADO_COTADA_CENTRAL_COMPRAS;
                    listaDeItensDaCotacao[i].quantidadeProdutoCotado =
                        listaDeItensDaCotacao[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                    listaDeItensDaCotacao[i].valorTabelaProdutoCotado =
                        listaDeItensDaCotacao[i].PRECO_UNITARIO_ITENS_TABELA_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                    listaDeItensDaCotacao[i].valorRespondidoProdutoCotado =
                        listaDeItensDaCotacao[i].PRECO_UNITARIO_ITENS_COTACAO_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                    listaDeItensDaCotacao[i].unidadeProdutoCotado = listaDeItensDaCotacao[i].unidadeProdutoCotado;

                    if (dadosDaCotacaoFilha.RECEBEU_CONTRA_PROPOSTA == true)
                    {
                        listaDeItensDaCotacao[i].valorContraPropostaProdutoCotado = 
                            listaDeItensDaCotacao[i].PRECO_UNITARIO_ITENS_CONTRA_PROPOSTA_CENTRAL_COMPRAS.ToString("C2", CultureInfo.CurrentCulture).Replace("R$", "");
                    }

                    //if (listaDeItensDaCotacao[i].PRODUTO_COTADO_CENTRAL_COMPRAS)
                    //{
                    //    listaDeItensDaCotacao[i].produtoCotadoNaoCotado = "sim";
                    //}
                    //else
                    //{
                    //    listaDeItensDaCotacao[i].produtoCotadoNaoCotado = "nao";
                    //}

                    if ((listaDeItensDaCotacao[i].PRODUTO_COTADO_CENTRAL_COMPRAS) && (dadosDaCotacaoFilha.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS))
                    {
                        listaDeItensDaCotacao[i].produtoCotadoNaoCotado = "sim";
                    }
                    else if ((listaDeItensDaCotacao[i].PRODUTO_COTADO_CENTRAL_COMPRAS == false) && (dadosDaCotacaoFilha.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS))
                    {
                        listaDeItensDaCotacao[i].produtoCotadoNaoCotado = "nao";
                    }
                    else
                    {
                        listaDeItensDaCotacao[i].produtoCotadoNaoCotado = "sim";
                    }

                    if (dadosDaCotacaoFilha.ACEITOU_CONTRA_PROPOSTA == true)
                    {
                        listaDeItensDaCotacao[i].aceitouContraProposta = "sim";
                    }

                    //CONSULTAR SE o PRODUTO FOI PEDIDO ao FORNECEDOR cuja RESPOSTA está sendo ANALISADA
                    itemPedidoCC =
                        negociosItensPedidoCC.ConsultarSeOFornecedorRecebeuPedidoParaEsteProduto(listaDeItensDaCotacao[i].ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_CENTRAL_COMPRAS);

                    if (itemPedidoCC)
                    {
                        listaDeItensDaCotacao[i].itemFoiPedido = "sim";
                    }
                    else
                    {
                        listaDeItensDaCotacao[i].itemFoiPedido = "nao";
                    }
                }

                return listaDeItensDaCotacao;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //SALVAR PREÇOS RESPONDIDOS para a COTAÇÃO RECEBIDA
        public bool ResponderACotacaoPelaPrimeiraVez(int idCotacaoFilha, string[] idsItensCotados2, string[] valoresTabelaItensCotados2, 
            string[] valoresDiferenciadosItensCotados2)
        {
            bool respostaItensGravada = false;
            int idItemCotado;
            decimal valorTabelaItemCotado = 0;
            decimal valorDiferenciadoRespondido = 0;

            for (int i = 0; i < idsItensCotados2.Length; i++)
            {
                idItemCotado = Convert.ToInt32(idsItensCotados2[i].Replace("[", "").Replace("]", "").Replace("\"", ""));
                valoresTabelaItensCotados2[i] = valoresTabelaItensCotados2[i].Replace("[", "").Replace("]", "").Replace("\"", "").Replace(".", ",");
                valoresDiferenciadosItensCotados2[i] = valoresDiferenciadosItensCotados2[i].Replace("[", "").Replace("]", "").Replace("\"", "").Replace(".", ",");

                if ((valoresTabelaItensCotados2[i] != "") && (valoresTabelaItensCotados2[i] != null))
                {
                    valorTabelaItemCotado = Convert.ToDecimal(valoresTabelaItensCotados2[i]);
                }
                else
                {
                    valorTabelaItemCotado = 0;
                }

                if ((valoresDiferenciadosItensCotados2[i] != "") && (valoresDiferenciadosItensCotados2[i] != null))
                {
                    valorDiferenciadoRespondido = Convert.ToDecimal(valoresDiferenciadosItensCotados2[i]);
                }
                else
                {
                    valorDiferenciadoRespondido = 0;
                }

                //SALVAR os VALORES RESPONDIDOS pelo FORNECEDOR
                respostaItensGravada = dRepository.GravarValoresRespondidosParaACotacao(idCotacaoFilha, idItemCotado, valorTabelaItemCotado, valorDiferenciadoRespondido);
            }

            return respostaItensGravada;
        }

        //CONSULTAR LISTA de ITENS da COTACAO FILHA
        public List<itens_cotacao_filha_negociacao_central_compras> ConsultarItensDaCotacaoDaCC(int iCCF)
        {
            return dRepository.ConsultarItensDaCotacaoDaCC(iCCF);
        }

        //DESFAZER MARCAÇÃO de ITEM de COTAÇÃO RESPONDIDO
        public void DesfazerMarcacaoDeItemDeCotacaoRespondido(int iCCF)
        {
            dRepository.DesfazerMarcacaoDeItemDeCotacaoRespondido(iCCF);
        }
    }
}
