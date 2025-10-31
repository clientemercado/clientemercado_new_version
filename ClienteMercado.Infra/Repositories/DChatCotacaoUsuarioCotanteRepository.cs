using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DChatCotacaoUsuarioCotanteRepository : RepositoryBase<cidades_empresa_usuario>
    {
        //Consulta o NÚMERO da ORDEM do texto da conversa no CHAT
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            List<chat_cotacao_usuario_cotante> quantidadeDeConversasNoChat =
                _contexto.chat_cotacao_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(idCotacaoFilha))).ToList();

            return quantidadeDeConversasNoChat.Count;
        }

        //Gravar PERGUNTA ou RESPOSTA do CHAT
        public chat_cotacao_usuario_cotante GravarConversaNoChat(chat_cotacao_usuario_cotante obj)
        {
            chat_cotacao_usuario_cotante gravarPerguntaOuRespostaNoChat =
                _contexto.chat_cotacao_usuario_cotante.Add(obj);
            _contexto.SaveChanges();

            return gravarPerguntaOuRespostaNoChat;
        }

        //Buscar conversa do CHAT entre COTANTE e FORNECEDOR
        public List<chat_cotacao_usuario_cotante> BuscarChatEntreUsuarioCOtanteEFornecedor(int idCotacaoFilha)
        {
            List<chat_cotacao_usuario_cotante> buscarChatUsuarioCotanteEFornecedor =
                _contexto.chat_cotacao_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(idCotacaoFilha))).ToList();

            return buscarChatUsuarioCotanteEFornecedor;
        }
    }
}
