using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DCardsUsuarioProfissionalRepository
    {
        //Gravando dados do cartão de crédito do usuário profissional de serviços
        public cards_usuario_profissional GravarDadosCartaoUsuarioProfissional(cards_usuario_profissional obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cards_usuario_profissional cardsUsuarioProfissional = _contexto.cards_usuario_profissional.Add(obj);
                _contexto.SaveChanges();

                return cardsUsuarioProfissional;
            }
        }
    }
}
