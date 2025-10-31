using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NCardsUsuarioProfissionalService
    {
        //Gravando dados do cartão de crédito do usuário profissional de serviços
        public cards_usuario_profissional GravarDadosCartaoUsuarioProfissional(cards_usuario_profissional obj)
        {
            DCardsUsuarioProfissionalRepository dcardsusuarioprofissional = new DCardsUsuarioProfissionalRepository();

            return dcardsusuarioprofissional.GravarDadosCartaoUsuarioProfissional(obj);
        }
    }
}
