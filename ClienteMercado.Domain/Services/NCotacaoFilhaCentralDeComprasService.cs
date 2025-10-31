using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoFilhaCentralDeComprasService
    {
        DCotacaoFilhaCentralDeComprasRepository dcotacaofilhacentraldecompras = new DCotacaoFilhaCentralDeComprasRepository();

        //Consulta os dados da COTAÇÃO FILHA enviada pela CENTRAL de COMPRAS, a ser respondida pelo FORNECEDOR
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCentralDeComprasASerRespondida(cotacao_filha_central_compras obj)
        {
            return dcotacaofilhacentraldecompras.ConsultarDadosDaCotacaoFilhaCentralDeComprasASerRespondida(obj);
        }

        //QUANTIDADE de EMPRESAS respondendo a COTAÇÃO
        public int BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            return dcotacaofilhacentraldecompras.BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(iD_COTACAO_MASTER_CENTRAL_COMPRAS);
        }

        //QUANTIDADE de EMPRESAS que responderam a COTAÇÃO
        public int BuscarQuantidadeDeEmpresasJahResponderamACotacao(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            return dcotacaofilhacentraldecompras.BuscarQuantidadeDeEmpresasJahResponderamACotacao(iD_COTACAO_MASTER_CENTRAL_COMPRAS);
        }

        //BUSCAR LISTA de COTAÇÕES ENVIADAS
        public List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> BuscarListaDeCotacoesEnviadasConformeCotacaoMaster(int iCM)
        {
            return dcotacaofilhacentraldecompras.BuscarListaDeCotacoesEnviadasConformeCotacaoMaster(iCM);
        }

        //Gerar a COTAÇÃO FILHA (cópia da Cotação MASTER) para o FORNECEDOR que recebeu a COTAÇÃO
        public cotacao_filha_central_compras GerarCotacaoFilhaDaCC(cotacao_filha_central_compras obj)
        {
            return dcotacaofilhacentraldecompras.GerarCotacaoFilhaDaCC(obj);
        }

        //CARREGAR LISTA de IDS de EMPRESAS que RECEBERAM a COTAÇÃO
        public List<ListaIdsDeEmpresasFornecedorasCotadas> BuscaListaDeIdsDosFornecedoresCotados(int iCM)
        {
            return dcotacaofilhacentraldecompras.BuscaListaDeIdsDosFornecedoresCotados(iCM);
        }

        //CONSULTAR DADOS da COTAÇÃO FILHA
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCC(int iCM, int iCCF)
        {
            return dcotacaofilhacentraldecompras.ConsultarDadosDaCotacaoFilhaCC(iCM, iCCF);
        }

        //SETAR como 'true' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
        public void SetarCampoDeEnvioDeContraPropostaComoEnviada(int idCotacaoFilha)
        {
            dcotacaofilhacentraldecompras.SetarCampoDeEnvioDeContraPropostaComoEnviada(idCotacaoFilha);
        }

        //SETAR como 'false' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
        public void SetarCampoDeEnvioDeContraPropostaComoCancelada(int idCotacaoFilha)
        {
            dcotacaofilhacentraldecompras.SetarCampoDeEnvioDeContraPropostaComoCancelada(idCotacaoFilha);
        }

        //VERIFICAR SE JÁ FOI ENVIADO CONTRA-PROPOSTA A ALGUM OUTRO FORNECEDOR
        public List<cotacao_filha_central_compras> BuscarListaDeCotacoesFilhaQueReceberamContraProposta(int iCM)
        {
            return dcotacaofilhacentraldecompras.BuscarListaDeCotacoesFilhaQueReceberamContraProposta(iCM);
        }

        //BUSCAR LISTA de COTAÇÕES RECEBIDAS FILTRADAS
        public List<ListaDeCotacoesRecebidasPeloFornecedorViewModel> BuscarListaDeCotacoesRecebidasPeloFornecedorConformeFiltro(int tipoFiltragem, int codPesquisar,
            int idGrupoAtividadesFiltro)
        {
            var statusDaCotacaorecebida = "";

            NCotacaoFilhaCentralDeComprasService negociosCotacaoFilhaCC = new NCotacaoFilhaCentralDeComprasService();
            NCotacaoIndividualEmpresaCentralComprasService negociosCotacaoIndividual = new NCotacaoIndividualEmpresaCentralComprasService();
            NPedidoCentralComprasService negociosPedidosDaCotacaoCC = new NPedidoCentralComprasService();

            List<ListaDeCotacoesRecebidasPeloFornecedorViewModel> listaDeCotacaoesRecebidas =
                dcotacaofilhacentraldecompras.BuscarListaDeCotacoesRecebidasPeloFornecedorConformeFiltro(tipoFiltragem, codPesquisar, idGrupoAtividadesFiltro);

            for (int i = 0; i < listaDeCotacaoesRecebidas.Count; i++)
            {
                //QUANTIDADE de EMPRESAS respondendo a COTAÇÃO
                int quantidadeDeEmpresasRespondendoACotacao =
                    negociosCotacaoFilhaCC.BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(listaDeCotacaoesRecebidas[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);
                //QUANTIDADE de EMPRESAS que responderam a COTAÇÃO
                int quantidadeDeEmpresasJahResponderamACotacao =
                    negociosCotacaoFilhaCC.BuscarQuantidadeDeEmpresasJahResponderamACotacao(listaDeCotacaoesRecebidas[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);
                //BUSCA LISTA DE COTAÇÕES-ENVIADAS verificando se ALGUMA DELAS já recebeu uma CONTRA_PROPOSTA
                List<cotacao_filha_central_compras> quantidadeDeContraPospostasPraEstaCotacao =
                    negociosCotacaoFilhaCC.BuscarListaDeCotacoesFilhaQueReceberamContraProposta(listaDeCotacaoesRecebidas[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                //CARREGAR LISTA de EMPRESAS que JÁ REGISTRARAM o ACEITE dos VALORES COTADOS
                List<ListaDeIdsDasCotacoesIndividuaisViewModel> cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor =
                    negociosCotacaoIndividual.BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(listaDeCotacaoesRecebidas[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                listaDeCotacaoesRecebidas[i].quantosForncedoresRespondendo = quantidadeDeEmpresasRespondendoACotacao;
                listaDeCotacaoesRecebidas[i].quantosForncedoresRespondenderam = quantidadeDeEmpresasJahResponderamACotacao;

                if (listaDeCotacaoesRecebidas[i].tipoCotacao == "DIRECIONADA")
                {
                    listaDeCotacaoesRecebidas[i].tipoCotacao = "DIREC";
                }

                listaDeCotacaoesRecebidas[i].dataRecebimentoCotacao = listaDeCotacaoesRecebidas[i].DATA_RECEBEU_COTACAO_CENTRAL_COMPRAS.ToShortDateString();

                //EXIBE ou NÃO a DATA de ENCERRAMENTO da COTAÇÃO
                if (Convert.ToDateTime(listaDeCotacaoesRecebidas[i].dataEncerramentoCotacao) < DateTime.Now)
                {
                    listaDeCotacaoesRecebidas[i].dataEncerramentoCotacao = "";
                }

                //DEFINIR STATUS da COTAÇÃO RECEBIDA
                if (cotacoesIndividuaisQueRegistraramAceiteDaRespostaDoFornecedor.Count > 0)
                {
                    statusDaCotacaorecebida = "ANALISANDO RESP.";
                }
                else if (quantidadeDeContraPospostasPraEstaCotacao.Count > 0)
                {
                    statusDaCotacaorecebida = "ANALISANDO RESP.";
                }
                else
                {
                    statusDaCotacaorecebida = "NOVA";
                }

                //VERIFICAR se a COTAÇÃO se já se converteu em PEDIDO
                pedido_central_compras dadosDoPedido =
                    negociosPedidosDaCotacaoCC.VerificaSeACotacaoPossuiPedido(listaDeCotacaoesRecebidas[i].ID_COTACAO_MASTER_CENTRAL_COMPRAS);

                if (dadosDoPedido != null)
                {
                    statusDaCotacaorecebida = "ENCERRADA";
                }

                listaDeCotacaoesRecebidas[i].statusDaCotacao = statusDaCotacaorecebida;
            }

            return listaDeCotacaoesRecebidas;
        }

        //DADOS da COTAÇÃO FILHA RECEBIDA pela EMPRESA em questão
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCCPeloCodigoDaEmpresaFornecedora(int iCM, int idEmpresaCotada)
        {
            return dcotacaofilhacentraldecompras.ConsultarDadosDaCotacaoFilhaCCPeloCodigoDaEmpresaFornecedora(iCM, idEmpresaCotada);
        }

        //SETAR CONTRA-PROPOSTA COMO ACEITA para a COTAÇÃO
        public void SetarContraPropostaComoAceitaPeloFornecedor(int iCM, int iCCF)
        {
            dcotacaofilhacentraldecompras.SetarContraPropostaComoAceitaPeloFornecedor(iCM, iCCF);
        }

        //SETAR CONTRA-PROPOSTA COMO NÃO ACEITA para a COTAÇÃO
        public void SetarContraPropostaComoNaoAceitaPeloFornecedor(int iCM, int iCCF)
        {
            dcotacaofilhacentraldecompras.SetarContraPropostaComoNaoAceitaPeloFornecedor(iCM, iCCF);
        }

        //SETAR RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarReceBimentoDePedidoParaACotacao(int iCM, int iCCF)
        {
            dcotacaofilhacentraldecompras.SetarReceBimentoDePedidoParaACotacao(iCM, iCCF);
        }

        //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarComoNaoReceBimentoDePedidoParaACotacao(int iCM, int iCCF)
        {
            dcotacaofilhacentraldecompras.SetarComoNaoReceBimentoDePedidoParaACotacao(iCM, iCCF);
        }

        //SETAR CONFIRMANDO o PEDIDO como ACEITO
        public void SetarConfirmandoAceiteDoPedido(int iCM, int iCCF, int idPedido, int idTipoFrete, int idFormaPagto, string dataEntrega)
        {
            dcotacaofilhacentraldecompras.SetarConfirmandoAceiteDoPedido(iCM, iCCF, idPedido, idTipoFrete, idFormaPagto, dataEntrega);
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_ACEITE_COTACAO na tabela cotacao_filha_central_compras
        public void SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM, int iCCF, int idFor)
        {
            dcotacaofilhacentraldecompras.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM, iCCF, idFor);
        }

        //SETAR ESTA COTAÇÃO como RESPONDIDA por este FORNECEDOR
        public void SetarComoRespondidaEstaCotacaoPorEsteFornecedor(int iCM, int iCCF)
        {
            dcotacaofilhacentraldecompras.SetarComoRespondidaEstaCotacaoPorEsteFornecedor(iCM, iCCF);
        }

        //DESFAZER MARCAÇÃO de COTAÇÃO ENVIADA
        public void DesfazerMarcacaoDeCotacaoRespondida(int iCM, int idEmpresaCotada, int iCCF)
        {
            dcotacaofilhacentraldecompras.DesfazerMarcacaoDeCotacaoRespondida(iCM, idEmpresaCotada, iCCF);
        }

        //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarDesistenciaDoFornecedorDePedidoParaACotacao(int iCM, int iCCF, string descMotivo)
        {
            dcotacaofilhacentraldecompras.SetarDesistenciaDoFornecedorDePedidoParaACotacao(iCM, iCCF, descMotivo);
        }

        ////BUSCAR APENAS 1 COTAÇÃO na LISTA de COTAÇÕES ENVIADAS
        //public List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> BuscarUmaCotacaoEnviadasConformeCotacaoMaster(int iCM)
        //{
        //    return dcotacaofilhacentraldecompras.BuscarUmaCotacaoEnviadasConformeCotacaoMaster(iCM);
        //}
    }
}
