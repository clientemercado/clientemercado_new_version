using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCidadesService
    {
        DCidadesRepository dRepository = new DCidadesRepository();

        //Consultar dados da CIDADE
        public cidades_empresa_usuario ConsultarDadosDaCidade(int idCidade)
        {
            return dRepository.ConsultarDadosDaCidade(idCidade);
        }

        //CARREGA LISTA de CIDADES
        public List<ListaDeCidadesViewModel> CarregarListadeCidades(string term)
        {
            return dRepository.CarregarListadeCidades(term);
        }
    }
}
