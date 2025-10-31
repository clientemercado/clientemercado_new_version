using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.Models
{
    public class ResponderCotacaoRecebida
    {
        public string NOME_COTACAO_RECEBIDA { get; set; }

        public int ID_COTACAO_FILHA { get; set; }

        public int ID_COTACAO_MASTER { get; set; }

        public int ID_CODIGO_USUARIO_COTANTE { get; set; }

        public string DATA_CRIACAO_COTACAO_RECEBIDA { get; set; }

        public string STATUS_COTACAO_RECEBIDA { get; set; }

        public string CATEGORIA_COTACAO_RECEBIDA { get; set; }

        public string TIPO_COTACAO_RECEBIDA { get; set; }

        public string DATA_ENCERRAMENTO_COTACAO_RECEBIDA { get; set; }

        public string OBSERVACAO_COTACAO_USUARIO_COTANTE { get; set; }

        public decimal PERCENTUAL_RESPONDIDA_COTACAO_RECEBIDA { get; set; }

        public List<ProdutosDaCotacao> ListagemProdutosDaCotacaoRecebida { get; set; }

        public List<FornecedoresCotados> ListagemFornecedoresDaCotacao { get; set; }

        public string PERCENTUAL_DESCONTO_ASER_APLICADO { get; set; }

        public int APLICACAO_DO_DESCONTO { get; set; }

        public string IDS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DA_COTACAO { get; set; }

        public string VALORES_UNITARIOS_PRODUTOS_SELECIONADOS_PARA_RESPOSTA_DACOTACAO { get; set; }

        public string CONDICAO_PAGAMENTO_COTACAO_USUARIO_COTANTE { get; set; }

        public string VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR { get; set; }

        public decimal VALOR_TOTAL_DO_LOTE_DE_MERCADORIAS_SOMAR_SEM_DESCONTO { get; set; }

        public List<SelectListItem> ListagemTiposDeFrete { get; set; }
        public int ID_TIPO_DE_FRETE { get; set; }

        public string ID_PRODUTO_FOTOS { get; set; }

        public string ID_QUADRO_FOTOS { get; set; }

        public string QUANTIDADE_FOTOS_NOPRODUTO { get; set; }

        public string TIPO_IMAGEM_EXCLUSAO { get; set; }

        public string BOTAO_CONFIRMAR_FOTOS { get; set; }

        public string LISTA_NOMES_FOTOS_PRODUTOS_ALTERNATIVOS { get; set; }

        public List<FotosProdutosAlternativos> ListagemDasFotosDeProdutosAlternativosAnexadasAosItensDaCotacao { get; set; }

        public string ID_EMPRESA { get; set; }

        public string ID_USUARIO { get; set; }

        public string COTACAO_EDITADA { get; set; }

        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE { get; set; }

        public string TEXTO_CHAT_COTACAO_USUARIO_COTANTE_ALTERNATIVO { get; set; }

        public string COTACAO_RESPONDIDA { get; set; }
    }
}
