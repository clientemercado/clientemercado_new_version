using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NProdutosServicosEmpresaProfissionalService
    {
        DProdutosServicosEmpresaProfissionalRepository dRepository = new DProdutosServicosEmpresaProfissionalRepository();

        //Carrega a lista de atividades ligadas ao grupo de atividades da empresa, para seleção no autocomplete
        public List<produtos_servicos_empresa_profissional> ListaAtividadesEmpresaProfissional(int idRamoAtividade)
        {
            return dRepository.ListaAtividadesEmpresaProfissional(idRamoAtividade);
        }

        //Gravando nova atividade, produto ou serviço para Empresa/Profissional
        public produtos_servicos_empresa_profissional GravarNovoProdutoServicoEmpresaProfissional(produtos_servicos_empresa_profissional obj)
        {
            return dRepository.GravarNovoProdutoServicoEmpresaProfissional(obj);
        }

        //Consultar dados dos PRODUTOS da COTAÇÂO
        public produtos_servicos_empresa_profissional ConsultarDadosDoProdutoDaCotacao(int idProduto)
        {
            return dRepository.ConsultarDadosDoProdutoDaCotacao(idProduto);
        }

        //CARREGA a LISTA de PRODUTOS de TESTE
        public List<produtos_servicos_empresa_profissional> ListaDeProdutosParaTeste()
        {
            return dRepository.ListaDeProdutosParaTeste();
        }

        //BUSCAR LISTA de PRODUTOS conforme digitado
        public List<produtos_servicos_empresa_profissional> ListaProdutosEmpresa(string term)
        {
            return dRepository.ListaProdutosEmpresa(term);
        }

        //BUSCAR NOME do PRODUTO
        internal string ConsultarDescricaoDoProduto(int idProduto)
        {
            return dRepository.ConsultarDescricaoDoProduto(idProduto);
        }
    }
}
