using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoMasterCentralDeComprasRepository : RepositoryBase<cotacao_master_central_compras>
    {
        //GRAVAR a COTAÇÃO MASTER
        public cotacao_master_central_compras GerarContacaoMasterDaCentralDeCompras(cotacao_master_central_compras obj)
        {
            cotacao_master_central_compras cotacaoMasterGerada = new cotacao_master_central_compras();

            cotacao_master_central_compras dadosDaCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == obj.ID_CENTRAL_COMPRAS) && (m.NOME_COTACAO_CENTRAL_COMPRAS == obj.NOME_COTACAO_CENTRAL_COMPRAS)));

            if (dadosDaCotacaoMaster == null)
            {
                cotacaoMasterGerada =
                    _contexto.cotacao_master_central_compras.Add(obj);
                _contexto.SaveChanges();

                return cotacaoMasterGerada;
            }

            return null;
        }

        //CARREGA LISTA de COTAÇÕES MASTER da CENTRAL de COMPRAS selecionada (Obs: Funciona tbm como FILTRO)
        public List<ListaDeCotacaoesDaCentralDeComprasViewModel> ListaDeCotacoesDaCentralDeCompras(int cCC)
        {
            var query = "";

            query = "SELECT CM.* FROM cotacao_master_central_compras CM WHERE CM.ID_CENTRAL_COMPRAS = " + cCC;

            var result = _contexto.Database.SqlQuery<ListaDeCotacaoesDaCentralDeComprasViewModel>(query).ToList();
            return result;
        }

        //CONSULTAR DADOS da COTACAO MASTER
        public cotacao_master_central_compras ConsultarDadosDaCotacaoMasterCC(int iCM)
        {
            cotacao_master_central_compras dadosDaCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            return dadosDaCotacaoMaster;
        }

        //VERIFICA se a COTAÇÃO já foi ENVIADA aos FORNECEDORES
        public bool VerificarSeACotacaoJahFoiEnviadaAosFornecedores(int idCotacaoMaster)
        {
            var query = "";
            bool cotacaoEnviada = false;

            query = "SELECT CM.* FROM cotacao_master_central_compras CM WHERE CM.ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + idCotacaoMaster;
            var result = _contexto.Database.SqlQuery<cotacao_master_central_compras>(query).FirstOrDefault();

            if (result != null)
            {
                cotacaoEnviada = result.COTACAO_ENVIADA_FORNECEDORES;
            }

            return cotacaoEnviada;
        }

        //CANCELAR MARCAÇÃO de CONTRA-PROPOSTA
        public void CancelarMarcacaoDeContraProposta(int iCM)
        {
            cotacao_master_central_compras dadosCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosCotacaoMaster != null)
            {
                dadosCotacaoMaster.NEGOCIACAO_CONTRA_PROPOSTA = false;
                _contexto.SaveChanges();
            }
        }

        //IDENTIFICAR na COTAÇÃO MASTER o FORNECEDOR q RECEBEU PEDIDO
        public void SetarIdFornecedorNaCotacaoMaster(int iCM)
        {
            cotacao_master_central_compras dadosCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosCotacaoMaster != null)
            {
                dadosCotacaoMaster.ID_EMPRESA_FORNECEDORA_APROVADA = Sessao.IdEmpresaUsuario;

                _contexto.SaveChanges();
            }
        }

        //SETAR NULL no CAMPO relacionado ao ID do PEDIDO
        public void SetarNullNoIdDoPedidoNaCotacaoMaster(int iCM)
        {
            cotacao_master_central_compras dadosCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosCotacaoMaster != null)
            {
                dadosCotacaoMaster.ID_EMPRESA_FORNECEDORA_APROVADA = null;
                _contexto.SaveChanges();
            }
        }

        //SETAR NEGOCIAÇÃO COMO ACEITA por TODOS os COTANTES da CENTRAL de COMPRAS
        public void SetarEstaNegociaçãoComoAceitaPelosCotantes(int iCM)
        {
            cotacao_master_central_compras dadosCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosCotacaoMaster != null)
            {
                dadosCotacaoMaster.NEGOCIACAO_COTACAO_ACEITA = true;
                _contexto.SaveChanges();
            }
        }

        //SETAR COTAÇÃO MASTER COMO COM CONTRA-PROPOSTA RECEBIDA
        public void SetarCampoDeContraProposta(int iCM)
        {
            cotacao_master_central_compras dadosCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosCotacaoMaster != null)
            {
                dadosCotacaoMaster.NEGOCIACAO_CONTRA_PROPOSTA = true;
                _contexto.SaveChanges();
            }
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE na tabela cotacao_master_central_compras
        public void SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM, int idFor)
        {
            cotacao_master_central_compras dadosDaCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosDaCotacaoMaster != null)
            {
                dadosDaCotacaoMaster.SOLICITAR_CONFIRMACAO_COTACAO = true;
                dadosDaCotacaoMaster.ID_EMPRESA_FORNECEDORA_APROVACAO = idFor;

                _contexto.SaveChanges();
            }
        }

        //SETAR COTAÇÃO MASTER como ENVIADA
        public void SetarCotacaoMasterComoEnviadaAosFornecedores(int iCM)
        {
            cotacao_master_central_compras dadosDaCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosDaCotacaoMaster != null)
            {
                dadosDaCotacaoMaster.COTACAO_ENVIADA_FORNECEDORES = true;
                _contexto.SaveChanges();
            }
        }

        //CARREGAR DADOS da COTAÇÃO MASTER
        public string CarregarNomeDaCotacaoMaster(int iCM)
        {
            var nomeCotacaoMaster = "";

            cotacao_master_central_compras dadosDaCotacaoMaster =
                _contexto.cotacao_master_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM));

            if (dadosDaCotacaoMaster != null)
            {
                nomeCotacaoMaster = dadosDaCotacaoMaster.NOME_COTACAO_CENTRAL_COMPRAS;
            }

            return nomeCotacaoMaster;
        }

        //CARREGA LISTA AUTOCOMPLETE de COTAÇÕES da CENTRAL de COMPRAS
        public List<cotacao_master_central_compras> CarregarListaAutoCompleteDasCotacoesDaCC(string term)
        {
            var query = "SELECT CM.* FROM cotacao_master_central_compras CM WHERE CM.NOME_COTACAO_CENTRAL_COMPRAS LIKE '%" + term + "%'";

            var result = _contexto.Database.SqlQuery<cotacao_master_central_compras>(query).ToList();
            return result;
        }
    }
}
