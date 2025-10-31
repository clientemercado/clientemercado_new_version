using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NCentralDeComprasService
    {
        DCentralDeComprasRepository dRepository = new DCentralDeComprasRepository();

        //CRIAR/GRAVAR CENTRAL de COMPRAS
        public DadosDaCentralComprasViewModel CriarCentralDeComprasNoSistema(central_de_compras obj)
        {
            return dRepository.CriarCentralDeComprasNoSistema(obj);
        }

        //CARREGAR os DADOS da CENTRAL de COMPRAS
        public central_de_compras ConsultarDadosDaCentralDeCompras(int codCentralCompras, int codEmpresaAdm)
        {
            return dRepository.ConsultarDadosDaCentralDeCompras(codCentralCompras, codEmpresaAdm);
        }

        //EXCLUIR a CENTRAL de COMPRAS
        public void ExcluirACentralDeCompras(int codCentralCompras, int codEmpresaAdm)
        {
            dRepository.ExcluirACentralDeCompras(codCentralCompras, codEmpresaAdm);
        }

        //CONSULTAR DADOS sobre a CENTRAL de COMPRAS
        public central_de_compras ConsultarDadosGeraisSobreACentralDeComprasPeloID(int codCentralCompras)
        {
            return dRepository.ConsultarDadosGeraisSobreACentralDeComprasPeloID(codCentralCompras);
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS
        public List<ListaCentraisComprasViewModel> CarregarListaAutoCompleteCentraisDeCompras(string term)
        {
            return dRepository.CarregarListaAutoCompleteCentraisDeCompras(term);
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS do SISTEMA
        public List<ListaCentraisComprasViewModel> CarregarListaAutoCompleteCentraisDeComprasDoSistema(string term)
        {
            return dRepository.CarregarListaAutoCompleteCentraisDeComprasDoSistema(term);
        }

        //CARREGA LISTA de CENTRAIS de COMPRAS do SISTEMA
        public List<ListaCentraisComprasViewModel> BuscarListaDeCentraisDeComprasDoSistema()
        {
            return dRepository.BuscarListaDeCentraisDeComprasDoSistema();
        }

        //CARREGA DADOS da CENTRAL de COMPRAS
        public central_de_compras CarregarDadosDaCentralDeCompras(int cCC)
        {
            return dRepository.CarregarDadosDaCentralDeCompras(cCC);
        }
    }
}
