using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class DadosDaCentralComprasViewModel : central_de_compras
    {
        public string tipoParticipacao { get; set; }
        public string mensagemRetorno { get; set; }
        public string idCCCriptografado { get; set; }
        public string idEmpresaAdmCriptografado { get; set; }
    }
}
