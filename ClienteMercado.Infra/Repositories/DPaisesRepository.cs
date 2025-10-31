using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DPaisesRepository : RepositoryBase<paises_empresa_usuario>
    {
        //Busca lista de Países
        public List<paises_empresa_usuario> ListaPaises()
        {
            return _contexto.paises_empresa_usuario.OrderBy(m => m.PAIS_EMPRESA_USUARIO).ToList();
        }

        //BUSCAR CÓD. do PAÍS
        public paises_empresa_usuario BuscarCodigoDoPaisPeloNomeDoPais(string pais)
        {
            paises_empresa_usuario dadosDoPaisPesquisado = _contexto.paises_empresa_usuario.FirstOrDefault(m => (m.PAIS_EMPRESA_USUARIO.Contains(pais)));

            return dadosDoPaisPesquisado;
        }
    }
}
