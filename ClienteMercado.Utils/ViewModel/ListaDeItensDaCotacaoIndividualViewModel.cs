using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeItensDaCotacaoIndividualViewModel : itens_cotacao_individual_empresa_central_compras
    {
        public int idItemCotacaoIndividual { get; set; }

        public int idProduto { get; set; }

        public string descricaoProdutoCotado { get; set; }

        public string marcaProdutoCotado { get; set; }

        public string codMarcaProdutoCotado { get; set; }

        public string quantidadeProdutoCotado { get; set; }

        public string unidadeProdutoCotado { get; set; }

        public int codUnidadeProduto { get; set; }

        public string embalagemProduto { get; set; }

        public int codEmbalagemProduto { get; set; }

        public bool recebeu_cp { get; set; }

        public bool aceitou_cp { get; set; }

        public decimal preco_unitario_resposta { get; set; }

        public decimal preco_unitario_contra_proposta { get; set; }

        public string valorDoProdutoCotado { get; set; }

        public string totalDoProdutoCotado { get; set; }

        public int ID_COTACAO_MASTER_CENTRAL_COMPRAS { get; set; }

        public string msgCotacaoEnviada { get; set; }

        public List<SelectListItem> listagemDeUnidadesProdutosACotar { get; set; }

        public string cotacaoNegociacaoAceita { get; set; }

        public string itemFoiPedido { get; set; }

        public string fornecedorItemPedido { get; set; }

        public bool pedidoConfirmado { get; set; }
    }
}
