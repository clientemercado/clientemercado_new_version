using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoFilhaNegociacaoUsuarioEmpresaService
    {
        DItensCotacaoFilhaNegociacaoUsuarioEmpresaRepository ditenscotacaofilhanegociacaousuarioempresa =
            new DItensCotacaoFilhaNegociacaoUsuarioEmpresaRepository();

        //Gravar os Itens que fazem parte da COTAÇÃO FILHA, gerada pelo USUÁRIO EMPRESA para o FORNECEDOR
        public itens_cotacao_filha_negociacao_usuario_empresa GravarItensDaCotacaoFilhaUsuarioEmpresa(itens_cotacao_filha_negociacao_usuario_empresa obj)
        {
            return ditenscotacaofilhanegociacaousuarioempresa.GravarItensDaCotacaoFilhaUsuarioEmpresa(obj);
        }

        //Consultar os ITENS que fazem parte da COTAÇÃO FILHA
        public List<itens_cotacao_filha_negociacao_usuario_empresa> ConsultarItensDaCotacaoDoUsuarioEmpresa(int idCotacaoFilha)
        {
            return ditenscotacaofilhanegociacaousuarioempresa.ConsultarItensDaCotacaoFilhaUsuarioEmpresa(idCotacaoFilha);
        }

        //Responder a COTAÇÃO FILHA, enviada pelo USUÁRIO EMPRESA
        public itens_cotacao_filha_negociacao_usuario_empresa GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioEmpresa(itens_cotacao_filha_negociacao_usuario_empresa obj,
            int idCotacaoFilha, int tipoGravacao)
        {
            return ditenscotacaofilhanegociacaousuarioempresa.GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioEmpresa(obj, idCotacaoFilha, tipoGravacao);
        }

        //Consultar itens da COTAÇÃO FILHA respondidos
        public List<ListaItensCotadosRespondidosPeloFornecedorViewModel> ConsultarItensDaCotacaoDoUsuarioEmpresaRespondidosPeloFornecedor(int idCotacaoFilha)
        {
            return ditenscotacaofilhanegociacaousuarioempresa.ConsultarItensDaCotacaoDoUsuarioEmpresa(idCotacaoFilha);
        }

        //BUSCA e CALCULA o VALOR FINAL
        public List<ListaPorItemCotadoJaCalculadoUsuarioEmpresaCotanteViewModel> BuscarValorTotalPorProdutoDestaCotacao(string listaIdsCotacoesEnviadas, int idCodigoProduto)
        {
            return ditenscotacaofilhanegociacaousuarioempresa.BuscarValorTotalPorProdutoDestaCotacao(listaIdsCotacoesEnviadas, idCodigoProduto);
        }
    }
}
