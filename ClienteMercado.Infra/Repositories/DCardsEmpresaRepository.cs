using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DCardsEmpresaRepository
    {
        //Gravando dados do cartão de crédito da empresa
        public cards_empresa GravarDadosCartaoEmpresa(cards_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cards_empresa cardsEmpresa = _contexto.cards_empresa.Add(obj);
                _contexto.SaveChanges();

                return cardsEmpresa;
            }
        }
    }
}
