using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DItensCotacaoFilhaNegociacaoUsuarioCotanteProdutosAlternativosRepository
    {
        //Gravar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante GravarProdutoAlternativoNaCotacaoDoUsuarioCotante(
            fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante gravarFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante.Add(obj);
                _contexto.SaveChanges();

                return gravarFotosProdutosAlternativos;
            }
        }

        //Excluir IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public bool ExcluirProdutoAlternativoNaCotacaoDoUsuarioCotante(int idImagemPRodutoAlternativo)
        {
            bool excluidos = false;

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante excluirFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante.FirstOrDefault(m => m.ID_CODIGO_ITEM_ALTERNATIVO_COTACAO_FILHA_USUARIO_COTANTE.Equals(idImagemPRodutoAlternativo));

                if (excluirFotosProdutosAlternativos != null)
                {
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante.Remove(excluirFotosProdutosAlternativos);
                    _contexto.SaveChanges();

                    excluidos = true;
                }

                return excluidos;
            }
        }

        //Excluir TODAS as IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public string ExcluirTodasAsImagensDeProdutosAlternativosAnexadas(int idProdutoCotacao)
        {
            string nomesImagensExcluidas = "";

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> excluirFotosProdutosAlternativos =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante.Where(m => (m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE.Equals(idProdutoCotacao))).ToList();

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
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante.Remove(excluirFotosProdutosAlternativos[i]);
                    _contexto.SaveChanges();
                }

                return nomesImagensExcluidas;
            }
        }

        //Consultar IMAGENS dos PRODUTOS ALTERNATIVOS para os itens da cotação enviada pelo USUÁRIO COTANTE
        public List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> ConsultarRegistrosDasFotosDosProdutosAnexadosNaCotacaoDoUsuarioCotante(int idProdutoCotacao)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante> fotosArmazenadas =
                    _contexto.fotos_itens_alternativos_cotacao_filha_negociacao_usuario_cotante
                    .Where(m => m.ID_CODIGO_COTACAO_FILHA_NEGOCIACAO_USUARIO_COTANTE.Equals(idProdutoCotacao)).ToList();

                return fotosArmazenadas;
            }
        }
    }
}
