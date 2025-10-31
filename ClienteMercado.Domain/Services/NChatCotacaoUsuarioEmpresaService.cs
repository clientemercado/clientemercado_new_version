using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NChatCotacaoUsuarioEmpresaService
    {
        DChatCotacaoUsuarioEmpresaRepository dchatusuarioempresa = new DChatCotacaoUsuarioEmpresaRepository();

        //Consulta o NÚMERO da ORDEM do texto da conversa no CHAT
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            return dchatusuarioempresa.ConsultarNumeroDeOrdemDeExibicaoNoChat(idCotacaoFilha);
        }

        //Gravar PERGUNTA ou RESPOSTA do CHAT
        public chat_cotacao_usuario_empresa GravarConversaNoChat(chat_cotacao_usuario_empresa obj, int idEmpresaCotada, string textoPerguntaOuResposta)
        {
            return dchatusuarioempresa.GravarConversaNoChat(obj, idEmpresaCotada, textoPerguntaOuResposta);
        }

        //Buscar conversa do CHAT entre COTANTE e FORNECEDOR
        public List<chat_cotacao_usuario_empresa> BuscarChatEntreUsuarioEmpresaEFornecedor(int idCotacaoFilha)
        {
            return dchatusuarioempresa.BuscarChatEntreUsuarioEmpresaEFornecedor(idCotacaoFilha);
        }
    }
}
