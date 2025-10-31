using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DCardsUsuarioEmpresaRepository
    {
        //Gravando dados do cartão de crédito do usuário da empresa
        public cards_usuario_empresa GravarDadosCartaoUsuarioEmpresa(cards_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                cards_usuario_empresa cardsUsuarioEmpresa = _contexto.cards_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return cardsUsuarioEmpresa;
            }
        }
    }
}
