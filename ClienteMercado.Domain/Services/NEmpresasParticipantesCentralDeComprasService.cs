using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresasParticipantesCentralDeComprasService
    {
        DEmpresasParticipantesCentralDeComprasRepository dRepository = new DEmpresasParticipantesCentralDeComprasRepository();

        //EXCLUIR a CENTRAL de COMPRAS
        public void ExcluirParticipacaoNaCentralDeCompras(int codCentralCompras, int codEmpresaParticipante)
        {
            dRepository.ExcluirParticipacaoNaCentralDeCompras(codCentralCompras, codEmpresaParticipante);
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS das quais a EMPRESA LOGADA participa
        public List<ListaDeParticipacoesDaEmpresaEmCentraisDeComprasViewModel> BuscarListaDeCentraisDeComprasQueParticipo()
        {
            return dRepository.BuscarListaDeCentraisDeComprasQueParticipo();
        }

        //BUSCAR QUANTIDADE de EMPRESAS PARTICIPANTES da CENTRAL de COMPRAS
        public int BuscarQuantidadeDeEmpresasParticipantes(int iD_CENTRAL_COMPRAS)
        {
            return dRepository.BuscarQuantidadeDeEmpresasParticipantes(iD_CENTRAL_COMPRAS);
        }

        //REGISTRAR a EMPRESA CONVIDADA na tabela empresas_participantes_central_de_compras como "AGUARDANDO RESPOSTA CONVITE"
        public void GravarConviteDeParticipacaoDaEmpresaNaCentralDeCompras(empresas_participantes_central_de_compras obj)
        {
            dRepository.GravarConviteDeParticipacaoDaEmpresaNaCentralDeCompras(obj);
        }

        //VERIFICAR se a EMPRESA em questâo já ESTÁ ATIVA na CENTRAL de COMPRAS
        public bool VerificarSeEmpresaParticipanteAceitouConviteParticipacaoCC(int cCC)
        {
            return dRepository.VerificarSeEmpresaParticipanteAceitouConviteParticipacaoCC(cCC);
        }

        //BUSCAR LISTA de EMPRESAS da CENTRAL de COMPRAS
        public List<empresas_participantes_central_de_compras> BuscarListaDeEmpresasParticipantesDaCC(int cCC)
        {
            return dRepository.BuscarListaDeEmpresasParticipantesDaCC(cCC);
        }

        //REGISTRAR ACEITAÇÃO do CONVITE
        public void AceitarConviteDeParticipacaoNaCC(int codCC)
        {
            dRepository.AceitarConviteDeParticipacaoNaCC(codCC);
        }

        //BUSCAR LISTA de EMPRESAS PARTICIPANTES CONFIRMADAS na CENTRAL COMPRAS
        public List<empresas_participantes_central_de_compras> BuscarListaDeEmpresasConfirmadasComoParticipantesDaCC(int cCC)
        {
            return dRepository.BuscarListaDeEmpresasConfirmadasComoParticipantesDaCC(cCC);
        }

        //VERIFICAR PARTICIPAÇÃO da EMPRESA na CENTRAL de COMPRAS
        public empresas_participantes_central_de_compras ConsultarSeEmpresaParticipaDaCentralDeCompras(int cCC)
        {
            return dRepository.ConsultarSeEmpresaParticipaDaCentralDeCompras(cCC);
        }

        //VERIFICAR SE A EMPRESA LOGADA RECEBEU CONVITE DA CENTRAL COMPRAS em QUESTÃO
        public empresas_participantes_central_de_compras ConsultarSeEmpresaTemConvitePendenteNestaCC(int cCC)
        {
            return dRepository.ConsultarSeEmpresaTemConvitePendenteNestaCC(cCC);
        }

        //CONSULTAR DADOS da EMPRESA PARTICIPANTE da CENTRAL COMPRAS
        public empresas_participantes_central_de_compras BuscarDadosDaEmpresaParticipanteDaCC(int cCC, int idEmpresaCC)
        {
            return dRepository.BuscarDadosDaEmpresaParticipanteDaCC(cCC, idEmpresaCC);
        }

        //PEGAR ID da EMPRESA LOGADA na CENTRAL de COMPRAS
        public empresas_participantes_central_de_compras BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(int cCC, int idEmpresaLogada)
        {
            return dRepository.BuscarDadosDaEmpresaParticipanteDaCCPorIDdaEmpresa(cCC, idEmpresaLogada);
        }
    }
}
