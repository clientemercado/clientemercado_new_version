using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NChatCotacaoUsuarioCotanteService
    {
        DChatCotacaoUsuarioCotanteRepository dchatusuariocotante = new DChatCotacaoUsuarioCotanteRepository();

        //Consulta o NÚMERO da ORDEM do texto da conversa no CHAT
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            return dchatusuariocotante.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha);
        }

        //Gravar PERGUNTA ou RESPOSTA do CHAT
        public chat_cotacao_usuario_cotante GravarConversaNoChat(chat_cotacao_usuario_cotante obj)
        {
            return dchatusuariocotante.GravarConversaNoChat(obj);
        }

        //Buscar conversa do CHAT entre COTANTE e FORNECEDOR
        public List<chat_cotacao_usuario_cotante> BuscarChatEntreUsuarioCOtanteEFornecedor(int idCotacaoFilha)
        {
            return dchatusuariocotante.BuscarChatEntreUsuarioCOtanteEFornecedor(idCotacaoFilha);
        }
    }
}
