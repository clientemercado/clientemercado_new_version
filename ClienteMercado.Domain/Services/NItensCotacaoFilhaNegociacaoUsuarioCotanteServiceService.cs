using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoFilhaNegociacaoUsuarioCotanteService
    {
        DItensCotacaoFilhaNegociacaoUsuarioCotanteRepository ditenscotacaofilhanegociacaousuariocotante = new DItensCotacaoFilhaNegociacaoUsuarioCotanteRepository();

        //Gravar os Itens que fazem parte da COTAÇÃO FILHA, gerada pelo USUÁRIO COTANTE para o FORNECEDOR
        public itens_cotacao_filha_negociacao_usuario_cotante GravarItensDaCotacaoFilhaUsuarioCotante(itens_cotacao_filha_negociacao_usuario_cotante obj)
        {
            return ditenscotacaofilhanegociacaousuariocotante.GravarItensDaCotacaoFilhaUsuarioCotante(obj);
        }

        //Consultar os ITENS que fazem parte da COTAÇÃO FILHA
        public List<itens_cotacao_filha_negociacao_usuario_cotante> ConsultarItensDaCotacaoDoUsuarioCotante(int idCotacaoFilha)
        {
            return ditenscotacaofilhanegociacaousuariocotante.ConsultarItensDaCotacaoFilhaUsuarioCotante(idCotacaoFilha);
        }

        //Responder a COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
        public itens_cotacao_filha_negociacao_usuario_cotante GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioCotante(itens_cotacao_filha_negociacao_usuario_cotante obj,
            int idCotacaoFilha, int tipoGravacao)
        {
            return ditenscotacaofilhanegociacaousuariocotante.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioCotante(obj, idCotacaoFilha, tipoGravacao);
        }

        //BUSCAR DETERMINADO ITEM em todas as COTAÇÕES FILHA, para ANÁLISE por PRODUTOS
        public itens_cotacao_filha_negociacao_usuario_cotante BuscarDeterminadoItemDeUmaCotacaoEnviadaPeloUsuarioCotante(int idCotacaoFilha, int idProduto)
        {
            return ditenscotacaofilhanegociacaousuariocotante.BuscarDeterminadoItemDeUmaCotacaoEnviadaPeloUsuarioCotante(idCotacaoFilha, idProduto);
        }

        //BUSCA e CALCULA o VALOR FINAL
        public List<ListaPorItemCotadoJaCalculadoUsuarioCotanteViewModel> BuscarValorTotalPorProdutoDestaCotacao(string listaIdsCotacoesEnviadas, int idCodigoProduto)
        {
            return ditenscotacaofilhanegociacaousuariocotante.BuscarValorTotalPorProdutoDestaCotacao(listaIdsCotacoesEnviadas, idCodigoProduto);
        }

        //VERIFICAR QUANTIDADE de ITENS COTADOS
        public int ConsultarQuantidadeDeItensRespondidosPeloUsuarioCotante(int idCotacaoFilha)
        {
            return ditenscotacaofilhanegociacaousuariocotante.ConsultarQuantidadeDeItensRespondidosPeloUsuarioCotante(idCotacaoFilha);
        }

        ////BUSCA e CALCULA o VALOR TOTAL por COTACAO por EMPRESA
        //public List<ListaPorCotacaoJaCalculadoOTotalRespondidoUsuarioCotanteViewModel> BuscarValorTotalRespondidoPorCotacaoEnviada(string listaIdsCotacoesEnviadas)
        //{
        //    return ditenscotacaofilhanegociacaousuariocotante.BuscarValorTotalRespondidoPorCotacaoEnviada(listaIdsCotacoesEnviadas);
        //}
    }
}
