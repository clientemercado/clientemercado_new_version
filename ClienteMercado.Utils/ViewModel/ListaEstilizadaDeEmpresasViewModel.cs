using ClienteMercado.Data.Entities;
using System.Collections.Generic;

namespace ClienteMercado.Utils.ViewModel
{
    public class ListaEstilizadaDeEmpresasViewModel : empresa_usuario
    {
        public int numeroItem { get; set; }

        public int idEmpresa { get; set; }

        public string nomeEmpresa { get; set; }

        public string eMailEmpresa { get; set; }

        public string logradouroEmpresa { get; set; }

        public string cidadeEmpresa { get; set; }

        public string ufEmpresa { get; set; }

        public string usuarioContatoEmpresa { get; set; }

        public string CIDADE_EMPRESA_USUARIO { get; set; }

        public string NOME_USUARIO { get; set; }

        public List<ListaDeItensDaCotacaoIndividualViewModel> listaDeItensCotadosPorEmpresa { get; set; }
    }
}
