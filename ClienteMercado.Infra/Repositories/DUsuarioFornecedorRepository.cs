using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClienteMercado.Infra.Repositories
{
    public class DUsuarioFornecedorRepository : RepositoryBase<USUARIO_FORNECEDOR>

    {
        public USUARIO_FORNECEDOR ConsultarDadosUsuarioEmpresaFornecedor(int idEmpresa)
        {
            try
            {
                USUARIO_FORNECEDOR dadosUsu = _contexto.usuario_fornecedor.FirstOrDefault(u => u.id_empresa_fornecedor == idEmpresa);
                return dadosUsu;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public USUARIO_FORNECEDOR GravarNovoUsuarioAdmEmpresaFornecedor(USUARIO_FORNECEDOR obj)
        {
            try
            {
                USUARIO_FORNECEDOR dadosUsuario = _contexto.usuario_fornecedor.Add(obj);
                _contexto.SaveChanges();

                return dadosUsuario;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GravarDadosEditadosDoUsuarioEmpresaFornecedor(USUARIO_FORNECEDOR obj)
        {
            try
            {
                USUARIO_FORNECEDOR dadosUsuario = _contexto.usuario_fornecedor.FirstOrDefault(u => u.cpf_usuario_fornecedor == obj.cpf_usuario_fornecedor);
                if (dadosUsuario != null)
                {
                    dadosUsuario.nome_usuario_fornecedor = obj.nome_usuario_fornecedor;
                    dadosUsuario.eh_master_usuario_fornecedor = true;
                    dadosUsuario.usuario_ativo_usuario_fornecedor = true;
                    dadosUsuario.login_usuario_empresa_fornecedor = obj.login_usuario_empresa_fornecedor;
                    dadosUsuario.email_usuario_empresa_fornecedor = obj.email_usuario_empresa_fornecedor;
                    _contexto.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public USUARIO_FORNECEDOR ConsultarLoginUsuarioEmpresaFornecedora(USUARIO_FORNECEDOR obj)
        {
            try
            {
                USUARIO_FORNECEDOR loginUsuarioEmpresaFornecedor =
                    _contexto.usuario_fornecedor.FirstOrDefault(
                        m =>
                            (m.login_usuario_empresa_fornecedor.Equals(obj.login_usuario_empresa_fornecedor) || 
                            m.email_usuario_empresa_fornecedor.Equals(obj.email_usuario_empresa_fornecedor)) && 
                            m.passw_usuario_empresa_fornecedor.Equals(obj.passw_usuario_empresa_fornecedor));

                return loginUsuarioEmpresaFornecedor;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public USUARIO_FORNECEDOR ConsultarDadosUsuarioMasterEmpForn(int idUsuario)
        {
            try
            {
                USUARIO_FORNECEDOR dadosUsu = _contexto.usuario_fornecedor.FirstOrDefault(u => u.id_usuario_fornecedor == idUsuario);
                return dadosUsu;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
