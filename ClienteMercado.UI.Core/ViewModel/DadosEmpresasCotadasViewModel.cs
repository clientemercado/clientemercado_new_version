namespace ClienteMercado.UI.Core.ViewModel
{
    public class DadosEmpresasCotadasViewModel
    {
        public int idCodCotacaoFilhaEnviadaAoFornecedor { get; set; }

        public int idEmpresaCotada { get; set; }

        public string nomeFantasiaEmpresaCotada { get; set; }

        public string telefoneEmpresaCotada { get; set; }

        public string emailEmpresaCotada { get; set; }

        public string cotacaoFoiRespondida { get; set; }

        public string recebeuContraProposta { get; set; }

        public string aceitouContraProposta { get; set; }

        public string rejeitouContraProposta { get; set; }
    }
}
