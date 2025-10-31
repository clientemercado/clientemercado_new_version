using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DProdutosServicosEmpresaProfissionalRepository : RepositoryBase<produtos_servicos_empresa_profissional>
    {
        //Carrega a lista de atividades ligadas ao grupo de atividades da empresa, para seleção no autocomplete (Conforme tipo selecionado (Produtos / Serviços))
        public List<produtos_servicos_empresa_profissional> ListaAtividadesEmpresaProfissional(int idRamoAtividade)
        {
            return _contexto.produtos_servicos_empresa_profissional.Where(m => m.ID_GRUPO_ATIVIDADES == idRamoAtividade).ToList();
        }

        //Gravando nova atividade, produto ou serviço para Empresa/Profissional
        public produtos_servicos_empresa_profissional GravarNovoProdutoServicoEmpresaProfissional(produtos_servicos_empresa_profissional obj)
        {
            produtos_servicos_empresa_profissional produtosServicosEmpresaProfissional = new produtos_servicos_empresa_profissional();

            produtos_servicos_empresa_profissional jaExisteProduto =
                _contexto.produtos_servicos_empresa_profissional.FirstOrDefault(m => ((m.ID_GRUPO_ATIVIDADES == obj.ID_GRUPO_ATIVIDADES) && (m.DESCRICAO_PRODUTO_SERVICO == obj.DESCRICAO_PRODUTO_SERVICO)));

            if (jaExisteProduto == null)
            {
                produtosServicosEmpresaProfissional =
                    _contexto.produtos_servicos_empresa_profissional.Add(obj);
                _contexto.SaveChanges();
            }

            return produtosServicosEmpresaProfissional;
        }

        //Consultar dados dos PRODUTOS da COTAÇÂO
        public produtos_servicos_empresa_profissional ConsultarDadosDoProdutoDaCotacao(int idProduto)
        {
            produtos_servicos_empresa_profissional dadosDoProduto =
                _contexto.produtos_servicos_empresa_profissional.FirstOrDefault(m => (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS.Equals(idProduto)));

            return dadosDoProduto;
        }

        //BUSCAR NOME do PRODUTO
        public string ConsultarDescricaoDoProduto(int idProduto)
        {
            produtos_servicos_empresa_profissional dadosDoProduto =
                _contexto.produtos_servicos_empresa_profissional.FirstOrDefault(m => (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS == idProduto));

            return dadosDoProduto.DESCRICAO_PRODUTO_SERVICO;
        }

        //BUSCAR LISTA de PRODUTOS conforme digitado
        public List<produtos_servicos_empresa_profissional> ListaProdutosEmpresa(string term)
        {
            var query = "SELECT * FROM produtos_servicos_empresa_profissional WHERE DESCRICAO_PRODUTO_SERVICO LIKE '" + term + "%'";

            var result = _contexto.Database.SqlQuery<produtos_servicos_empresa_profissional>(query).ToList();
            return result;
        }

        //CARREGA a LISTA de PRODUTOS de TESTE
        public List<produtos_servicos_empresa_profissional> ListaDeProdutosParaTeste()
        {
            List<produtos_servicos_empresa_profissional> listaDeProdutosDaCotacaoTeste =
                _contexto.produtos_servicos_empresa_profissional.Where(m => (m.ID_CODIGO_PRODUTOS_SERVICOS_EMPRESAS_PROFISSIONAIS > 0)).Take(5).ToList();

            return listaDeProdutosDaCotacaoTeste;
        }
    }
}
