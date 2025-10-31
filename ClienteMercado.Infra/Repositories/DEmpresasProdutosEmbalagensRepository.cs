using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEmpresasProdutosEmbalagensRepository : RepositoryBase<empresas_produtos_embalagens>
    {
        //TRAZ EMBALAGENS VINCULADAS AO PRODUTO REGISTRADO
        public List<ListaDeEmbalagensDosProdutosViewModel> ListaDeEmbalagensVinculadasAoProduto(string term, int codProduto)
        {
            try
            {
                var query = "";
                List<ListaDeEmbalagensDosProdutosViewModel> listaDeEmbalagens = new List<ListaDeEmbalagensDosProdutosViewModel>();

                if (codProduto > 0)
                {
                    //CONFORME FILTRO
                    query = "SELECT * FROM empresas_produtos_embalagens WHERE ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS IN (" + codProduto + ") AND DESCRICAO_PRODUTO_EMBALAGEM LIKE '%" + term + "%'";
                    listaDeEmbalagens = _contexto.Database.SqlQuery<ListaDeEmbalagensDosProdutosViewModel>(query).ToList();

                    if (listaDeEmbalagens.Count == 0)
                    {
                        //TRAZ TODAS
                        query = "SELECT * FROM empresas_produtos_embalagens WHERE DESCRICAO_PRODUTO_EMBALAGEM LIKE '%" + term + "%'";
                        listaDeEmbalagens = _contexto.Database.SqlQuery<ListaDeEmbalagensDosProdutosViewModel>(query).ToList();
                    }
                }
                else
                {
                    //TRAZ TODAS
                    query = "SELECT * FROM empresas_produtos_embalagens WHERE DESCRICAO_PRODUTO_EMBALAGEM LIKE '%" + term + "%'";
                    listaDeEmbalagens = _contexto.Database.SqlQuery<ListaDeEmbalagensDosProdutosViewModel>(query).ToList();
                }

                return listaDeEmbalagens;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //BUSCAR EMBALAGEM
        public string ConsultarDescricaoDaEmbalagemDoProduto(int idEmbalagens)
        {
            empresas_produtos_embalagens dadosEmbalagem =
                _contexto.empresas_produtos_embalagens.FirstOrDefault(m => (m.ID_EMPRESAS_PRODUTOS_EMBALAGENS == idEmbalagens));

            return dadosEmbalagem.DESCRICAO_PRODUTO_EMBALAGEM;
        }

        //BUSCAR EMBALAGEM do PRODUTO COTADO
        public empresas_produtos_embalagens ConsultarDadosDaEmbalagemDoProduto(int idEmbalagem)
        {
            try
            {
                empresas_produtos_embalagens dadosDaEmbalagem =
                    _contexto.empresas_produtos_embalagens.FirstOrDefault(m => (m.ID_EMPRESAS_PRODUTOS_EMBALAGENS == idEmbalagem));

                return dadosDaEmbalagem;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        //GRAVAR VÍNCULO da EMBALAGEM ao PRODUTO
        public empresas_produtos_embalagens GravarVinculoDaEmbalagemAoProduto(empresas_produtos_embalagens obj)
        {
            try
            {
                empresas_produtos_embalagens embalagemGravada = new empresas_produtos_embalagens();

                empresas_produtos_embalagens produtoEmbalagemExiste =
                    _contexto.empresas_produtos_embalagens.FirstOrDefault(m => ((m.DESCRICAO_PRODUTO_EMBALAGEM == obj.DESCRICAO_PRODUTO_EMBALAGEM) && (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS == obj.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS)));

                if (produtoEmbalagemExiste == null)
                {
                    embalagemGravada =
                        _contexto.empresas_produtos_embalagens.Add(obj);
                    _contexto.SaveChanges();
                }

                return embalagemGravada;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }
    }
}
