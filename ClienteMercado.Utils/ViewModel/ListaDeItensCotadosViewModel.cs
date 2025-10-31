using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeItensCotadosViewModel : itens_cotacao_filha_negociacao_central_compras
    {
        public int idItemCotado { get; set; }

        public int idProdutoCotado { get; set; }

        public string produtoCotado { get; set; }

        public string marcaProdutoCotado { get; set; }

        public string produtoEmbalagem { get; set; }

        public string quantidadeProdutoCotado { get; set; }

        public string unidadeProdutoCotado { get; set; }

        public string valorTabelaProdutoCotado { get; set; }

        public string valorRespondidoProdutoCotado { get; set; }

        public string valorContraPropostaProdutoCotado { get; set; }

        public int codigoProduto { get; set; }

        public string produtoCotadoNaoCotado { get; set; }

        public string aceitouContraProposta { get; set; }

        public string itemFoiPedido { get; set; }
    }
}
