using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DTiposFreteRepository : RepositoryBase<tipos_frete>
    {
        //Consultar os TIPOS de FRETE
        public List<tipos_frete> ListaDeTiposDeFrete()
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                List<tipos_frete> dadosDosTiposDeFrete = _contexto.tipos_frete.ToList();

                return dadosDosTiposDeFrete;
            }
        }

        //CARREGAR DESCRIÇÃO do TIPO de FRETE
        public string ConsultarDescricaoTipoFrete(int idTipoFrete)
        {
            tipos_frete dadosFrete = _contexto.tipos_frete.FirstOrDefault(m => (m.ID_TIPO_FRETE == idTipoFrete));

            var descricaoFrete = dadosFrete.DESCRICAO_TIPO_FRETE.Trim().Split('-');

            return descricaoFrete[0];
        }
    }
}
