using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DChatCotacaoUsuarioEmpresaRepository : RepositoryBase<chat_cotacao_usuario_empresa>
    {
        //Consulta o NÚMERO da ORDEM do texto da conversa no CHAT
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            List<chat_cotacao_usuario_empresa> quantidadeDeConversasNoChat =
                _contexto.chat_cotacao_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(idCotacaoFilha))).ToList();

            return quantidadeDeConversasNoChat.Count;
        }

        //Gravar PERGUNTA ou RESPOSTA do CHAT
        public chat_cotacao_usuario_empresa GravarConversaNoChat(chat_cotacao_usuario_empresa obj, int idEmpresaCotada, string textoPerguntaOuResposta)
        {
            chat_cotacao_usuario_empresa gravarPerguntaOuRespostaNoChat =
                _contexto.chat_cotacao_usuario_empresa.Add(obj);
            _contexto.SaveChanges();

            return gravarPerguntaOuRespostaNoChat;
        }

        //Buscar conversa do CHAT entre COTANTE e FORNECEDOR
        public List<chat_cotacao_usuario_empresa> BuscarChatEntreUsuarioEmpresaEFornecedor(int idCotacaoFilha)
        {
            List<chat_cotacao_usuario_empresa> buscarChatUsuarioEmpresaEFornecedor =
                _contexto.chat_cotacao_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(idCotacaoFilha))).ToList();

            return buscarChatUsuarioEmpresaEFornecedor;
        }
    }
}
