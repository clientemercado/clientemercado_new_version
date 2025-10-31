using System;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaPorItemCotadoJaCalculadoUsuarioCotanteViewModel
    {
        public int ID_DACOTACAO { get; set; }

        public int ID_PRODUTO_COTADO { get; set; }

        public decimal PRECO_FINAL_CALCULADO_DO_PRODUTO { get; set; }

        public int ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        public decimal QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        public decimal PRECO_ITENS_COTACAO_USUARIO_COTANTE { get; set; }

        public int ID_CODIGO_EMPRESA { get; set; }

        public int ID_CODIGO_USUARIO { get; set; }

        public int ID_TIPO_FRETE { get; set; }

        public int ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        public DateTime DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        public string FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE { get; set; }

        public string OBSERVACAO_COTACAO_USUARIO_COTANTE { get; set; }
    }
}
