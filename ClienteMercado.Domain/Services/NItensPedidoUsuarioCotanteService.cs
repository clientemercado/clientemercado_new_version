using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NItensPedidoUsuarioCotanteService
    {
        DItensPedidoUsuarioCotanteRepository dItensPedidoUsuarioCotante =
            new DItensPedidoUsuarioCotanteRepository();

        //GRAVAR ITEM do PEDIDO vinculado ao PEDIDO gerado
        public int GravarItemDoPedido(itens_pedido_usuario_cotante obj)
        {
            return dItensPedidoUsuarioCotante.GravarItemDoPedido(obj);
        }
    }
}
