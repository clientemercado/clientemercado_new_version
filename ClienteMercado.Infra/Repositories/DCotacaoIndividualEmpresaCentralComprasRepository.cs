using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCotacaoIndividualEmpresaCentralComprasRepository : RepositoryBase<cotacao_individual_empresa_central_compras>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;

        //VERIFICANDO se TEM / NÃO TEM COTAÇÃO ANEXADA
        public string VerificarSeEmpresaPossuiCotacaoIndividualAnexada(int iD_COTACAO_MASTER_CENTRAL_COMPRAS, int idEmpresaNaCC)
        {
            try
            {
                var temCotacaoIndividualAnexada = "nao";

                cotacao_individual_empresa_central_compras dadosDaCotacaoIndividual =
                    _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iD_COTACAO_MASTER_CENTRAL_COMPRAS) && (m.ID_EMPRESA_CENTRAL_COMPRAS == idEmpresaNaCC)));

                if (dadosDaCotacaoIndividual != null)
                {
                    if (dadosDaCotacaoIndividual.COTACAO_INDIVIDUAL_ANEXADA)
                    {
                        temCotacaoIndividualAnexada = "sim";
                    }
                }

                return temCotacaoIndividualAnexada;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GRAVAR a COTAÇÃO INDIVIDUAL
        public cotacao_individual_empresa_central_compras GerarRegistroDeCotacaoIndividual(cotacao_individual_empresa_central_compras obj)
        {
            cotacao_individual_empresa_central_compras cotacaoJaRegistrada =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == obj.ID_COTACAO_MASTER_CENTRAL_COMPRAS)
                && (m.ID_EMPRESA_CENTRAL_COMPRAS == obj.ID_EMPRESA_CENTRAL_COMPRAS)));

            if (cotacaoJaRegistrada == null)
            {
                cotacao_individual_empresa_central_compras dadosDoRegistroDaCotacaoIndividual =
                    _contexto.cotacao_individual_empresa_central_compras.Add(obj);
                _contexto.SaveChanges();

                return dadosDoRegistroDaCotacaoIndividual;
            }

            return null;
        }

        //CARREGAR LISTA de COTAÇÕES INDIVIDUAIS ANEXADAS
        public List<ListaDeIdsDasCotacoesIndividuaisViewModel> BuscarListaDeCotacoesIndividuaisAnexadas(int iCM)
        {
            var query = "SELECT ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS " +
                        "FROM cotacao_individual_empresa_central_compras WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM +
                        " AND COTACAO_INDIVIDUAL_ANEXADA = 1";

            var result = _contexto.Database.SqlQuery<ListaDeIdsDasCotacoesIndividuaisViewModel>(query).ToList();
            return result;
        }

        //CONSULTAR DADOS do ITEM da COTAÇÃO INDIVIDUAL
        public itens_cotacao_individual_empresa_central_compras ConsultarDadosDoItemDaCotacao(int iCM, int idItemDaCotacaoIndividual)
        {
            var query = "SELECT * " +
                        "FROM itens_cotacao_individual_empresa_central_compras " +
                        "WHERE ID_ITENS_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS = " + idItemDaCotacaoIndividual;

            var result = _contexto.Database.SqlQuery<itens_cotacao_individual_empresa_central_compras>(query).FirstOrDefault();
            return result;
        }

        //SETAR FLAG SOLICITAR_CONFIRMACAO_COTACAO como TRUE
        public List<cotacao_individual_empresa_central_compras> SetarFlagDeEnvioDeSolicitacaoDeConfirmacaoParaPedidoDosItensCotados(int iCM)
        {
            List<cotacao_individual_empresa_central_compras> listaCotacoesIndividuaisDaCotacaoMaster =
                _contexto.cotacao_individual_empresa_central_compras.Where(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)
                && (m.COTACAO_INDIVIDUAL_ANEXADA == true) && (m.NEGOCIACAO_COTACAO_ACEITA == false))).ToList();

            for (int i = 0; i < listaCotacoesIndividuaisDaCotacaoMaster.Count; i++)
            {
                listaCotacoesIndividuaisDaCotacaoMaster[i].SOLICITAR_CONFIRMACAO_COTACAO = true;
            }

            _contexto.SaveChanges();

            return listaCotacoesIndividuaisDaCotacaoMaster;
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_REJEITADA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagConfirmandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(int iCM, int idEmpresaCC)
        {
            cotacao_individual_empresa_central_compras dadosDaCotacaoIndividual =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) 
                && (m.ID_EMPRESA_CENTRAL_COMPRAS == idEmpresaCC)));

            if (dadosDaCotacaoIndividual != null)
            {
                dadosDaCotacaoIndividual.NEGOCIACAO_COTACAO_ACEITA = true;

                _contexto.SaveChanges();
            }

            return dadosDaCotacaoIndividual;
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_REJEITADA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagRejeitandoAceitacaoDosValoresNegociadosPorEmpresaAdmComOFornecedor(int iCM, int idEmpresaCC)
        {
            cotacao_individual_empresa_central_compras dadosDaCotacaoIndividual =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)
                && (m.ID_EMPRESA_CENTRAL_COMPRAS == idEmpresaCC)));

            if (dadosDaCotacaoIndividual != null)
            {
                dadosDaCotacaoIndividual.NEGOCIACAO_COTACAO_REJEITADA = true;

                _contexto.SaveChanges();
            }

            return dadosDaCotacaoIndividual;
        }

        //CARREGAR LISTA de EMPRESAS COTANTES participantes desta COTAÇÃO
        public List<ListaDeEmpresasCotantesDeUmaCotacaoViewModel> BuscarListaDeEmpresasCotantesNestaCotacao(int iCM, int cCC)
        {
            var query = "SELECT ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS as idCotacaoInvidualDaEmpresa, EP.ID_CODIGO_EMPRESA, EU.*, " +
                        "CI.NEGOCIACAO_COTACAO_ACEITA as aceitounaoaceitounegociacao, EEU.TIPO_LOGRADOURO_EMPRESA_USUARIO as tipoLogradouro, " +
                        "EEU.LOGRADOURO_CEP_EMPRESA_USUARIO as logradouro, B.BAIRRO_CIDADE_EMPRESA_USUARIO as bairro, CEU.CIDADE_EMPRESA_USUARIO as cidade, " +
                        "ES.UF_EMPRESA_USUARIO as ufEstado, UE.NICK_NAME_USUARIO as apelidoUsuario, UE.NOME_USUARIO as nomeUsuario, " +
                        "CC.ID_CODIGO_EMPRESA_ADM_CENTRAL_COMPRAS as idEmpresaAdmCC " +
                        "FROM cotacao_individual_empresa_central_compras CI " +
                        "INNER JOIN empresas_participantes_central_de_compras EP ON(EP.ID_EMPRESA_CENTRAL_COMPRAS = CI.ID_EMPRESA_CENTRAL_COMPRAS) " +
                        "INNER JOIN empresa_usuario EU ON(EU.ID_CODIGO_EMPRESA = EP.ID_CODIGO_EMPRESA) " +
                        "INNER JOIN usuario_empresa UE ON(UE.ID_CODIGO_EMPRESA = EU.ID_CODIGO_EMPRESA) " +
                        "INNER JOIN enderecos_empresa_usuario EEU ON(EEU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = EU.ID_CODIGO_ENDERECO_EMPRESA_USUARIO) " +
                        "INNER JOIN cidades_empresa_usuario CEU ON(CEU.ID_CIDADE_EMPRESA_USUARIO = EEU.ID_CIDADE_EMPRESA_USUARIO) " +
                        "INNER JOIN bairros_empresa_usuario B ON(B.ID_BAIRRO_EMPRESA_USUARIO = EEU.ID_BAIRRO_EMPRESA_USUARIO) " +
                        "INNER JOIN estados_empresa_usuario ES ON(ES.ID_ESTADOS_EMPRESA_USUARIO = CEU.ID_ESTADOS_EMPRESA_USUARIO) " +
                        "INNER JOIN central_de_compras CC ON (CC.ID_CENTRAL_COMPRAS = " + cCC + ")" +
                        "WHERE CI.ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM + " AND CI.COTACAO_INDIVIDUAL_ANEXADA = 1";

            var result = _contexto.Database.SqlQuery<ListaDeEmpresasCotantesDeUmaCotacaoViewModel>(query).ToList();
            return result;
        }

        //CARREGAR LISTA de EMPRESAS que JÁ REGISTRARAM o ACEITE dos VALORES COTADOS
        public List<ListaDeIdsDasCotacoesIndividuaisViewModel> BuscarListaDeEmpresasQueConfirmaramOsValoresRespondidosPorFornecedores(int iCM)
        {
            var query = "SELECT ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS " +
                        "FROM cotacao_individual_empresa_central_compras WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM +
                        " AND COTACAO_INDIVIDUAL_ANEXADA = 1 AND SOLICITAR_CONFIRMACAO_COTACAO = 1 AND NEGOCIACAO_COTACAO_ACEITA = 1";

            var result = _contexto.Database.SqlQuery<ListaDeIdsDasCotacoesIndividuaisViewModel>(query).ToList();
            return result;
        }

        //SETAR FLAG NEGOCIACAO_COTACAO_ACEITA como TRUE na tabela cotacao_individual_empresa_central_compras
        public cotacao_individual_empresa_central_compras SetarFlagDeAceitacaoDaNegociacaoDaCotacaoRespondidaPelosFornecedores(int codCentralCompras, int iCM, 
            int idEmpresaAdm)
        {
            cotacao_individual_empresa_central_compras dadosDaCotacaoIndividual = new cotacao_individual_empresa_central_compras();

            empresas_participantes_central_de_compras dadosEmpresasParticipantes =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == codCentralCompras) 
                && (m.ID_CODIGO_EMPRESA == idEmpresaAdm)));

            if (dadosEmpresasParticipantes != null)
            {
                dadosDaCotacaoIndividual =
                    _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) 
                    && (m.ID_EMPRESA_CENTRAL_COMPRAS == dadosEmpresasParticipantes.ID_EMPRESA_CENTRAL_COMPRAS)));

                if (dadosDaCotacaoIndividual != null)
                {
                    dadosDaCotacaoIndividual.NEGOCIACAO_COTACAO_ACEITA = true;
                    _contexto.SaveChanges();
                }
            }

            return dadosDaCotacaoIndividual;
        }

        //CONSULTAR IDS das EMPRESAS que ANEXARAM suas COTAÇÕES
        public List<ListaDeIdsDeEmpresasQueAnexaramACotacaoViewModel> BuscarListaDeIdsDeEmpresasQueAnexaramACotacao(int iCM)
        {
            var query = "SELECT ID_COTACAO_INDIVIDUAL_EMPRESA_CENTRAL_COMPRAS, EP.ID_CODIGO_EMPRESA, CI.NEGOCIACAO_COTACAO_ACEITA " +
                        "FROM cotacao_individual_empresa_central_compras CI " +
                        "INNER JOIN empresas_participantes_central_de_compras EP ON(EP.ID_EMPRESA_CENTRAL_COMPRAS = CI.ID_EMPRESA_CENTRAL_COMPRAS) " +
                        "WHERE CI.ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM +
                        "AND CI.COTACAO_INDIVIDUAL_ANEXADA = 1";

            var result = _contexto.Database.SqlQuery<ListaDeIdsDeEmpresasQueAnexaramACotacaoViewModel>(query).ToList();
            return result;
        }

        //BUSCAR LISTA de EMPRESAS que ainda NÃO ANEXARAM suas COTAÇÕES
        public List<ListaDeIdsDasEmpresasQueAindaNaoAnexaramCotacao> BuscarEmpresasQueAindaNaoAnexaramSuasCotacoes(int iCM)
        {
            var query = "SELECT EP.ID_CODIGO_EMPRESA " +
                        "FROM cotacao_individual_empresa_central_compras CI " +
                        "INNER JOIN empresas_participantes_central_de_compras EP ON(EP.ID_EMPRESA_CENTRAL_COMPRAS = CI.ID_EMPRESA_CENTRAL_COMPRAS) " +
                        "WHERE CI.ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iCM + " " +
                        "AND ((CI.COTACAO_INDIVIDUAL_ANEXADA = 0) OR (CI.COTACAO_INDIVIDUAL_ANEXADA IS NULL))";

            var result = _contexto.Database.SqlQuery<ListaDeIdsDasEmpresasQueAindaNaoAnexaramCotacao>(query).ToList();
            return result;
        }

        //QUANTIDADE de EMPRESAS com COTAÇÃO ANEXADA na CC
        public int BuscarQuantidadeEmpresasComCotacaoAnexada(int iD_COTACAO_MASTER_CENTRAL_COMPRAS)
        {
            var query = "SELECT * FROM cotacao_individual_empresa_central_compras WHERE ID_COTACAO_MASTER_CENTRAL_COMPRAS = " + iD_COTACAO_MASTER_CENTRAL_COMPRAS;
            var result = _contexto.Database.SqlQuery<cotacao_individual_empresa_central_compras>(query).ToList();

            return result.Count;
        }

        //SETAR como ANEXADO
        public void AnexarMinhaCotacaoNaCotacaoMaster(int iPCC, int iCM)
        {
            cotacao_individual_empresa_central_compras buscarCotacaoIndividual =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)
                && (m.ID_EMPRESA_CENTRAL_COMPRAS == iPCC)));

            if (buscarCotacaoIndividual != null)
            {
                buscarCotacaoIndividual.COTACAO_INDIVIDUAL_ANEXADA = true;
                _contexto.SaveChanges();
            }
        }

        //SETAR como DESANEXADO
        public void DesanexarMinhaCotacaoNaCotacaoMaster(int iPCC, int iCM)
        {
            cotacao_individual_empresa_central_compras buscarCotacaoIndividual =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM)
                && (m.ID_EMPRESA_CENTRAL_COMPRAS == iPCC)));

            if (buscarCotacaoIndividual != null)
            {
                buscarCotacaoIndividual.COTACAO_INDIVIDUAL_ANEXADA = false;
                _contexto.SaveChanges();
            }
        }

        //CARREGAR dados da COTAÇÃO INDIVIDUAL gerada (Se a empresa logada possuir...)
        public cotacao_individual_empresa_central_compras CarregarDadosDaCotacao(int iCM, int idEmpresaCC)
        {
            cotacao_individual_empresa_central_compras dadosDaCotacaoIndividual =
                _contexto.cotacao_individual_empresa_central_compras.FirstOrDefault(m => ((m.ID_COTACAO_MASTER_CENTRAL_COMPRAS == iCM) && (m.ID_EMPRESA_CENTRAL_COMPRAS == idEmpresaCC)));

            return dadosDaCotacaoIndividual;
        }
    }
}
