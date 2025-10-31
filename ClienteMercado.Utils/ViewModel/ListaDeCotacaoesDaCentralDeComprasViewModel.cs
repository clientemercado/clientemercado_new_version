using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeCotacaoesDaCentralDeComprasViewModel : cotacao_master_central_compras
    {
        public string nomeDaCotacao { get; set; }

        public string dataCriacaoDaCotacao { get; set; }

        public string dataEncerramentoCotacao { get; set; }

        public string grupoAtividades { get; set; }

        public int quantasEmpresas { get; set; }

        public int quantasEmpresasJahResponderam { get; set; }

        public string statusCotacao { get; set; }

        public string temCotacaoAnexada { get; set; }

        public string statusEnvioCotacao { get; set; }

        public bool cotacaoJahEnviadaAosFornecedores { get; set; }
    }
}
