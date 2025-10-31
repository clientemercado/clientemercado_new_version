using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NMeiosPagamentoFaturaServicosService
    {
        DMeiosPagamentoFaturaServicosRepository dmeiospagamentofaturaservicos = new DMeiosPagamentoFaturaServicosRepository();

        public List<meios_pagamento_fatura_servicos> ListaDeMeiosPagamento()
        {
            return dmeiospagamentofaturaservicos.ListaDeMeiosPagamento();
        }
    }
}
