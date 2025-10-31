using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaProdutoUnicoCotadoPorEmpresaViewModel : itens_cotacao_filha_negociacao_central_compras
    {
        public string menorPreco { get; set; }

        public string precoMenor { get; set; }

        public decimal subTotalMenorPreco { get; set; }

        public string valorTotalUnitarioVsQuantidade { get; set; }

        public string produtoCotadoSimNao { get; set; }
    }
}
