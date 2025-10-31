using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using ClienteMercado.Utils.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMercado.Infra.Repositories
{
    public class DFormaPagamentoRepository : RepositoryBase<forma_pagamento>
    {
        int? idEmpresa = Sessao.IdEmpresaUsuario;

        //GRAVAR FORMA DE PAGAMENTO
        public forma_pagamento GravarFormaPagto(forma_pagamento obj)
        {
            forma_pagamento gravarFormaPagto =
                _contexto.forma_pagamento.Add(obj);
            _contexto.SaveChanges();

            return gravarFormaPagto;
        }

        //CARREGAR LISTA de FORMAS de PAGAMENTO
        public List<forma_pagamento> CarregarListaDeFormasPagamento()
        {
            List<forma_pagamento> listaFormasPagamento = _contexto.forma_pagamento.Where(m => (m.ID_CODIGO_EMPRESA == idEmpresa)).ToList();

            return listaFormasPagamento;
        }
    }
}
