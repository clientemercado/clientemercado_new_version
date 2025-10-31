using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaDeEmpresasCotantesDeUmaCotacaoViewModel : empresa_usuario
    {
        public int idCotacaoInvidualDaEmpresa { get; set; }

        public bool aceitounaoaceitounegociacao { get; set; }

        public string enderecoCompletoDaEmpresaCotante { get; set; }

        public string tipoLogradouro { get; set; }

        public string logradouro { get; set; }

        public string bairro { get; set; }

        public string cidade { get; set; }

        public string ufEstado { get; set; }

        public string apelidoUsuario { get; set; }

        public string nomeUsuario { get; set; }

        public int idEmpresaAdmCC { get; set; }

        public string ehEmpresaAdmDaCC { get; set; }
    }
}
