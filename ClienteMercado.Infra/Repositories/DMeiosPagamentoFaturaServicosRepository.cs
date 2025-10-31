using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DMeiosPagamentoFaturaServicosRepository
    {
        public List<meios_pagamento_fatura_servicos> ListaDeMeiosPagamento()
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                return _contexto.meios_pagamento_fatura_servicos.ToList();
            }
        }
    }
}
