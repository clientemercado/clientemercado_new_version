using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NCardsEmpresaService
    {
        //Gravando dados do cartão de crédito da empresa
        public cards_empresa GravarDadosCartaoEmpresa(cards_empresa obj)
        {
            DCardsEmpresaRepository dcardsempresa = new DCardsEmpresaRepository();

            return dcardsempresa.GravarDadosCartaoEmpresa(obj);
        }
    }
}
