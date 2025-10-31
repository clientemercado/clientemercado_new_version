using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoIndividualEmpresaCentralComprasService
    {
        DItensCotacaoIndividualEmpresaCentralComprasRepository dRepository = new DItensCotacaoIndividualEmpresaCentralComprasRepository();

        //GRAVAR ITENS da COTAÇÃO INDIVIDUAL
        public itens_cotacao_individual_empresa_central_compras GravarItemNaCotacaoIndividualDaEmpresa(itens_cotacao_individual_empresa_central_compras obj)
        {
            return dRepository.GravarItemNaCotacaoIndividualDaEmpresa(obj);
        }

        //CARREGAR LISTA de ITENS que compoem a COTAÇÂO INDIVUAL
        public List<ListaDeItensDaCotacaoIndividualViewModel> CarregarListaDeItensDaCotacaoIndividual(int iCI)
        {
            bool cotacaoEnviada = false;
            NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();

            List<ListaDeItensDaCotacaoIndividualViewModel> listaDeItensDaCotacaoIndividual = dRepository.CarregarListaDeItensDaCotacaoIndividual(iCI);

            //VERIFICA se a COTAÇÃO já foi ENVIADA aos FORNECEDORES
            if (listaDeItensDaCotacaoIndividual.Count > 0)
            {
                cotacaoEnviada =
                    negociosCotacaoMaster.VerificarSeACotacaoJahFoiEnviadaAosFornecedores(listaDeItensDaCotacaoIndividual[0].ID_COTACAO_MASTER_CENTRAL_COMPRAS);
            }

            if (listaDeItensDaCotacaoIndividual.Count > 0)
            {
                NProdutosServicosEmpresaProfissionalService negociosProdutosServicos = new NProdutosServicosEmpresaProfissionalService();
                NUnidadesProdutosService negociosUnidadeProduto = new NUnidadesProdutosService();
                NEmpresasProdutosEmbalagensService negociosProdutosEmbalagens = new NEmpresasProdutosEmbalagensService();
                NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();

                for (int i = 0; i < listaDeItensDaCotacaoIndividual.Count; i++)
                {
                    listaDeItensDaCotacaoIndividual[i].idItemCotacaoIndividual = listaDeItensDaCotacaoIndividual[i].ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS;
                    listaDeItensDaCotacaoIndividual[i].idProduto = listaDeItensDaCotacaoIndividual[i].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS;

                    //BUSCAR NOME do PRODUTO
                    produtos_servicos_empresa_profissional dadosDoProdutoDaCotacao =
                        negociosProdutosServicos.ConsultarDadosDoProdutoDaCotacao(listaDeItensDaCotacaoIndividual[i].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);
                    listaDeItensDaCotacaoIndividual[i].descricaoProdutoCotado = dadosDoProdutoDaCotacao.DESCRICAO_PRODUTO_SERVICO.ToUpper();

                    //BUSCAR UNIDADE do PRODUTO COTADO
                    unidades_produtos dadosDaUnidadeProduto = negociosUnidadeProduto.ConsultarDadosDaUnidadeDoProduto(listaDeItensDaCotacaoIndividual[i].ID_CODIGO_UNIDADE_PRODUTO);
                    listaDeItensDaCotacaoIndividual[i].unidadeProdutoCotado = dadosDaUnidadeProduto.DESCRICAO_UNIDADE_PRODUTO;
                    listaDeItensDaCotacaoIndividual[i].codUnidadeProduto = dadosDaUnidadeProduto.ID_CODIGO_UNIDADE_PRODUTO;

                    //BUSCAR EMBALAGEM do PRODUTO COTADO
                    empresas_produtos_embalagens dadosDaEmbalagem =
                        negociosProdutosEmbalagens.ConsultarDadosDaEmbalagemDoProduto(listaDeItensDaCotacaoIndividual[i].ID_EMPRESAS_PRODUTOS_EMBALAGENS);
                    listaDeItensDaCotacaoIndividual[i].embalagemProduto = dadosDaEmbalagem.DESCRICAO_PRODUTO_EMBALAGEM;
                    listaDeItensDaCotacaoIndividual[i].codEmbalagemProduto = dadosDaEmbalagem.ID_EMPRESAS_PRODUTOS_EMBALAGENS;

                    //BUSCAR EMPRESA FABRICANTE/MARCA
                    empresas_fabricantes_marcas dadosDoFabricanteMarca =
                        negociosFabricantesMarcas.ConsultarEmpresaFabricanteOuMarca(listaDeItensDaCotacaoIndividual[i].ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);
                    listaDeItensDaCotacaoIndividual[i].marcaProdutoCotado = dadosDoFabricanteMarca.DESCRICAO_EMPRESA_FABRICANTE_MARCAS;
                    listaDeItensDaCotacaoIndividual[i].codMarcaProdutoCotado = dadosDoFabricanteMarca.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS.ToString();

                    listaDeItensDaCotacaoIndividual[i].quantidadeProdutoCotado = String.Format("{0:0.00}", listaDeItensDaCotacaoIndividual[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS);

                    listaDeItensDaCotacaoIndividual[i].valorDoProdutoCotado = "0,00";

                    listaDeItensDaCotacaoIndividual[i].listagemDeUnidadesProdutosACotar = ListagemUnidadesDePesoEMedida();

                    if (cotacaoEnviada)
                    {
                        listaDeItensDaCotacaoIndividual[i].msgCotacaoEnviada = "EM COTACAO";
                    }
                }
            }

            return listaDeItensDaCotacaoIndividual;
        }

        //EXCLUIR ITEM da COTAÇÃO
        public void ExcluirItemDaCotacao(int codItemCotacao, int iCM)
        {
            dRepository.ExcluirItemDaCotacao(codItemCotacao, iCM);
        }

        //EXCLUIR TODOS os ITEMS da COTAÇÃO
        public void ExcluirTodosOsItemsDaCotacao(int iCI)
        {
            dRepository.ExcluirTodosOsItemsDaCotacao(iCI);
        }

        //Carrega as siglas relacionadas às Unidades de peso e medida para os produtos da Cotação
        private static List<SelectListItem> ListagemUnidadesDePesoEMedida()
        {
            //Buscar Unidades de peso e medida
            NUnidadesProdutosService negocioUnidadesProdutos = new NUnidadesProdutosService();
            List<unidades_produtos> listaUnidadesProdutos = negocioUnidadesProdutos.ListaUnidadesProdutos();

            List<SelectListItem> listUnidades = new List<SelectListItem>();

            listUnidades.Add(new SelectListItem { Text = "...", Value = "0" });

            foreach (var unidadesProdutos in listaUnidadesProdutos)
            {
                listUnidades.Add(new SelectListItem
                {
                    Text = unidadesProdutos.DESCRICAO_UNIDADE_PRODUTO,
                    Value = unidadesProdutos.ID_CODIGO_UNIDADE_PRODUTO.ToString()
                });
            }

            return listUnidades;
        }

        //GRAVAR DADOS EDITADOS do ITEM
        public void GravarItemEditadoNaCotacaoIndividualEmpresa(itens_cotacao_individual_empresa_central_compras obj)
        {
            dRepository.GravarItemEditadoNaCotacaoIndividualEmpresa(obj);
        }

        //BUSCAR ITENS da COTAÇÃO INDIVIDUAL - AGRUPADOS
        public List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel> CarregarListaDeItensAgrupadosDasCotacoesIndividuais(string listaIdsCotacoesIndividuais, int iCM)
        {
            bool cotacaoEnviada = false;
            NCotacaoMasterCentralDeComprasService negociosCotacaoMaster = new NCotacaoMasterCentralDeComprasService();

            List<ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel> listaDeItensAgrupadosDasCotacoesIndividuais =
                dRepository.CarregarListaDeItensAgrupadosDasCotacoesIndividuais(listaIdsCotacoesIndividuais);

            //VERIFICA se a COTAÇÃO já foi ENVIADA aos FORNECEDORES
            cotacaoEnviada =
                negociosCotacaoMaster.VerificarSeACotacaoJahFoiEnviadaAosFornecedores(iCM);

            //POPULAR CAMPOS para EXIBIÇÃO no GRID
            for (int i = 0; i < listaDeItensAgrupadosDasCotacoesIndividuais.Count; i++)
            {
                NProdutosServicosEmpresaProfissionalService negociosProdutosServico = new NProdutosServicosEmpresaProfissionalService();
                NEmpresasFabricantesMarcasService negociosFabricantesMarcas = new NEmpresasFabricantesMarcasService();
                NUnidadesProdutosService negociosUnidades = new NUnidadesProdutosService();
                NEmpresasProdutosEmbalagensService negociosEmbalagens = new NEmpresasProdutosEmbalagensService();

                listaDeItensAgrupadosDasCotacoesIndividuais[i].numeroItem = (i + 1);

                //BUSCAR NOME do PRODUTO
                listaDeItensAgrupadosDasCotacoesIndividuais[i].descricaoProdutoCotacaoIndividual =
                    negociosProdutosServico.ConsultarDescricaoDoProduto(listaDeItensAgrupadosDasCotacoesIndividuais[i].ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS);

                //BUSCAR MARCA
                listaDeItensAgrupadosDasCotacoesIndividuais[i].marcaFabricanteProdutoCotacaoIndividual =
                    negociosFabricantesMarcas.ConsultarDescricaoDaEmpresaFabricanteOuMarca(listaDeItensAgrupadosDasCotacoesIndividuais[i].ID_CODIGO_EMPRESA_FABRICANTE_MARCAS);

                //BUSCAR UNIDADE
                listaDeItensAgrupadosDasCotacoesIndividuais[i].unidadeProdutoCotacaoIndividual =
                    negociosUnidades.ConsultarDescricaoDaUnidadeDoProduto(listaDeItensAgrupadosDasCotacoesIndividuais[i].ID_CODIGO_UNIDADE_PRODUTO);

                //BUSCAR EMBALAGEM
                listaDeItensAgrupadosDasCotacoesIndividuais[i].embalagemProdutoCotacaoIndividual =
                    negociosEmbalagens.ConsultarDescricaoDaEmbalagemDoProduto(listaDeItensAgrupadosDasCotacoesIndividuais[i].ID_EMPRESAS_PRODUTOS_EMBALAGENS);

                listaDeItensAgrupadosDasCotacoesIndividuais[i].quantidadeDoProdutoEmbalagem =
                    String.Format("{0:0.00}", listaDeItensAgrupadosDasCotacoesIndividuais[i].QUANTIDADE_ITENS_COTACAO_CENTRAL_COMPRAS);

                if (cotacaoEnviada)
                {
                    listaDeItensAgrupadosDasCotacoesIndividuais[i].msgCotacaoEnviada = "EM COTAÇÃO";
                }
            }

            return listaDeItensAgrupadosDasCotacoesIndividuais;
        }

        //SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO e quem é o FORNECEDOR
        public void SetarItemComoPedido(int idItemPedido, int idFornecedor, int idPedidoGeradoCC)
        {
            dRepository.SetarItemComoPedido(idItemPedido, idFornecedor, idPedidoGeradoCC);
        }

        //DESFAZER SETAR PRODUTO da COTAÇÃO INDIVIDUAL como PEDIDO
        public itens_cotacao_individual_empresa_central_compras DesfazimentoDeItemComoPedido(int idItemPedido, int idPedido)
        {
            return dRepository.DesfazimentoDeItemComoPedido(idItemPedido, idPedido);
        }

        //DESFAZER SETAR TODOS os PRODUTOS da COTAÇÃO INDIVIDUAL como PEDIDO
        public void DesfazimentoDeTodosOsItensComoPedido(int idPedido)
        {
            dRepository.DesfazimentoDeTodosOsItensComoPedido(idPedido);
        }
    }
}
