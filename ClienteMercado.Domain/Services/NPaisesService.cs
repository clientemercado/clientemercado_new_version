using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NPaisesService
    {
        DPaisesRepository dpaises = new DPaisesRepository();

        public List<paises_empresa_usuario> ListaPaises()
        {
            //Busca lista de Países
            return dpaises.ListaPaises();
        }

        //BUSCAR CÓD. do PAÍS
        public paises_empresa_usuario BuscarCodigoDoPaisPeloNomeDoPais(string pais)
        {
            return dpaises.BuscarCodigoDoPaisPeloNomeDoPais(pais);
        }
    }
}
