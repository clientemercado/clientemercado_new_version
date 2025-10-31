using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.UI.Core.ViewModel
{
    public class ListaDadosProdutoCotacaoViewModel
    {
        public int idItemCotacao { get; set; }
        public int idItemCotacaoIndividualEmpresa { get; set; }
        public string habilitar { get; set; }
        public string hint { get; set; }
        public string descricaoProdutoCotacao { get; set; }
        public string marcaProduto { get; set; }
        public string unidadeProdutoCotacao { get; set; }
        public string quantidadeProdutoCotacao { get; set; }
        public string valorUnitarioTabela { get; set; }
        public string valorUnitarioDiferenciado { get; set; }
        public string valorUnitarioContraProposta { get; set; }
        public string valorTotalUnitarioVsQuantidade { get; set; }
        public string somaTotalCalculado { get; set; }
        public bool temContraProposta { get; set; }
        public string ehOMenorPreco { get; set; }
        public List<ListaProdutoUnicoCotadoPorEmpresaViewModel> listaUmProdutoCotadoPorEmpresa { get; set; }
        public string somaSubTotais { get; set; }
        public string itemFoiPedido { get; set; }
        public string codControlePedido { get; set; }
    }
}
