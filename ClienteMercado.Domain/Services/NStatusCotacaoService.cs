using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;

namespace ClienteMercado.Domain.Services
{
    public class NStatusCotacaoService
    {
        DStatusCotacaoRepository dstatusdacotacao = new DStatusCotacaoRepository();

        //Consultar dados do Status da Cotação
        public status_cotacao ConsultarDadosDoStatusDaCotacao(int idStatusDaCotacao)
        {
            return dstatusdacotacao.ConsultarDadosDoStatusDaCotacao(idStatusDaCotacao);
        }
    }
}
