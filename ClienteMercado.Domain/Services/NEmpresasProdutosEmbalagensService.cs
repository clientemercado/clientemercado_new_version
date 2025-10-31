using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using ClienteMercado.Utils.ViewModel;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NEmpresasProdutosEmbalagensService
    {
        DEmpresasProdutosEmbalagensRepository dRepository = new DEmpresasProdutosEmbalagensRepository();

        //TRAZ EMBALAGENS VINCULADAS AO PRODUTO REGISTRADO
        public List<ListaDeEmbalagensDosProdutosViewModel> ListaDeEmbalagensVinculadasAoProduto(string term, int codProduto)
        {
            return dRepository.ListaDeEmbalagensVinculadasAoProduto(term, codProduto);
        }

        //GRAVAR VÍNCULO da EMBALAGEM ao PRODUTO
        public empresas_produtos_embalagens GravarVinculoDaEmbalagemAoProduto(empresas_produtos_embalagens obj)
        {
            return dRepository.GravarVinculoDaEmbalagemAoProduto(obj);
        }

        //BUSCAR EMBALAGEM do PRODUTO COTADO
        public empresas_produtos_embalagens ConsultarDadosDaEmbalagemDoProduto(int idEmbalagem)
        {
            return dRepository.ConsultarDadosDaEmbalagemDoProduto(idEmbalagem);
        }

        //BUSCAR EMBALAGEM
        internal string ConsultarDescricaoDaEmbalagemDoProduto(int idEmbalagens)
        {
            return dRepository.ConsultarDescricaoDaEmbalagemDoProduto(idEmbalagens);
        }
    }
}
