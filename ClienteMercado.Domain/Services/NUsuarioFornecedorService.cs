using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NUsuarioFornecedorService
    {
        DUsuarioFornecedorRepository dRepository = new DUsuarioFornecedorRepository();

        public USUARIO_FORNECEDOR ConsultarDadosUsuarioEmpresaFornecedor(int idEmpresa) => dRepository.ConsultarDadosUsuarioEmpresaFornecedor(idEmpresa);

        public USUARIO_FORNECEDOR ConsultarDadosUsuarioMasterEmpForn(int idUsuario) => dRepository.ConsultarDadosUsuarioMasterEmpForn(idUsuario);

        public USUARIO_FORNECEDOR ConsultarLoginUsuarioEmpresaFornecedora(USUARIO_FORNECEDOR obj) => dRepository.ConsultarLoginUsuarioEmpresaFornecedora(obj);

        public void GravarDadosEditadosDoUsuarioEmpresaFornecedor(USUARIO_FORNECEDOR obj) => dRepository.GravarDadosEditadosDoUsuarioEmpresaFornecedor(obj);

        public USUARIO_FORNECEDOR GravarNovoUsuarioAdmEmpresaFornecedor(USUARIO_FORNECEDOR obj) => dRepository.GravarNovoUsuarioAdmEmpresaFornecedor(obj);
    }
}
