using ClienteMercado.Data.Entities;
using ClienteMercado.Infra.Repositories;
using System;
using System.Collections.Generic;

namespace ClienteMercado.Domain.Services
{
    public class NTiposFreteService
    {
        DTiposFreteRepository dtRepository = new DTiposFreteRepository();

        //Consultar os TIPOS de FRETE
        public List<tipos_frete> ListaDeTiposDeFrete()
        {
            return dtRepository.ListaDeTiposDeFrete();
        }

        //CARREGAR DESCRIÇÃO do TIPO de FRETE
        public string ConsultarDescricaoTipoFrete(int idTipoFrete)
        {
            return dtRepository.ConsultarDescricaoTipoFrete(idTipoFrete);
        }
    }
}
