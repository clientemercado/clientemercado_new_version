using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresasFabricantesMarcasService
    {
        DEmpresasFabricantesMarcasRepository dRepository = new DEmpresasFabricantesMarcasRepository();

        //Carrega a Lista de Fabricantes e Narcas registrados no bd, conforme o termo informado
        public List<ListaDeEmpresasFabricantesEMarcasViewModel> ListaDeFabricantesEMarcas(string term)
        {
            return dRepository.ListaDeFabricantesEMarcas(term);
        }

        //Gravar nova Empresa Fabricante ou Marcas
        public empresas_fabricantes_marcas GravarNovaEmpresaFabricanteEMarcas(empresas_fabricantes_marcas obj)
        {
            return dRepository.GravarNovaEmpresaFabricanteEMarcas(obj);
        }

        //Consultar Empresas Fabricantes ou Marcas
        public empresas_fabricantes_marcas ConsultarEmpresaFabricanteOuMarca(int idFabricanteOuMarca)
        {
            return dRepository.ConsultarEmpresaFabricanteOuMarca(idFabricanteOuMarca);
        }

        //TRAZ MARCAS/FABRICANTES VINCULADAS AO PRODUTO
        public List<ListaDeEmpresasFabricantesEMarcasViewModel> ListaDeFabricantesEMarcasVinculadasAoProduto(string term, int codProduto)
        {
            return dRepository.ListaDeFabricantesEMarcasVinculadasAoProduto(term, codProduto);
        }

        //BUSCAR MARCA
        public string ConsultarDescricaoDaEmpresaFabricanteOuMarca(int idFabricanteMarca)
        {
            return dRepository.ConsultarDescricaoDaEmpresaFabricanteOuMarca(idFabricanteMarca);
        }
    }
}
