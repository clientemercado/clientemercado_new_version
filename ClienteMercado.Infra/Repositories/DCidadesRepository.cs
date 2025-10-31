using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCidadesRepository : RepositoryBase<cidades_empresa_usuario>
    {
        //Consultar dados da CIDADE
        public cidades_empresa_usuario ConsultarDadosDaCidade(int idCidade)
        {
            cidades_empresa_usuario dadosDaCidade = _contexto.cidades_empresa_usuario.FirstOrDefault(m => (m.ID_CIDADE_EMPRESA_USUARIO.Equals(idCidade)));

            return dadosDaCidade;
        }

        //CARREGA LISTA de CIDADES
        public List<ListaDeCidadesViewModel> CarregarListadeCidades(string term)
        {
            var query = "SELECT C.* FROM cidades_empresa_usuario C WHERE C.CIDADE_EMPRESA_USUARIO LIKE '%" + term + "%'";

            var result = _contexto.Database.SqlQuery<ListaDeCidadesViewModel>(query).ToList();
            return result;
        }
    }
}
