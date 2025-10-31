using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoIndividualEmpresaCentralComprasService
    {
        DCotacaoIndividualEmpresaCentralComprasRepository dRepository = new DCotacaoIndividualEmpresaCentralComprasRepository();

        //VERIFICANDO se TEM / NÃO TEM COTAÇÃO ANEXADA
        public string VerificarSeEmpresaPossuiCotacaoIndividualAnexada(int iD_COTACAO_MASTER_CENTRAL_COMPRAS, int idEmpresaNaCC)
        {
            return dRepository.VerificarSeEmpresaPossuiCotacaoIndividualAnexada(iD_COTACAO_MASTER_CENTRAL_COMPRAS, idEmpresaNaCC);
        }

        //CARREGAR dados da COTAÇÃO INDIVIDUAL gerada (Se a empresa logada possuir...)
        public cotacao_individual_empresa_central_compras CarregarDadosDaCotacao(int iCM, int idEmpresaCC)
        {
            return dRepository.CarregarDadosDaCotacao(iCM, idEmpresaCC);
        }

        //GRAVAR a COTAÇÃO INDIVIDUAL
        public cotacao_individual_empresa_central_compras GerarRegistroDeCotacaoIndividual(cotacao_individual_empresa_central_compras obj)
        {
            return dRepository.GerarRegistroDeCotacaoIndividual(obj);
        }

        //SETAR como ANEXADO
        public void AnexarMinhaCotacaoNaCotacaoMaster(int iPCC, int iCM)
        {
            dRepository.AnexarMinhaCotacaoNaCotacaoMaster(iPCC, iCM);
        }

        //SETAR como DESANEXADO
        public void DesanexarMinhaCotacaoNaCotacaoMaster(int iPCC, int iCM)
        {
            dRepository.DesanexarMinhaCotacaoNaCotacaoMaster(iPCC, iCM);
        }

        //QUANTIDADE de EMPRESAS com COTAÇÃO ANEXADA na CC
        public int BuscarQuantidadeEmpresasComCotacaoAnexada(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            return dRepository.BuscarQuantidadeEmpresasComCotacaoAnexada(iD_COTACAO_MASTER_CENTRAL_COMPRAS);
        }

        //CARREGAR LISTA de COTAÇÕES INDIVIDUAIS ANEXADAS
        public List<ListaDeIdsDasCotacoesIndividuaisViewModel> BuscarListaDeCotacoesIndividuaisAnexadas(int iCM)
        {
            return dRepository.BuscarListaDeCotacoesIndividuaisAnexadas(iCM);
        }

        //BUSCAR EMPRESAS que ainda NÃO ANEXARAM suas COTAÇÕES
        public List<ListaDeIdsDasEmpresasQueAindaNaoAnexaramCotacao> BuscarEmpresasQueAindaNaoAnexaramSuasCotacoes(int iCM)
        {
            return dRepository.BuscarEmpresasQueAindaNaoAnexaramSuasCotacoes(iCM);
        }

        //CONSULTAR DADOS do ITEM da COTAÇÃO INDIVIDUAL
        public itens_cotacao_individual_empresa_central_compras ConsultarDadosDoItemDaCotacao(int iCM, int idItemDaCotacaoIndividual)
        {
            return dRepository.ConsultarDadosDoItemDaCotacao(iCM, idItemDaCotacaoIndividual);
        }

        //CONSULTAR IDS das EMPRESAS que ANEXARAM suas COTAÇÕES
        public List<ListaDeIdsDeEmpresasQueAnexaramACotacaoViewModel> BuscarListaDeIdsDeEmpresasQueAnexaramACotacao(int iCM)
        {
            return dRepository.BuscarListaDeIdsDeEmpresasQueAnexaramACotacao(iCM);
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE
        public List<cotacao_individual_empresa_central_compras> SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM)
        {
            return dRepository.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM);
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_ACEITA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagDeAceitacaoDaNegociacaoDaCotacaoRespondidaPelosFornecedores(int codCentralCompras, int iCM, 
            int idEmpresaAdm)
        {
            return dRepository.SetarFlagDeAceitacaoDaNegociacaoDaCotacaoRespondidaPelosFornecedores(codCentralCompras, iCM, idEmpresaAdm);
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_ACEITA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagConfirmandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(int iCM, int idEmpresaCC)
        {
            return dRepository.SetarFlagConfirmandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(iCM, idEmpresaCC);
        }

        //CARREGAR LISTA de EMPRESAS que JÁ REGISTRARAM o ACEITE dos VALORES COTADOS
        public List<ListaDeIdsDasCotacoesIndividuaisViewModel> BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(int iCM)
        {
            return dRepository.BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(iCM);
        }

        //CARREGAR LISTA de EMPRESAS COTANTES participantes desta COTAÇÃO
        public List<ListaDeEmpresasCotantesDeUmaCotacaoViewModel> BuscarListaDeEmpresasCotantesNestaCotacao(int iCM, int cCC)
        {
            List<ListaDeEmpresasCotantesDeUmaCotacaoViewModel> listaEmpresasDaCotacao = dRepository.BuscarListaDeEmpresasCotantesNestaCotacao(iCM, cCC);

            for (int i = 0; i < listaEmpresasDaCotacao.Count; i++)
            {
                listaEmpresasDaCotacao[i].enderecoCompletoDaEmpresaCotante =
                    listaEmpresasDaCotacao[i].tipoLogradouro.Trim() + " " + listaEmpresasDaCotacao[i].logradouro.Trim() + " - " + listaEmpresasDaCotacao[i].bairro;

                if ((listaEmpresasDaCotacao[i].apelidoUsuario == "") && (listaEmpresasDaCotacao[i].apelidoUsuario == null))
                {
                    listaEmpresasDaCotacao[i].apelidoUsuario = listaEmpresasDaCotacao[i].nomeUsuario;
                }

                if (listaEmpresasDaCotacao[i].ID_CODIGO_EMPRESA == Convert.ToInt32(listaEmpresasDaCotacao[0].idEmpresaAdmCC))
                {
                    listaEmpresasDaCotacao[i].ehEmpresaAdmDaCC = "sim";
                }
            }

            return listaEmpresasDaCotacao;
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_REJEITADA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagRejeitandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(int iCM, 
            int iD_EMPRESA_CENTRAL_COMPRAS)
        {
            return dRepository.SetarFlagRejeitandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(iCM, iD_EMPRESA_CENTRAL_COMPRAS);
        }
    }
}
