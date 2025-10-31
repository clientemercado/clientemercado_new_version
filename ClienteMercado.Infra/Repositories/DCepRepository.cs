using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DCepRepository
    {
        //Consultar CEP baseado no cep digitado pelo usuário
        public enderecos_empresa_usuario ConsultarCep(enderecos_empresa_usuario obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            enderecos_empresa_usuario cep =
                _contexto.enderecos_empresa_usuario.FirstOrDefault(
                    m => m.CEP_ENDERECO_EMPRESA_USUARIO.Equals(obj.CEP_ENDERECO_EMPRESA_USUARIO));

            return cep;
        }

        //Consultar CEP e afins, baseado no id do cep informado pelo sistema
        public enderecos_empresa_usuario ConsultarCepPorId(enderecos_empresa_usuario obj)
        {
            cliente_mercadoContext _contexto = new cliente_mercadoContext();

            enderecos_empresa_usuario cep = _contexto.enderecos_empresa_usuario.FirstOrDefault(m => m.ID_CODIGO_ENDERECO_EMPRESA_USUARIO.Equals(obj.ID_CODIGO_ENDERECO_EMPRESA_USUARIO));

            return cep;
        }
    }
}
