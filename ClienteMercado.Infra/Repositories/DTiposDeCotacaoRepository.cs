using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DTiposDeCotacaoRepository
    {
        //Consultar os dados dos TIPOs de COTAÇÃO
        public tipos_cotacao ConsultarDadosDoTipoDeCotacao(tipos_cotacao obj)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                tipos_cotacao consultarDados =
                    _contexto.tipos_cotacao.FirstOrDefault(m => (m.ID_CODIGO_TIPO_COTACAO.Equals(obj.ID_CODIGO_TIPO_COTACAO)));

                return consultarDados;
            }
        }
    }
}
