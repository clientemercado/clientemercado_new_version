using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DUsuarioEmpresaRepository : RepositoryBase<usuario_empresa>
    {
        //Consultar Usuários ligados à Empresa
        public List<usuario_empresa> ConsultarUsuariosLigadosAEmpresa(usuario_empresa obj)
        {
            List<usuario_empresa> listaUsuariosFornecedores = _contexto.usuario_empresa.Where(m => (m.ID_CODIGO_EMPRESA.Equals(obj.ID_CODIGO_EMPRESA))).ToList();

            return listaUsuariosFornecedores;
        }

        //Consultar dados dos Vendedores que receberão aviso de cotação
        public List<usuario_empresa> ConsultarDadosDosUsuariosVendedoresQueReceberaoAvisoDeCotacao(string[] listaIDsFornecedores)
        {
            List<usuario_empresa> dadosUsuariosVendedores = new List<usuario_empresa>();

            //Consulta o e-mail de cada ID da lista
            for (int i = 0; i < listaIDsFornecedores.Length; i++)
            {
                int idFornecedor = Convert.ToInt32(listaIDsFornecedores[i]);

                usuario_empresa buscaDadosDoUsuarioVendedor = _contexto.usuario_empresa.FirstOrDefault(m => m.ID_CODIGO_USUARIO.Equals(idFornecedor));

                if (buscaDadosDoUsuarioVendedor != null)
                {
                    dadosUsuariosVendedores.Add(buscaDadosDoUsuarioVendedor);
                }
            }

            return dadosUsuariosVendedores;
        }

        //CONSULTAR DADOS do USUÁRIO da EMPRESA FORNECEDORA pelo ID da EMPRESA
        public usuario_empresa ConsultarDadosDoUsuarioDaEmpresaFornecedoraPeloIdDaEmpresa(int idEmpresa)
        {
            usuario_empresa dadosDoUsuario = _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA == idEmpresa));
            return dadosDoUsuario;
        }

        //GRAVAR DADOS ATUALIZADOS do USUÁRIO
        public usuario_empresa AtualizarDadosCadastrais(usuario_empresa obj)
        {
            usuario_empresa dadosDoUsuarioASerAtualizado =
                _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_USUARIO == obj.ID_CODIGO_USUARIO));

            if (dadosDoUsuarioASerAtualizado != null)
            {
                dadosDoUsuarioASerAtualizado.NOME_USUARIO = obj.NOME_USUARIO;

                if ((obj.NICK_NAME_USUARIO != null) && (obj.NICK_NAME_USUARIO != ""))
                {
                    dadosDoUsuarioASerAtualizado.NICK_NAME_USUARIO = obj.NICK_NAME_USUARIO;
                }

                if ((obj.CPF_USUARIO_EMPRESA != null) && (obj.CPF_USUARIO_EMPRESA != ""))
                {
                    dadosDoUsuarioASerAtualizado.CPF_USUARIO_EMPRESA = obj.CPF_USUARIO_EMPRESA;
                }

                if (obj.PAIS_USUARIO_EMPRESA > 0)
                {
                    dadosDoUsuarioASerAtualizado.PAIS_USUARIO_EMPRESA = obj.PAIS_USUARIO_EMPRESA;
                }

                dadosDoUsuarioASerAtualizado.ID_CODIGO_ENDERECO_EMPRESA_USUARIO = obj.ID_CODIGO_ENDERECO_EMPRESA_USUARIO;
                dadosDoUsuarioASerAtualizado.TELEFONE1_USUARIO_EMPRESA = obj.TELEFONE1_USUARIO_EMPRESA;
                dadosDoUsuarioASerAtualizado.TELEFONE2_USUARIO_EMPRESA = obj.TELEFONE2_USUARIO_EMPRESA;
                dadosDoUsuarioASerAtualizado.DATA_ULTIMA_ATUALIZACAO_USUARIO = obj.DATA_ULTIMA_ATUALIZACAO_USUARIO;

                _contexto.SaveChanges();
            }

            return dadosDoUsuarioASerAtualizado;
        }

        //Consultar dados de um Usuário da Empresa
        public usuario_empresa ConsultarDadosDoUsuarioDaEmpresa(int idUsuario)
        {
            //usuario_empresa dadosDoUsuario = _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_EMPRESA.Equals(idUsuario)));
            usuario_empresa dadosDoUsuario = _contexto.usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_USUARIO.Equals(idUsuario)));

            return dadosDoUsuario;
        }
    }
}
