using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DAtividadesEmpresaRepository
    {
        //Gravar atividades particulares da Empresa
        public atividades_empresa GravarAtividadeProdutoServicoEmpresa(atividades_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                atividades_empresa atividadesEmpresa = _contexto.atividades_empresa.Add(obj);
                _contexto.SaveChanges();

                return atividadesEmpresa;
            }
        }
    }
}
