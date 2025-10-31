using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensPedidoUsuarioCotanteRepository
    {
        //GRAVAR ITEM do PEDIDO vinculado ao PEDIDO gerado
        public int GravarItemDoPedido(itens_pedido_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_pedido_usuario_cotante gravarItenPedido =
                    _contexto.itens_pedido_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return gravarItenPedido.ID_CODIGO_ITENS_PEDIDO_USUARIO_COTANTE;
            }
        }
    }
}
