using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresasProdutosMarcasRepository : RepositoryBase<empresas_produtos_marcas>
    {
        //GRAVAR VÍNCULO da MARCA ou FABRICANTE ao PRODUTO
        public void GravarVinculoDaMarcaOuFabricanteAoProduto(empresas_produtos_marcas obj)
        {
            empresas_produtos_marcas jahExisteVinculoParaEmpresaProdutoMarca =
                _contexto.empresas_produtos_marcas.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS == obj.ID_CODIGO_EMPRESA_FABRICANTE_MARCAS)
                && (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS == obj.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS));

            if (jahExisteVinculoParaEmpresaProdutoMarca == null)
            {
                _contexto.empresas_produtos_marcas.Add(obj);
                _contexto.SaveChanges();
            }
        }
    }
}
