using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoFilhaNegociacaoUsuarioEmpresaProdutosAlternativosRepository
    {
        //Gravar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa GravarProdutoAlternativoNaCotacaoDoUsuarioEmpresa(
            fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa gravarFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa.Add(obj);
                _contexto.SaveChanges();

                return gravarFotosProdutosAlternativos;
            }
        }

        //Excluir IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public bool ExcluirProdutoAlternativoNaCotacaoDoUsuarioEmpresa(int idImagemPRodutoAlternativo)
        {
            bool excluidos = false;

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa excluirFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa.FirstOrDefault(m => m.ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_EMPRESA.Equals(idImagemPRodutoAlternativo));

                if (excluirFotosProdutosAlternativos != null)
                {
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa.Remove(excluirFotosProdutosAlternativos);
                    _contexto.SaveChanges();

                    excluidos = true;
                }

                return excluidos;
            }
        }

        //Excluir TODAS as IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public string ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(int idProdutoCotacao)
        {
            string nomesImagensExcluidas = "";

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> excluirFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa.Where(m => (m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA.Equals(idProdutoCotacao))).ToList();

                for (int i = 0; i < excluirFotosProdutosAlternativos.Count; i++)
                {
                    //Popula um stringão com o nome de todas as IMAGENS que estão sendo EXCLUÍDAS,
                    //para depois serem excluídas fisicamente
                    if (nomesImagensExcluidas == "")
                    {
                        nomesImagensExcluidas = excluirFotosProdutosAlternativos[i].NOME_ARQUIVO_IMAGEM;
                    }
                    else if (nomesImagensExcluidas != "")
                    {
                        nomesImagensExcluidas = (nomesImagensExcluidas + "," + excluirFotosProdutosAlternativos[i].NOME_ARQUIVO_IMAGEM);
                    }

                    //Ato de EXCLUSÃO das IMAGENS do banco
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa.Remove(excluirFotosProdutosAlternativos[i]);
                    _contexto.SaveChanges();
                }

                return nomesImagensExcluidas;
            }
        }

        //Consultar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE / USUÁRIO EMPRESA
        public List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioEmpresa(int idProdutoCotacao)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa> fotosArmazenadas =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_empresa
                    .Where(m => m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_EMPRESA.Equals(idProdutoCotacao)).ToList();

                return fotosArmazenadas;
            }
        }
    }
}
