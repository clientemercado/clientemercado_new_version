using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosService
    {
        DItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosRepository ditenscotacaofilhausuarioempresaprodutosalternativos =
            new DItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosRepository();

        //Gravar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa GravarProdutoAlternativoNaCotacaoDoUsuarioEmpresa(
            fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa obj)
        {
            return ditenscotacaofilhausuarioempresaprodutosalternativos.GravarProdutoAlternativoNaCotacaoDoUsuarioEmpresa(obj);
        }

        //Excluir IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public bool ExcluirProdutoAlternativoNaCotacaoDoUsuarioEmpresa(int idImagemPRodutoAlternativo)
        {
            return ditenscotacaofilhausuarioempresaprodutosalternativos.ExcluirProdutoAlternativoNaCotacaoDoUsuarioEmpresa(idImagemPRodutoAlternativo);
        }

        //Excluir TODAS as IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public string ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(int idProdutoCotacao)
        {
            return ditenscotacaofilhausuarioempresaprodutosalternativos.ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(idProdutoCotacao);
        }

        //Consultar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(int idProdutoCotacao)
        {
            return ditenscotacaofilhausuarioempresaprodutosalternativos.ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(idProdutoCotacao);
        }
    }
}
