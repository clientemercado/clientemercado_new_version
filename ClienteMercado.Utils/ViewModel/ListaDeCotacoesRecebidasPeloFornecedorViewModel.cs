using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeCotacoesRecebidasPeloFornecedorViewModel : cotacao_filha_central_compras
    {
        public string nomeDaCotacao { get; set; }

        public string nomeDaCentralCompras { get; set; }

        public string tipoCotacao { get; set; }

        public int quantosForncedoresRespondendo { get; set; }

        public string dataRecebimentoCotacao { get; set; }

        public string dataEncerramentoCotacao { get; set; }

        public string descricaoRamoAtividade { get; set; }

        public int quantosForncedoresRespondenderam { get; set; }

        public string statusDaCotacao { get; set; }

        public int cCC { get; set; }
    }
}
