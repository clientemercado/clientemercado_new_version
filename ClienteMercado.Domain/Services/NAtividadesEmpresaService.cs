using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NAtividadesEmpresaService
    {
        //Gravar Atividades particulares da Empresa
        public atividades_empresa GravarAtividadeProdutoServicoEmpresa(atividades_empresa obj)
        {
            DAtividadesEmpresaRepository datividadesempresa = new DAtividadesEmpresaRepository();

            return datividadesempresa.GravarAtividadeProdutoServicoEmpresa(obj);
        }
    }
}
