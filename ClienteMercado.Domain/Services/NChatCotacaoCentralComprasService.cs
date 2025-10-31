using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NChatCotacaoCentralComprasService
    {
        DChatCotacaoCentralComprasRepository dRepository = new DChatCotacaoCentralComprasRepository();

        //Busca os dados do CHAT entre USUÁRIO ADM da CENTRAL COMPRAS e a EMPRESA FORNECEDORA
        public List<chat_cotacao_central_compras> BuscarChatEntreEmpresaCotanteEFornecedor(int idCotacaoFilhaCC)
        {
            return dRepository.BuscarChatEntreEmpresaCotanteEFornecedor(idCotacaoFilhaCC);
        }

        //Buscar conversa do CHAT entre USUÁRIO ADM da cENTRAL COMPRAS e a EMPRESA FORNECEDORA
        public List<chat_cotacao_central_compras> BuscarChatEntreUsuarioAdmDaCCEFornecedor(int idCotacaoFilha)
        {
            return dRepository.BuscarChatEntreUsuarioAdmDaCCEFornecedor(idCotacaoFilha);
        }

        //BUSCAR POSIÇÃO da ÚLTIMA CONVERSA no CHAT, CONFORME COTAÇÃO em QUESTÃO
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            return dRepository.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha);
        }

        //Gravar CONVERSA no CHAT - CENTRAL COMPRAS
        public chat_cotacao_central_compras GravarConversaNoChat(chat_cotacao_central_compras obj)
        {
            return dRepository.GravarConversaNoChat(obj);
        }
    }
}
