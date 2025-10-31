using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeCotacoesEnviadasParaEmpresasCotadasViewModel : cotacao_filha_central_compras
    {
        public int idEmpresa_Cotada { get; set; }

        public string nomeFantasiaEmpresaCotada { get; set; }

        public string telefone1DaEmpresaCotada { get; set; }

        public string email1DaEmpresaCotada { get; set; }

        public string cotacaoFoiRespondida { get; set; }

        public string recebeuContraProposta { get; set; }

        public string aceitouContraProposta { get; set; }

        public string rejeitouContraProposta { get; set; }
    }
}
