using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DStatusCotacaoRepository
    {
        //Consultar dados do Status da Cotação
        public status_cotacao ConsultarDadosDoStatusDaCotacao(int idStatusDaCotacao)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                status_cotacao statusDaCotacao = _contexto.status_cotacao.FirstOrDefault(m => m.ID_CODIGO_STATUS_COTACAO.Equals(idStatusDaCotacao));

                return statusDaCotacao;
            }
        }
    }
}
