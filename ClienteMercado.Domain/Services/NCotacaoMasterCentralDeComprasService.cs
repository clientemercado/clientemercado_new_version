using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCotacaoMasterCentralDeComprasService
    {
        DCotacaoMasterCentralDeComprasRepository dRepository = new DCotacaoMasterCentralDeComprasRepository();

        //GRAVAR a COTAÇÃO MASTER
        public cotacao_master_central_compras GerarContacaoMasterDaCentralDeCompras(cotacao_master_central_compras obj)
        {
            return dRepository.GerarContacaoMasterDaCentralDeCompras(obj);
        }

        //CARREGA LISTA de COTAÇÕES MASTER da CENTRAL de COMPRAS selecionada (Obs: Funciona tbm como FILTRO)
        public List<ListaDeCotacaoesDaCentralDeComprasViewModel> ListaDeCotacoesDaCentralDeCompras(int cCC)
        {
            return dRepository.ListaDeCotacoesDaCentralDeCompras(cCC);
        }

        //CARREGA LISTA AUTOCOMPLETE de COTAÇÕES da CENTRAL de COMPRAS
        public List<cotacao_master_central_compras> CarregarListaAutoCompleteDasCotacoesDaCC(string term)
        {
            return dRepository.CarregarListaAutoCompleteDasCotacoesDaCC(term);
        }

        //CARREGAR DADOS da COTAÇÃO MASTER
        public string CarregarNomeDaCotacaoMaster(int iCM)
        {
            return dRepository.CarregarNomeDaCotacaoMaster(iCM);
        }

        //CONSULTAR DADOS da COTACAO MASTER
        public cotacao_master_central_compras ConsultarDadosDaCotacaoMasterCC(int iCM)
        {
            return dRepository.ConsultarDadosDaCotacaoMasterCC(iCM);
        }

        //SETAR COTAÇÃO MASTER como ENVIADA
        public void SetarCotacaoMasterComoEnviadaAosFornecedores(int iCM)
        {
            dRepository.SetarCotacaoMasterComoEnviadaAosFornecedores(iCM);
        }

        //VERIFICA se a COTAÇÃO já foi ENVIADA aos FORNECEDORES
        public bool VerificarSeACotacaoJahFoiEnviadaAosFornecedores(int idCotacaoMaster)
        {
            return dRepository.VerificarSeACotacaoJahFoiEnviadaAosFornecedores(idCotacaoMaster);
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE na tabela cotacao_master_central_compras
        public void SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM, int idFor)
        {
            dRepository.SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(iCM, idFor);
        }

        //SETAR COTAÇÃO MASTER COMO COM CONTRA-PROPOSTA RECEBIDA
        public void SetarCampoDeContraProposta(int iCM)
        {
            dRepository.SetarCampoDeContraProposta(iCM);
        }

        //CANCELAR MARCAÇÃO de CONTRA-PROPOSTA
        public void CancelarMarcacaoDeContraProposta(int iCM)
        {
            dRepository.CancelarMarcacaoDeContraProposta(iCM);
        }

        //SETAR NEGOCIAÇÃO COMO ACEITA por TODOS os COTANTES da CENTRAL de COMPRAS
        public void SetarEstaNegociaçãoComoAceitaPelosCotantes(int iCM)
        {
            dRepository.SetarEstaNegociaçãoComoAceitaPelosCotantes(iCM);
        }

        //IDENTIFICAR na COTAÇÃO MASTER o FORNECEDOR q RECEBEU PEDIDO
        public void SetarIdFornecedorNaCotacaoMaster(int iCM)
        {
            dRepository.SetarIdFornecedorNaCotacaoMaster(iCM);
        }

        //SETAR NULL no CAMPO relacionado ao ID do PEDIDO
        public void SetarNullNoIdDoPedidoNaCotacaoMaster(int iCM)
        {
            dRepository.SetarNullNoIdDoPedidoNaCotacaoMaster(iCM);
        }
    }
}
