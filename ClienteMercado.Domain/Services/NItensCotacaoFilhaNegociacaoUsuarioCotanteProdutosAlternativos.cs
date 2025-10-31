using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosService
    {
        DItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosRepository ditenscotacaofilhausuariocotanteprodutosalternativos =
            new DItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosRepository();

        //Gravar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante GravarProdutoAlternativoNaCotacaoDoUsuarioCotante(
            fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante obj)
        {
            return ditenscotacaofilhausuariocotanteprodutosalternativos.GravarProdutoAlternativoNaCotacaoDoUsuarioCotante(obj);
        }

        //Excluir IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public bool ExcluirProdutoAlternativoNaCotacaoDoUsuarioCotante(int idImagemPRodutoAlternativo)
        {
            return ditenscotacaofilhausuariocotanteprodutosalternativos.ExcluirProdutoAlternativoNaCotacaoDoUsuarioCotante(idImagemPRodutoAlternativo);
        }

        //Excluir TODAS as IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public string ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(int idProdutoCotacao)
        {
            return ditenscotacaofilhausuariocotanteprodutosalternativos.ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(idProdutoCotacao);
        }

        //Consultar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(int idProdutoCotacao)
        {
            return ditenscotacaofilhausuariocotanteprodutosalternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(idProdutoCotacao);
        }
    }
}
