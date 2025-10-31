using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NTiposDeCotacaoService
    {
        DTiposDeCotacaoRepository dtiposdecotacao = new DTiposDeCotacaoRepository();

        //Consultar os dados dos TIPOs de COTAÇÃO
        public tipos_cotacao ConsultarDadosDoTipoDeCotacao(tipos_cotacao obj)
        {
            return dtiposdecotacao.ConsultarDadosDoTipoDeCotacao(obj);
        }
    }
}
