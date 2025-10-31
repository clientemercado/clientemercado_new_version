using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NUsuarioEmpresaService
    {
        DUsuarioEmpresaRepository dusuarioempresa = new DUsuarioEmpresaRepository();

        //Consultar Usuários ligados à Empresa
        public List<usuario_empresa> ConsultarUsuariosLigadosAEmpresa(usuario_empresa obj)
        {
            return dusuarioempresa.ConsultarUsuariosLigadosAEmpresa(obj);
        }

        //Consultar dados dos Vendedores que receberão aviso de cotação
        public List<usuario_empresa> ConsultarDadosDosUsuariosVendedoresQueReceberaoAvisoDeCotacao(string[] listaIDsFornecedores)
        {
            return dusuarioempresa.ConsultarDadosDosUsuariosVendedoresQueReceberaoAvisoDeCotacao(listaIDsFornecedores);
        }

        //Consultar dados de um Usuário da Empresa
        public usuario_empresa ConsultarDadosDoUsuarioDaEmpresa(int idUsuario)
        {
            return dusuarioempresa.ConsultarDadosDoUsuarioDaEmpresa(idUsuario);
        }

        //GRAVAR DADOS ATUALIZADOS do USUÁRIO
        public usuario_empresa AtualizarDadosCadastrais(usuario_empresa obj)
        {
            return dusuarioempresa.AtualizarDadosCadastrais(obj);
        }

        //CONSULTAR DADOS do USUÁRIO da EMPRESA FORNECEDORA pelo ID da EMPRESA
        public usuario_empresa ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(int idEmpresa)
        {
            return dusuarioempresa.ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(idEmpresa);
        }
    }
}
