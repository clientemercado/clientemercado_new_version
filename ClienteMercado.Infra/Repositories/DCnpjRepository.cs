using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCnpjRepository
    {
        public empresa_usuario ConsultarCnpj(empresa_usuario obj)
        {
            //Consulta CNPJ da Empresa
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                empresa_usuario cnpj =
                    _contexto.empresa_usuario.FirstOrDefault(
                        m => m.CNPJ_EMPRESA_USUARIO.Equals(obj.CNPJ_EMPRESA_USUARIO));

                return cnpj;
            }
        }

        public EMPRESA_FORNECEDOR ConsultarCnpjNew(EMPRESA_FORNECEDOR obj)
        {
            try
            {
                //Consulta CNPJ da Empresa
                using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
                {
                    EMPRESA_FORNECEDOR cnpj = _contexto.empresa_fornecedor.FirstOrDefault(m => m.cnpj_empresa_fornecedor == obj.cnpj_empresa_fornecedor);
                    return cnpj;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
