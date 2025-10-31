using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresaFornecedorRepository : RepositoryBase<EMPRESA_FORNECEDOR>
    {
        public EMPRESA_FORNECEDOR ConsultarDadosEmpresaFornecedor(int idEmpFornecedor)
        {
            try
            {
                EMPRESA_FORNECEDOR dadosEmpresa = _contexto.empresa_fornecedor.FirstOrDefault(e => e.id_empresa_fornecedor == idEmpFornecedor);
                return dadosEmpresa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GravarDadosEditadosEmpresaFornecedor(EMPRESA_FORNECEDOR obj)
        {
            try
            {
                EMPRESA_FORNECEDOR dadosEmpresa = _contexto.empresa_fornecedor.FirstOrDefault(e => e.cnpj_empresa_fornecedor == obj.cnpj_empresa_fornecedor);
                if (dadosEmpresa != null)
                {
                    dadosEmpresa.cnpj_empresa_fornecedor = Regex.Replace(obj.cnpj_empresa_fornecedor, "[./-]", "");
                    dadosEmpresa.nome_fantasia_empresa_fornecedor = obj.nome_fantasia_empresa_fornecedor;
                    dadosEmpresa.endereco_empresa_fornecedor = obj.endereco_empresa_fornecedor;
                    dadosEmpresa.complemento_empresa_fornecedor = obj.complemento_empresa_fornecedor;
                    dadosEmpresa.cep_empresa_fornecedor = obj.cep_empresa_fornecedor;
                    dadosEmpresa.cidade_empresa_fornecedor = obj.cidade_empresa_fornecedor;
                    dadosEmpresa.uf_empresa_fornecedor = obj.uf_empresa_fornecedor;
                    dadosEmpresa.bairro_empresa_fornecedor = obj.bairro_empresa_fornecedor;
                    dadosEmpresa.ramo_atividade_empresa_fornecedor = obj.ramo_atividade_empresa_fornecedor;
                    _contexto.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public EMPRESA_FORNECEDOR GravarNovaEmpresaFornecedor(EMPRESA_FORNECEDOR obj)
        {
            try
            {
                EMPRESA_FORNECEDOR dadosNovaEmpresa = _contexto.empresa_fornecedor.Add(obj);
                _contexto.SaveChanges();

                return dadosNovaEmpresa;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
