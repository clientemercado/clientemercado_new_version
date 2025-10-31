using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresasParticipantesCentralDeComprasRepository : RepositoryBase<empresas_participantes_central_de_compras>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;
        int? idUsuario = Sessao.IdUsuarioLogado;

        //EXCLUIR PARTICIPAÇÃO da EMPRESA
        public void ExcluirParticipacaoNaCentralDeCompras(int codCentralCompras, int codEmpresaParticipante)
        {
            empresas_participantes_central_de_compras participacaoNaCentralDeComprasAExcluir =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => (m.ID_CENTRAL_COMPRAS == codCentralCompras) && (m.ID_CODIGO_EMPRESA == codEmpresaParticipante));

            if (participacaoNaCentralDeComprasAExcluir != null)
            {
                _contexto.empresas_participantes_central_de_compras.Remove(participacaoNaCentralDeComprasAExcluir);
                _contexto.SaveChanges();
            }
        }

        //BUSCAR QUANTIDADE de EMPRESAS PARTICIPANTES da CENTRAL de COMPRAS
        public int BuscarQuantidadeDeEmpresasParticipantes(int iD_CENTRAL_COMPRAS)
        {
            int quantasEmpresasParticipam =
                _contexto.empresas_participantes_central_de_compras.Where(m => (m.ID_CENTRAL_COMPRAS == iD_CENTRAL_COMPRAS)
                && (m.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS == true)).ToList().Count();

            return quantasEmpresasParticipam;
        }

        //REGISTRAR a EMPRESA CONVIDADA na tabela empresas_participantes_central_de_compras como "AGUARDANDO RESPOSTA CONVITE"
        public void GravarConviteDeParticipacaoDaEmpresaNaCentralDeCompras(empresas_participantes_central_de_compras obj)
        {
            empresas_participantes_central_de_compras consultaParticipacao =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == obj.ID_CENTRAL_COMPRAS)
                && (m.ID_CODIGO_EMPRESA == obj.ID_CODIGO_EMPRESA)));

            if (consultaParticipacao == null)
            {
                _contexto.empresas_participantes_central_de_compras.Add(obj);
                _contexto.SaveChanges();
            }
        }

        //BUSCAR LISTA de EMPRESAS da CENTRAL de COMPRAS
        public List<empresas_participantes_central_de_compras> BuscarListaDeEmpresasParticipantesDaCC(int cCC)
        {
            var query = "SELECT * FROM empresas_participantes_central_de_compras WHERE ID_CENTRAL_COMPRAS = " + cCC;

            var result = _contexto.Database.SqlQuery<empresas_participantes_central_de_compras>(query).ToList();
            return result;
        }

        //BUSCAR LISTA de EMPRESAS PARTICIPANTES CONFIRMADAS na CENTRAL COMPRAS
        public List<empresas_participantes_central_de_compras> BuscarListaDeEmpresasConfirmadasComoParticipantesDaCC(int cCC)
        {
            var query = "SELECT EP.* FROM empresas_participantes_central_de_compras EP " +
                        "WHERE EP.ID_CENTRAL_COMPRAS = " + cCC + " AND EP.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS = 1";

            var result = _contexto.Database.SqlQuery<empresas_participantes_central_de_compras>(query).ToList();
            return result;
        }

        //CONSULTAR DADOS da EMPRESA PARTICIPANTE da CENTRAL COMPRAS
        public empresas_participantes_central_de_compras BuscarDadosDaEmpresaParticipanteDaCC(int cCC, int idEmpresaCC)
        {
            var query = "SELECT EP.* FROM empresas_participantes_central_de_compras EP " +
                        "WHERE EP.ID_CENTRAL_COMPRAS = " + cCC + " AND EP.ID_EMPRESA_CENTRAL_COMPRAS = " + idEmpresaCC;

            var result = _contexto.Database.SqlQuery<empresas_participantes_central_de_compras>(query).FirstOrDefault();
            return result;
        }

        //PEGAR ID da EMPRESA LOGADA na CENTRAL de COMPRAS
        public empresas_participantes_central_de_compras BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(int cCC, int idEmpresaLogada)
        {
            var query = "SELECT EP.* FROM empresas_participantes_central_de_compras EP " +
                        "WHERE EP.ID_CENTRAL_COMPRAS = " + cCC + " AND EP.ID_CODIGO_EMPRESA = " + idEmpresaLogada;

            var result = _contexto.Database.SqlQuery<empresas_participantes_central_de_compras>(query).FirstOrDefault();
            return result;
        }

        //VERIFICAR SE A EMPRESA LOGADA RECEBEU CONVITE DA CENTRAL COMPRAS em QUESTÃO
        public empresas_participantes_central_de_compras ConsultarSeEmpresaTemConvitePendenteNestaCC(int cCC)
        {
            empresas_participantes_central_de_compras dadosDaEmpresaParticipanteCC =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == cCC) && (m.ID_CODIGO_EMPRESA == idEmpresa) && (m.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS == false)));

            return dadosDaEmpresaParticipanteCC;
        }

        //VERIFICAR PARTICIPAÇÃO da EMPRESA na CENTRAL de COMPRAS
        public empresas_participantes_central_de_compras ConsultarSeEmpresaParticipaDaCentralDeCompras(int cCC)
        {
            empresas_participantes_central_de_compras dadosDaEmpresaParticipanteCC =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == cCC) && (m.ID_CODIGO_EMPRESA == idEmpresa) && (m.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS == true)));

            return dadosDaEmpresaParticipanteCC;
        }

        //REGISTRAR ACEITAÇÃO do CONVITE
        public void AceitarConviteDeParticipacaoNaCC(int codCC)
        {
            empresas_participantes_central_de_compras dadosEmpresaParticipanteCC =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == codCC) && (m.ID_CODIGO_EMPRESA == idEmpresa)));

            if (dadosEmpresaParticipanteCC != null)
            {
                dadosEmpresaParticipanteCC.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS = true;
                dadosEmpresaParticipanteCC.DATA_ADESAO_EMPRESA_CENTRAL_COMPRAS = DateTime.Now;
                dadosEmpresaParticipanteCC.ID_CODIGO_USUARIO_ACEITOU = (int)idUsuario;

                _contexto.SaveChanges();
            }
        }

        //VERIFICAR se a EMPRESA em questâo já ESTÁ ATIVA na CENTRAL de COMPRAS
        public bool VerificarSeEmpresaParticipanteAceitouConviteParticipacaoCC(int cCC)
        {
            bool conviteAceito = false;

            empresas_participantes_central_de_compras conviteAceitoParticipacaoCC =
                _contexto.empresas_participantes_central_de_compras.FirstOrDefault(m => ((m.ID_CENTRAL_COMPRAS == cCC) && (m.ID_CODIGO_EMPRESA == idEmpresa)));

            if (conviteAceitoParticipacaoCC != null)
            {
                if (conviteAceitoParticipacaoCC.CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS)
                {
                    conviteAceito = true;
                }
            }

            return conviteAceito;
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS das quais a EMPRESA LOGADA participa
        public List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel> BuscarListaDeCentraisDeComprasQueParticipo()
        {
            var query = "SELECT EP.* FROM empresas_participantes_central_de_compras EP " +
                        "WHERE EP.ID_CODIGO_EMPRESA = " + idEmpresa;

            var result = _contexto.Database.SqlQuery<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel>(query).ToList();
            return result;
        }
    }
}
