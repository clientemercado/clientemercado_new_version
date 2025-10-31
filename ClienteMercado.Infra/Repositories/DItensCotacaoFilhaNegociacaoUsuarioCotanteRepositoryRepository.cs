using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoFilhaNegociacaoUsuarioCotanteRepository
    {
        //Gravar os Itens que fazem parte da COTAÇÃO FILHA, gerada pelo USUÁRIO COTANTE para o FORNECEDOR
        public itens_cotacao_filha_negociacao_usuario_cotante GravarItensDaCotacaoFilhaUsuarioCotante(itens_cotacao_filha_negociacao_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_filha_negociacao_usuario_cotante gravarItensCotacaoUsuarioCotante =
                    _contexto.itens_cotacao_filha_negociacao_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return gravarItensCotacaoUsuarioCotante;
            }
        }

        //Consultar os ITENS que fazem parte da COTAÇÃO FILHA
        public List<itens_cotacao_filha_negociacao_usuario_cotante> ConsultarItensDaCotacaoFilhaUsuarioCotante(int idCotacaoFilha)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<itens_cotacao_filha_negociacao_usuario_cotante> consultarItensDaCotacao =
                    _contexto.itens_cotacao_filha_negociacao_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(idCotacaoFilha))).ToList();

                return consultarItensDaCotacao;
            }
        }

        //Responder a COTAÇÃO FILHA, enviada pelo USUÁRIO COTANTE
        public itens_cotacao_filha_negociacao_usuario_cotante GravarValorDosProdutosCotadosEmRespostaACotacaoDoUsuarioCotante(itens_cotacao_filha_negociacao_usuario_cotante obj,
            int idCotacaoFilha, int tipoGravacao)
        {
            // tipoGravacao: 0 - RESPONDENDO A COTAÇÃO
            // tipoGravacao: 1 - EDITANDO A RESPOSTA DA COTAÇÃO

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_filha_negociacao_usuario_cotante respostaDeValorAoProdutoCotado =
                    _contexto.itens_cotacao_filha_negociacao_usuario_cotante.FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE == idCotacaoFilha)
                    && (m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE == obj.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE));

                if (respostaDeValorAoProdutoCotado != null)
                {
                    if (tipoGravacao == 0)
                    {
                        //RESPONDENDO A COTAÇÃO
                        respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_COTANTE = obj.PRECO_ITENS_COTACAO_USUARIO_COTANTE;
                        respostaDeValorAoProdutoCotado.PRODUTO_COTADO_USUARIO_COTANTE = true;   // Marca como ITEM COTADO

                        _contexto.SaveChanges();
                    }
                    else if (tipoGravacao == 1)
                    {
                        if (respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_COTANTE != obj.PRECO_ITENS_COTACAO_USUARIO_COTANTE)
                        {
                            //EDITANDO A RESPOSTA DA COTAÇÃO
                            respostaDeValorAoProdutoCotado.PRECO_ITENS_COTACAO_USUARIO_COTANTE = obj.PRECO_ITENS_COTACAO_USUARIO_COTANTE;
                            respostaDeValorAoProdutoCotado.ITEM_COTACAO_FILHA_EDITADO = true;   // Marca como ITEM EDITADO, pra informação do COTANTE

                            _contexto.SaveChanges();
                        }
                    }
                }

                return respostaDeValorAoProdutoCotado;
            }
        }

        //VERIFICAR QUANTIDADE de ITENS COTADOS
        public int ConsultarQuantidadeDeItensRespondidosPeloUsuarioCotante(int idCotacaoFilha)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<itens_cotacao_filha_negociacao_usuario_cotante> consultarItensDaCotacao =
                    _contexto.itens_cotacao_filha_negociacao_usuario_cotante
                    .Where(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE.Equals(idCotacaoFilha)) && (m.PRODUTO_COTADO_USUARIO_COTANTE == true)).ToList();

                return consultarItensDaCotacao.Count;
            }
        }

        //BUSCA e CALCULA o VALOR FINAL
        public List<ListaPorItemCotadoJaCalculadoUsuarioCotanteViewModel> BuscarValorTotalPorProdutoDestaCotacao(string listaIdsCotacoesEnviadas, int idCodigoProduto)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                var query = " SELECT ICM.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE AS ID_DACOTACAO, ICM.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE AS ID_PRODUTO_COTADO, " +
                            " SUM(ICM.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE * ICM.PRECO_ITENS_COTACAO_USUARIO_COTANTE) AS PRECO_FINAL_CALCULADO_DO_PRODUTO, " +
                            " ICM.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE, ICM.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE, ICM.PRECO_ITENS_COTACAO_USUARIO_COTANTE, E.ID_CODIGO_EMPRESA, " +
                            " UE.ID_CODIGO_USUARIO, CF.ID_TIPO_FRETE, CF.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE, CF.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE, " +
                            " CF.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE, CF.OBSERVACAO_COTACAO_USUARIO_COTANTE " +
                            " FROM itens_cotacao_filha_negociacao_usuario_cotante ICM " +
                            " LEFT JOIN cotacao_filha_usuario_cotante CF ON(CF.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE = ICM.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE) " +
                            " LEFT JOIN EMPRESA_USUARIO E ON(E.ID_CODIGO_EMPRESA = CF.ID_CODIGO_EMPRESA) " +
                            " LEFT JOIN USUARIO_EMPRESA UE ON(UE.ID_CODIGO_EMPRESA = E.ID_CODIGO_EMPRESA) " +
                            " WHERE ICM.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE IN(" + listaIdsCotacoesEnviadas + ") AND ICM.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE = " + idCodigoProduto +
                            " GROUP BY ICM.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE, ICM.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE, ICM.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE, " +
                            " ICM.PRECO_ITENS_COTACAO_USUARIO_COTANTE, " +
                            " ICM.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE, ICM.QUANTIDADE_ITENS_COTACAO_USUARIO_COTANTE, ICM.PRECO_ITENS_COTACAO_USUARIO_COTANTE, E.ID_CODIGO_EMPRESA, " +
                            " UE.ID_CODIGO_USUARIO, CF.ID_TIPO_FRETE, CF.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE, CF.DATA_RESPOSTA_COTACAO_FILHA_USUARIO_COTANTE, " +
                            " CF.FORMA_PAGAMENTO_COTACAO_FILHA_USUARIO_COTANTE, CF.OBSERVACAO_COTACAO_USUARIO_COTANTE " +
                            " ORDER BY PRECO_FINAL_CALCULADO_DO_PRODUTO ASC";

                var produtosCotadosJaCalculados = _contexto.Database.SqlQuery<ListaPorItemCotadoJaCalculadoUsuarioCotanteViewModel>(query).ToList();

                return produtosCotadosJaCalculados;
            }
        }

        //BUSCAR DETERMINADO ITEM em todas as COTAÇÕES FILHA, para ANÁLISE por PRODUTOS
        public itens_cotacao_filha_negociacao_usuario_cotante BuscarDeterminadoItemDeUmaCotacaoEnviadaPeloUsuarioCotante(int idCotacaoFilha, int idProduto)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                itens_cotacao_filha_negociacao_usuario_cotante itemDaCotacaoParaAnalise =
                    _contexto.itens_cotacao_filha_negociacao_usuario_cotante
                    .FirstOrDefault(m => (m.ID_CODIGO_COTACAO_FILHA_USUARIO_COTANTE == idCotacaoFilha) && (m.ID_CODIGO_ITENS_COTACAO_USUARIO_COTANTE == idProduto));

                return itemDaCotacaoParaAnalise;
            }
        }
    }
}
