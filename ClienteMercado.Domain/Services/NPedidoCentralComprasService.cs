using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NPedidoCentralComprasService
    {
        DPedidoCentralComprasRepository dPedidoCC = new DPedidoCentralComprasRepository();

        //VERIFICAR se a COTAÇÃO se já se converteu em PEDIDO
        public pedido_central_compras VerificaSeACotacaoPossuiPedido(int iCM)
        {
            return dPedidoCC.VerificaSeACotacaoPossuiPedido(iCM);
        }

        //GERAR o PEDIDO feito pelo USUÁRIO ADM da CENTRAL de COMPRAS (Independente do Pedido ser TOTAL ou PARCIAL)
        public pedido_central_compras GerarPedidoCC(pedido_central_compras obj)
        {
            return dPedidoCC.GerarPedidoCC(obj);
        }

        //VERIFICAR se o FORNECEDOR recebeu PEDIDO para ESTA COTAÇÃO
        public pedido_central_compras VerificarSeEstaCotacaoRecebeuPedido(int iCM, int iCCF)
        {
            return dPedidoCC.VerificarSeEstaCotacaoRecebeuPedido(iCM, iCCF);
        }

        //BUSCAR DADOS do PEDIDO
        public pedido_central_compras ConsultarDadosDoPedidoPeloCodigo(int idPedidoGeradoCC)
        {
            return dPedidoCC.ConsultarDadosDoPedidoPeloCodigo(idPedidoGeradoCC);
        }

        //ATUALIZAR o VALOR TOTAL REGISTRADO para o PEDIDO
        public pedido_central_compras AtualizarValorDoPedido(pedido_central_compras obj)
        {
            return dPedidoCC.AtualizarValorDoPedido(obj);
        }

        //VERIFICAR TODOS os PEDIDOS para ESTA COTAÇÃO
        public List<pedido_central_compras> BuscarTodosOsPedidosParaACotacao(int iCM)
        {
            return dPedidoCC.BuscarTodosOsPedidosParaACotacao(iCM);
        }

        //EXCLUIR o PEDIDO
        public bool ExcluirOPedido(string idPedido)
        {
            return dPedidoCC.ExcluirOPedido(idPedido);
        }

        //CONFIRMAR o ACEITE do PEDIDO
        public void SetarConfirmandoAceiteDoPedido(int iCM, int iCCF, int idPedido, int idTipoFrete, int idFormaPagto, string dataEntrega)
        {
            dPedidoCC.SetarConfirmandoAceiteDoPedido(iCM, iCCF, idPedido, idTipoFrete, idFormaPagto, dataEntrega);
        }

        //GERAR NOVO CODIGO de CONTROLE do PEDIDO
        public string GerarCodigoControleDoPedido(int cCC)
        {
            return dPedidoCC.GerarCodigoControleDoPedido(cCC);
        }

        //BAIXAR o PEDIDO
        public pedido_central_compras DarBaixaNoPedido(int idPedidoABaixar)
        {
            return dPedidoCC.DarBaixaNoPedido(idPedidoABaixar);
        }

        //VERIFICAR TODOS os PEDIDOS para BAIXA NESTA COTAÇÃO
        public List<pedido_central_compras> BuscarTodosOsPedidosParaBaixaNestaACotacao(int iCM)
        {
            return dPedidoCC.BuscarTodosOsPedidosParaBaixaNestaACotacao(iCM);
        }

        //VERIFICAR TODOS os PEDIDOS BAIXADOS NESTA COTAÇÃO
        public List<pedido_central_compras> BuscarTodosOsPedidosBaixadosParaEstaCotacao(int iCM)
        {
            return dPedidoCC.BuscarTodosOsPedidosBaixadosParaEstaCotacao(iCM);
        }
    }
}
