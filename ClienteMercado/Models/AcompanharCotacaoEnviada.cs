using System.Collections.Generic;

namespace ClienteMercado.Models
{
    //Dados para montagem do ACOMPANHAMENTO das COTAÇÕES
    public class AcompanharCotacaoEnviada
    {
        public string NOME_COTACAO_ENVIADA { get; set; }

        public string DATA_CRIACAO_COTACAO_ENVIADA { get; set; }

        public string STATUS_COTACAO_ENVIADA { get; set; }

        public string CATEGORIA_COTACAO_ENVIADA { get; set; }

        public string TIPO_COTACAO_ENVIADA { get; set; }

        public string DATA_ENCERRAMENTO_COTACAO_ENVIADA { get; set; }

        //public string CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE { get; set; }

        //public string OBSERVACAO_COTACAO_USUARIO_COTANTE { get; set; }

        public decimal PERCENTUAL_RESPONDIDA_COTACAO_ENVIADA { get; set; }

        public List<ProdutosDaCotacao> ListagemProdutosDaCotacaoEnviada { get; set; }

        public List<FornecedoresCotados> ListagemFornecedoresDaCotacaoEnviada { get; set; }

        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE_ALTERNATIVO { get; set; }

        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        public string ID_FORNECEDOR_COTACAO_ANALISADA { get; set; }
    }
}
