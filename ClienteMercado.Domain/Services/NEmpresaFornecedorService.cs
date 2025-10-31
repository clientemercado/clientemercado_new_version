using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresaFornecedorService
    {
        DEmpresaFornecedorRepository dRepository = new DEmpresaFornecedorRepository();

        public EMPRESA_FORNECEDOR ConsultarDadosEmpresaFornecedor(int idEmpFornecedor) => dRepository.ConsultarDadosEmpresaFornecedor(idEmpFornecedor);

        public void GravarDadosEditadosEmpresaFornecedor(EMPRESA_FORNECEDOR obj) => dRepository.GravarDadosEditadosEmpresaFornecedor(obj);

        public EMPRESA_FORNECEDOR GravarNovaEmpresaFornecedor(EMPRESA_FORNECEDOR obj) => dRepository.GravarNovaEmpresaFornecedor(obj);
    }
}
