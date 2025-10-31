using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Domain.Services
{
    public class NFormaPagamentoService
    {
        DFormaPagamentoRepository dRepository = new DFormaPagamentoRepository();

        //GRAVAR FORMA DE PAGAMENTO
        public forma_pagamento GravarFormaPagto(forma_pagamento obj)
        {
            return dRepository.GravarFormaPagto(obj);
        }

        //CARREGAR LISTA de FORMAS de PAGAMENTO
        public List<forma_pagamento> CarregarListaDeFormasPagamento()
        {
            return dRepository.CarregarListaDeFormasPagamento();
        }
    }
}
