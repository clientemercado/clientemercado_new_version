using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DChatCotacaoCentralComprasRepository : RepositoryBase<chat_cotacao_central_compras>
    {
        //Busca os dados do CHAT entre a EMPRESA COTANTE e a EMPRESA FORNECEDORA
        public List<chat_cotacao_central_compras> BuscarChatEntreEmpresaCotanteEFornecedor(int idCotacaoFilhaCC)
        {
            List<chat_cotacao_central_compras> buscarChatUsuarioEmpresaEFornecedor =
                _contexto.chat_cotacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS.Equals(idCotacaoFilhaCC))).ToList();

            return buscarChatUsuarioEmpresaEFornecedor;
        }

        //Buscar conversa do CHAT entre USUÁRIO ADM da cENTRAL COMPRAS e a EMPRESA FORNECEDORA
        public List<chat_cotacao_central_compras> BuscarChatEntreUsuarioAdmDaCCEFornecedor(int idCotacaoFilha)
        {
            List<chat_cotacao_central_compras> buscarChatUsuarioAdmCCEFornecedor =
                _contexto.chat_cotacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS.Equals(idCotacaoFilha))).ToList();

            return buscarChatUsuarioAdmCCEFornecedor;
        }

        //BUSCAR POSIÇÃO da ÚLTIMA CONVERSA no CHAT, CONFORME COTAÇÃO em QUESTÃO
        public int ConsultarNumeroDeOrdemDeExibicaoNoChat(int idCotacaoFilha)
        {
            List<chat_cotacao_central_compras> quantidadeDeConversasNoChat =
                _contexto.chat_cotacao_central_compras.Where(m => (m.ID_CODIGO_COTACAO_FILHA_CENTRAL_COMPRAS.Equals(idCotacaoFilha))).ToList();

            return quantidadeDeConversasNoChat.Count;
        }

        //Gravar CONVERSA no CHAT - CENTRAL COMPRAS
        public chat_cotacao_central_compras GravarConversaNoChat(chat_cotacao_central_compras obj)
        {
            chat_cotacao_central_compras gravarPerguntaOuRespostaNoChat =
                _contexto.chat_cotacao_central_compras.Add(obj);
            _contexto.SaveChanges();

            return gravarPerguntaOuRespostaNoChat;
        }
    }
}
