using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NCardsUsuarioEmpresaService
    {
        //Gravando dados do cartão de crédito do usuário da empresa
        public cards_usuario_empresa GravarDadosCartaoUsuarioEmpresa(cards_usuario_empresa obj)
        {
            DCardsUsuarioEmpresaRepository dcardsusuarioempresa = new DCardsUsuarioEmpresaRepository();

            return dcardsusuarioempresa.GravarDadosCartaoUsuarioEmpresa(obj);
        }
    }
}
