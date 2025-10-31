using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NPedidoUsuarioCotanteService
    {
        DPedidoUsuarioCotanteRepository dPedidoUsuarioCotante = new DPedidoUsuarioCotanteRepository();

        //GRAVAR PEDIDO feito pelo USUÁRIO COTANTE
        public int GravarPedidoUsuarioCotante(pedido_usuario_cotante obj)
        {
            return dPedidoUsuarioCotante.GravarPedidoUsuarioCotante(obj);
        }

        //VERIFICA se a COTAÇÃO em questão virou PEDIDO
        public string VerificarSeExistePedidoParaEstaCotacao(int idCotacaoMaster, int idCotacaoFilha)
        {
            return dPedidoUsuarioCotante.VerificarSeExistePedidoParaEstaCotacao(idCotacaoMaster, idCotacaoFilha);
        }
    }
}
