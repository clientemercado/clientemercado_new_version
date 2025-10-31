using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeDadosAgrupadosDasCotacoesIndividuaisDaCCViewModel : itens_cotacao_individual_empresa_central_compras
    {
        public int numeroItem { get; set; }

        public string descricaoProdutoCotacaoIndividual { get; set; }

        public string marcaFabricanteProdutoCotacaoIndividual { get; set; }

        public string unidadeProdutoCotacaoIndividual { get; set; }

        public string embalagemProdutoCotacaoIndividual { get; set; }

        public string quantidadeDoProdutoEmbalagem { get; set; }

        public string msgCotacaoEnviada { get; set; }
    }
}
