namespace ClienteMercado.Models
{
    public class ListaItensCotadosComRespostaDoFornecedor
    {
        public ListaItensCotadosComRespostaDoFornecedor()
        {

        }

        public string nomeProdutoCotado { get; set; }

        public string quantidadeCotada { get; set; }

        public string precoUnitario { get; set; }

        public string desconto { get; set; }

        public string valorTotal { get; set; }

        public string produtoAlternativo { get; set; }
    }
}
