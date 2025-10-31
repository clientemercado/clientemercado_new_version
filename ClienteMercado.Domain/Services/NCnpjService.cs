using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;

namespace ClienteMercado.Domain.Services
{
    public class NCnpjService
    {
        DCnpjRepository dcnpj = new DCnpjRepository();

        //Consulta CNPJ da Empresa
        public empresa_usuario ConsultarCnpj(empresa_usuario obj)
        {
            return dcnpj.ConsultarCnpj(obj);
        }

        public EMPRESA_FORNECEDOR ConsultarCnpjNew(EMPRESA_FORNECEDOR obj) => dcnpj.ConsultarCnpjNew(obj);
    }
}
