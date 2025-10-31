using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoFilhaCentralDeComprasRepository : RepositoryBase<cotacao_filha_central_compras>
    {
        int idEmpresa = Convert.ToInt32(Sessao.IdEmpresaUsuario);

        //Consulta os dados da COTAÇÃO FILHA enviada pela CENTRAL de COMPRAS, a ser respondida pelo FORNECEDOR
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCentralDeComprasASerRespondida(cotacao_filha_central_compras obj)
        {
            cotacao_filha_central_compras buscarDadosDaCotacao =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS.Equals(obj.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS))
                && (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA)) && (m.ID_CODIGO_USUARIO.Equals(obj.ID_CODIGO_USUARIO)));

            return buscarDadosDaCotacao;
        }

        //QUANTIDADE de EMPRESAS respondendo a COTAÇÃO
        public int BuscarQuantidadeDeEmpresasRespondendoACotacaoDaCC(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            //List<cotacao_filha_central_compras> cotacoesFilhasEnviadas =
            //    _contexto.cotacao_filha_central_compras.Where(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iD_COTACAO_MASTER_CENTRAL_COMPRAS)).ToList();

            //return cotacoesFilhasEnviadas.Count;

            var query = " SELECT * " +
                        " FROM cotacao_filha_central_compras " +
                        " WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iD_COTACAO_MASTER_CENTRAL_COMPRAS;

            List<cotacao_filha_central_compras> cotacoesFilhasEnviadas =
                _contexto.Database.SqlQuery<cotacao_filha_central_compras>(query).ToList();

            return cotacoesFilhasEnviadas.Count;
        }

        //QUANTIDADE de EMPRESAS que responderam a COTAÇÃO
        public int BuscarQuantidadeDeEmpresasJahResponderamACotacao(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            List<cotacao_filha_central_compras> cotacoesFilhaRespondidas =
                _contexto.cotacao_filha_central_compras.Where(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iD_COTACAO_MASTER_CENTRAL_COMPRAS)
                && (m.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS == true))).ToList();

            return cotacoesFilhaRespondidas.Count;
        }

        //Gerar a COTAÇÃO FILHA (cópia da Cotação MASTER) para o FORNECEDOR que recebeu a COTAÇÃO
        public cotacao_filha_central_compras GerarCotacaoFilhaDaCC(cotacao_filha_central_compras obj)
        {
            cotacao_filha_central_compras cotacaoFilhaCentralCompras =
                _contexto.cotacao_filha_central_compras.Add(obj);
            _contexto.SaveChanges();

            return cotacaoFilhaCentralCompras;
        }

        //CONSULTAR DADOS da COTAÇÃO FILHA
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCC(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.Include("empresa_usuario").Include("cotacao_master_central_compras")
                .FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            return dadosDaCotacaoFilha;
        }

        //SETAR como 'true' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
        public void SetarCampoDeEnvioDeContraPropostaComoEnviada(int idCotacaoFilha)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RECEBEU_CONTRA_PROPOSTA = true;
                _contexto.SaveChanges();
            }
        }

        //VERIFICAR SE JÁ FOI ENVIADO CONTRA-PROPOSTA A ALGUM OUTRO FORNECEDOR
        public List<cotacao_filha_central_compras> BuscarListaDeCotacoesFilhaQueReceberamContraProposta(int iCM)
        {
            List<cotacao_filha_central_compras> cotacoesFilhaComContraProposta =
                _contexto.cotacao_filha_central_compras.Where(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) && (m.RECEBEU_CONTRA_PROPOSTA == true))).ToList();

            return cotacoesFilhaComContraProposta;
        }

        //BUSCAR LISTA de COTAÇÕES RECEBIDAS FILTRADAS
        public List<ListaDeCotacoesRecebidasPeloFornecedorViewModel> BuscarListaDeCotacoesRecebidasPeloFornecedorConformeFiltro(int tipoFiltragem, int codPesquisar,
            int idGrupoAtividadesFiltro)
        {
            var query = "";
            var complemento = "";

            if (tipoFiltragem == 1)
            {
                //P/ COTAÇÃO ESPECÍFICA
                complemento = " AND CM.ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + codPesquisar;
            }
            else if (tipoFiltragem == 2)
            {
                //P/ RAMO ATIVIDADE
                complemento = " AND CM.ID_GRUPO_ATIVIDADES = " + idGrupoAtividadesFiltro;
            }
            else if (tipoFiltragem == 3)
            {
                //P/ CENTRAL COMPRAS ESPECÍFICA
                complemento = " AND CC.ID_CENTRAL_COMPRAS = " + codPesquisar;
            }
            else if (tipoFiltragem == 4)
            {
                //DIRECIONADAS
                complemento = " AND CM.ID_CODIGO_TIPO_COTACAO = 1";
            }
            else if (tipoFiltragem == 5)
            {
                //AVULSAS
                complemento = " AND CM.ID_CODIGO_TIPO_COTACAO = 2";
            }

            query = "SELECT CF.*, CM.NOME_COTACAO_CENTRAL_COMPRAS as nomeDaCotacao, CC.NOME_CENTRAL_COMPRAS as nomeDaCentralCompras, TC.DESCRICAO_TIPO_COTACAO as tipoCotacao, " +
                    "CONVERT(VARCHAR(10), CM.DATA_ENCERRAMENTO_COTACAO_CENTRAL_COMPRAS, 103) as dataEncerramentoCotacao, GA.DESCRICAO_ATIVIDADE as descricaoRamoAtividade, " +
                    "CM.ID_CENTRAL_COMPRAS as cCC " +
                    "FROM cotacao_filha_central_compras CF " +
                    "INNER JOIN cotacao_master_central_compras CM ON (CM.ID_COTACAO_MASTER_CENTRAL_COMPRAS = CF.ID_COTACAO_MASTER_CENTRAL_COMPRAS) " +
                    "INNER JOIN central_de_compras CC ON (CC.ID_CENTRAL_COMPRAS = CM.ID_CENTRAL_COMPRAS) " +
                    "INNER JOIN tipos_cotacao TC ON(TC.ID_CODIGO_TIPO_COTACAO = CM.ID_CODIGO_TIPO_COTACAO) " +
                    "INNER JOIN grupo_atividades_empresa GA ON(GA.ID_GRUPO_ATIVIDADES = CM.ID_GRUPO_ATIVIDADES) " +
                    "WHERE ID_CODIGO_EMPRESA = " + idEmpresa + complemento;

            List<ListaDeCotacoesRecebidasPeloFornecedorViewModel> listaDeCotacaoesRecebidas =
                _contexto.Database.SqlQuery<ListaDeCotacoesRecebidasPeloFornecedorViewModel>(query).ToList();

            return listaDeCotacaoesRecebidas;
        }

        //CONSULTAR DADOS da COTAÇÃO FILHA pelo ID da COTAÇÃO
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCCPeloIdCotacao(int idCotacaoFilha)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha));

            return dadosDaCotacaoFilha;
        }

        //SETAR como 'false' o CAMPO RECEBEU_CONTRA_PROPOSTA da COTAÇÃO FILHA
        public void SetarCampoDeEnvioDeContraPropostaComoCancelada(int idCotacaoFilha)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == idCotacaoFilha));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.ACEITOU_CONTRA_PROPOSTA = false;
                dadosDaCotacaoFilha.REJEITOU_CONTRA_PROPOSTA = false;
                dadosDaCotacaoFilha.RECEBEU_CONTRA_PROPOSTA = false;
                _contexto.SaveChanges();
            }
        }

        //CARREGAR LISTA de IDS de EMPRESAS que RECEBERAM a COTAÇÃO
        public List<ListaIdsDeEmpresasFornecedorasCotadas> BuscaListaDeIdsDosFornecedoresCotados(int iCM)
        {
            var query = "SELECT ID_CODIGO_EMPRESA FROM cotacao_filha_central_compras WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM;

            List<ListaIdsDeEmpresasFornecedorasCotadas> listaDeIdsDasEmpresasCotadas =
                _contexto.Database.SqlQuery<ListaIdsDeEmpresasFornecedorasCotadas>(query).ToList();

            return listaDeIdsDasEmpresasCotadas;
        }

        //DADOS da COTAÇÃO FILHA RECEBIDA pela EMPRESA em questão
        public cotacao_filha_central_compras ConsultarDadosDaCotacaoFilhaCCPeloCodigoDaEmpresaFornecedora(int iCM, int idEmpresaCotada)
        {
            var query = "SELECT * FROM cotacao_filha_central_compras WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM + " AND ID_CODIGO_EMPRESA = " + idEmpresaCotada;
            var dadosCotacaoFilha = _contexto.Database.SqlQuery<cotacao_filha_central_compras>(query).FirstOrDefault();

            return dadosCotacaoFilha;
        }

        //SETAR CONTRA-PROPOSTA COMO ACEITA para a COTAÇÃO
        public void SetarContraPropostaComoAceitaPeloFornecedor(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.ACEITOU_CONTRA_PROPOSTA = true;
                _contexto.SaveChanges();
            }
        }

        //DESFAZER MARCAÇÃO de COTAÇÃO ENVIADA
        public void DesfazerMarcacaoDeCotacaoRespondida(int iCM, int idEmpresaCotada, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha = 
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) 
                && (m.ID_CODIGO_EMPRESA == idEmpresaCotada) 
                && (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS = false;
                _contexto.SaveChanges();
            }
        }

        //SETAR ESTA COTAÇÃO como RESPONDIDA por este FORNECEDOR
        public void SetarComoRespondidaEstaCotacaoPorEsteFornecedor(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS = true;
                _contexto.SaveChanges();
            }
        }

        //SETAR CONFIRMANDO o PEDIDO como ACEITO
        public void SetarConfirmandoAceiteDoPedido(int iCM, int iCCF, int idPedido, int idTipoFrete, int idFormaPagto, string dataEntrega)
        {
            var dataFormatada = 
                Convert.ToDateTime(dataEntrega).Year.ToString() + "-" + Convert.ToDateTime(dataEntrega).Month.ToString() + "-" + Convert.ToDateTime(dataEntrega).Day.ToString();

            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.CONFIRMOU_PEDIDO = true;
                dadosDaCotacaoFilha.DATA_CONFIRMOU_PEDIDO = DateTime.Now;
                dadosDaCotacaoFilha.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = idPedido;
                dadosDaCotacaoFilha.DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS = Convert.ToDateTime(dataFormatada);
                dadosDaCotacaoFilha.ID_FORMA_PAGAMENTO = idFormaPagto;
                dadosDaCotacaoFilha.ID_TIPO_FRETE = idTipoFrete;

                _contexto.SaveChanges();
            }
        }

        //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarComoNaoReceBimentoDePedidoParaACotacao(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) 
                && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RECEBEU_PEDIDO = false;
                dadosDaCotacaoFilha.DATA_RECEBEU_PEDIDO = Convert.ToDateTime("1900-01-01");
                dadosDaCotacaoFilha.CONFIRMOU_PEDIDO = false;
                dadosDaCotacaoFilha.DATA_CONFIRMOU_PEDIDO = Convert.ToDateTime("1900-01-01");
                dadosDaCotacaoFilha.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = null;
                //dadosDaCotacaoFilha.REJEITOU_PEDIDO = true;
                //dadosDaCotacaoFilha.DATA_REJEITOU_PEDIDO = DateTime.Now;
                //dadosDaCotacaoFilha.DESCRICAO_MOTIVO_REJEITOU_PEDIDO = descMotivo;
                dadosDaCotacaoFilha.DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS = null;
                dadosDaCotacaoFilha.ID_FORMA_PAGAMENTO = null;
                dadosDaCotacaoFilha.ID_TIPO_FRETE = 2;

                _contexto.SaveChanges();
            }
        }

        //SETAR COMO NÃO RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarDesistenciaDoFornecedorDePedidoParaACotacao(int iCM, int iCCF, string descMotivo)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) 
                && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RECEBEU_PEDIDO = false;
                dadosDaCotacaoFilha.DATA_RECEBEU_PEDIDO = Convert.ToDateTime("1900-01-01");
                dadosDaCotacaoFilha.CONFIRMOU_PEDIDO = false;
                dadosDaCotacaoFilha.DATA_CONFIRMOU_PEDIDO = Convert.ToDateTime("1900-01-01");
                dadosDaCotacaoFilha.ID_CODIGO_PEDIDO_CENTRAL_COMPRAS = null;
                dadosDaCotacaoFilha.REJEITOU_PEDIDO = true;
                dadosDaCotacaoFilha.DATA_REJEITOU_PEDIDO = DateTime.Now;
                dadosDaCotacaoFilha.DESCRICAO_MOTIVO_REJEITOU_PEDIDO = descMotivo;
                dadosDaCotacaoFilha.DATA_ENTREGA_PEDIDO_CENTRAL_COMPRAS = null;
                dadosDaCotacaoFilha.ID_FORMA_PAGAMENTO = null;
                dadosDaCotacaoFilha.ID_TIPO_FRETE = 2;

                _contexto.SaveChanges();
            }
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_ACEITE_COTACAO na tabela cotacao_filha_central_compras
        public void SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM, int iCCF, int idFor)
        {
            cotacao_filha_central_compras dadosCotacaoFilhaCC =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) && (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_CODIGO_EMPRESA == idFor)));

            if (dadosCotacaoFilhaCC != null)
            {
                dadosCotacaoFilhaCC.SOLICITAR_CONFIRMACAO_ACEITE_COTACAO = true;
                _contexto.SaveChanges();
            }
        }

        //SETAR RECEBIMENTO de PEDIDO pra esta COTACAO
        public void SetarReceBimentoDePedidoParaACotacao(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.RECEBEU_PEDIDO = true;
                dadosDaCotacaoFilha.DATA_RECEBEU_PEDIDO = DateTime.Now;
                dadosDaCotacaoFilha.REJEITOU_PEDIDO = false;
                dadosDaCotacaoFilha.DATA_REJEITOU_PEDIDO = Convert.ToDateTime("1900-01-01");
                dadosDaCotacaoFilha.DESCRICAO_MOTIVO_REJEITOU_PEDIDO = null;
                _contexto.SaveChanges();
            }
        }

        //SETAR CONTRA-PROPOSTA COMO NÃO ACEITA pelo FORNECEDOR para a COTAÇÃO / DESMARCANDO RECEBIMENTO de CONTRA-PROPOSTA
        public void SetarContraPropostaComoNaoAceitaPeloFornecedor(int iCM, int iCCF)
        {
            cotacao_filha_central_compras dadosDaCotacaoFilha =
                _contexto.cotacao_filha_central_compras.FirstOrDefault(m => ((m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS == iCCF) && (m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)));

            if (dadosDaCotacaoFilha != null)
            {
                dadosDaCotacaoFilha.ACEITOU_CONTRA_PROPOSTA = false;
                dadosDaCotacaoFilha.REJEITOU_CONTRA_PROPOSTA = true;
                _contexto.SaveChanges();
            }
        }

        ////BUSCAR APENAS 1 COTAÇÃO na LISTA de COTAÇÕES ENVIADAS
        //public List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> BuscarUmaCotacaoEnviadasConformeCotacaoMaster(int iCM)
        //{
        //    var query = "SELECT TOP 1 ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS " +
        //                "FROM cotacao_filha_central_compras " +
        //                "WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM;

        //    List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> listaDeCotacoesFilhaEnviadas =
        //        _contexto.Database.SqlQuery<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel>(query).ToList();

        //    return listaDeCotacoesFilhaEnviadas;
        //}

        //BUSCAR LISTA de COTAÇÕES ENVIADAS
        public List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> BuscarListaDeCotacoesEnviadasConformeCotacaoMaster(int iCM)
        {
            var query = "SELECT ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS, ID_COTACAO_MASTER_CENTRAL_COMPRAS, ID_CODIGO_EMPRESA, ID_CODIGO_USUARIO, " +
                        "ID_TIPO_FRETE, RESPONDIDA_COTACAO_FILHA_CENTRAL_COMPRAS, DATA_RESPOSTA_COTACAO_FILHA_CENTRAL_COMPRAS, " + 
                        "FORMA_PAGAMENTO_COTACAO_FILHA_CENTRAL_COMPRAS, TIPO_DESCONTO, PERCENTUAL_DESCONTO, PRECO_LOTE_ITENS_COTACAO_CENTRAL_COMPRAS, " +
                        "OBSERVACAO_COTACAO_CENTRAL_COMPRAS, RECEBEU_CONTRA_PROPOSTA, ACEITOU_CONTRA_PROPOSTA, REJEITOU_CONTRA_PROPOSTA " +
                        "FROM cotacao_filha_central_compras " +
                        "WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM;

            List<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel> listaDeCotacoesFilhaEnviadas =
                _contexto.Database.SqlQuery<ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel>(query).ToList();

            return listaDeCotacoesFilhaEnviadas;
        }
    }
}
