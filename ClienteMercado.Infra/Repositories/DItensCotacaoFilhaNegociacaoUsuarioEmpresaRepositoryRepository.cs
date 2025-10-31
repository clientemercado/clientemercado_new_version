using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoFilhaNegociacaoUsuarioEmpresaRepository
    {
        //Gravar os Itens que fazem parte da COTAÇÃO FILHA, gerada pelo USUÁRIO EMPRESA para o FORNECEDOR
        public itens_cotacao_filha_negociacao_usuario_empresa GravarItensDaCotacaoFilhaUsuarioEmpresa(itens_cotacao_filha_negociacao_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_filha_negociacao_usuario_empresa gravarItensCotacaoUsuarioEmpresa =
                    _contexto.itens_cotacao_filha_negociacao_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return gravarItensCotacaoUsuarioEmpresa;
            }
        }

        //Consultar os ITENS que fazem parte da COTAÇÃO FILHA
        public List<itens_cotacao_filha_negociacao_usuario_empresa> ConsultarItensDaCotacaoFilhaUsuarioEmpresa(int idCotacaoFilha)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<itens_cotacao_filha_negociacao_usuario_empresa> consultarItensDaCotacao =
                    _contexto.itens_cotacao_filha_negociacao_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(idCotacaoFilha))).ToList();

                return consultarItensDaCotacao;
            }
        }

        //Responder a COTAÇÃO FILHA, enviada pelo USUÁRIO EMPRESA
        public itens_cotacao_filha_negociacao_usuario_empresa GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioEmpresa(itens_cotacao_filha_negociacao_usuario_empresa obj,
            int idCotacaoFilha, int tipoGravacao)
        {
            // tipoGravacao: 0 - RESPONDENDO A COTAÇÃO
            // tipoGravacao: 1 - EDITANDO A RESPOSTA DA COTAÇÃO

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_filha_negociacao_usuario_empresa respostaDeValorAoProdutoCotado =
                    _contexto.itens_cotacao_filha_negociacao_usuario_empresa.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA == idCotacaoFilha)
                    && (m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA == obj.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA));

                if (respostaDeValorAoProdutoCotado != null)
                {
                    if (tipoGravacao == 0)
                    {
                        //RESPONDENDO A COTAÇÃO
                        respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = obj.PRECO_ITENS_COTACAO_USUARIO_EMPRESA;
                        respostaDeValorAoProdutoCotado.PRODUTO_COTADO_USUARIO_EMPRESA = true;   // Marca como ITEM COTADO

                        _contexto.SaveChanges();
                    }
                    else if (tipoGravacao == 1)
                    {
                        if (respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_EMPRESA != obj.PRECO_ITENS_COTACAO_USUARIO_EMPRESA)
                        {
                            //EDITANDO A RESPOSTA DA COTAÇÃO
                            respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_EMPRESA = obj.PRECO_ITENS_COTACAO_USUARIO_EMPRESA;
                            respostaDeValorAoProdutoCotado.ITEM_COTACAO_FILHA_EDITADO = true;   // Marca como ITEM EDITADO, pra informação do COTANTE

                            _contexto.SaveChanges();
                        }
                    }
                }

                return respostaDeValorAoProdutoCotado;
            }
        }

        //BUSCA e CALCULA o VALOR FINAL
        public List<ListaPorItemCotadoJaCalculadoUsuarioEmpresaCotanteViewModel> BuscarValorTotalPorProdutoDestaCotacao(string listaIdsCotacoesEnviadas, int idCodigoProduto)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                var query = "SELECT ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA AS ID_DACOTACAO, " +
                            "SUM(QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA * PRECO_ITENS_COTACAO_USUARIO_EMPRESA) AS PRECO_FINAL_CALCULADO_DO_PRODUTO, " +
                            "ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA, QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA, PRECO_ITENS_COTACAO_USUARIO_EMPRESA " +
                            "FROM itens_cotacao_filha_negociacao_usuario_empresa " +
                            "WHERE ID_CODIGO_COTACAO_FILHA_USUARIO_EMPRESA IN(" + listaIdsCotacoesEnviadas + ") AND ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA = " + idCodigoProduto +
                            "GROUP BY ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA, QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA, PRECO_ITENS_COTACAO_USUARIO_EMPRESA, " +
                            "ID_CODIGO_ITENS_COTACAO_USUARIO_EMPRESA, QUANTIDADE_ITENS_COTACAO_USUARIO_EMPRESA, PRECO_ITENS_COTACAO_USUARIO_EMPRESA " +
                            "ORDER BY PRECO_FINAL_CALCULADO_DO_PRODUTO ASC";

                var produtosCotadosJaCalculados = _contexto.Database.SqlQuery<ListaPorItemCotadoJaCalculadoUsuarioEmpresaCotanteViewModel>(query).ToList();

                return produtosCotadosJaCalculados;
            }
        }

        public List<ListaItensCotadosRespondidosPeloFornecedorViewModel> ConsultarItensDaCotacaoDoUsuarioEmpresa(int idCotacaoFilha)
        {
            throw new NotImplementedException();
        }
    }
}
