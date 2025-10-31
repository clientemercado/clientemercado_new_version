using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresasProdutosMarcasService
    {
        DEmpresasProdutosMarcasRepository dRepository = new DEmpresasProdutosMarcasRepository();

        //GRAVAR VÍNCULO da MARCA ou FABRICANTE ao PRODUTO
        public void GravarVinculoDaMarcaOuFabricanteAoProduto(empresas_produtos_marcas obj)
        {
            dRepository.GravarVinculoDaMarcaOuFabricanteAoProduto(obj);
        }
    }
}
