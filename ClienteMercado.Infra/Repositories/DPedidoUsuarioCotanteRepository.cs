using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DPedidoUsuarioCotanteRepository
    {
        //GRAVAR PEDIDO feito pelo USUÁRIO COTANTE
        public int GravarPedidoUsuarioCotante(pedido_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                pedido_usuario_cotante gravarPedidoUsuarioCotante =
                    _contexto.pedido_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return gravarPedidoUsuarioCotante.ID_CODIGO_PEDIDO_USUARIO_COTANTE;
            }
        }

        //VERIFICA se a COTAÇÃO em questão virou PEDIDO
        public string VerificarSeExistePedidoParaEstaCotacao(int idCotacaoMaster, int idCotacaoFilha)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                var temPedido = "nao";
                pedido_usuario_cotante verificaSeExistePedido =
                    _contexto.pedido_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_MASTER_USUARIO_COTANTE == idCotacaoMaster)
                    && (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE == idCotacaoFilha));

                if (verificaSeExistePedido != null)
                {
                    temPedido = "sim";
                }

                return temPedido;
            }
        }
    }
}
