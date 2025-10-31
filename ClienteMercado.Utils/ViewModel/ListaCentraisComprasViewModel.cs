using ClienteMercado.Data.Entities;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaCentraisComprasViewModel : central_de_compras
    {
        public string nomeCentralCompras { get; set; }

        public int idGrupoAtividades { get; set; }

        public string ramoAtividadeCentralCompras { get; set; }

        public int quantidadeEmpresasParticipantesCentralCompras { get; set; }

        public int quantasEmpresas { get; set; }

        public string cidadeDaCentralCompras { get; set; }

        public string ufDaCentralCompras { get; set; }

        public string statusCentralCompras { get; set; }

        public string statusConviteCentralCompras { get; set; }

        public int quantas { get; set; }

        public string condicaoDaEmpresaNaCC { get; set; }

        //public bool CONVITE_ACEITO_PARTICIPACAO_CENTRAL_COMPRAS { get; set; }

        //public bool conviteAceitoParticipacaoNaCC { get; set; }
    }
}
