using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ClienteMercado.Infra.Repositories
{
    public class DEstadosRepository
    {
        //Busca lista de Estados brasileiros
        public List<estados_empresa_usuario> ListaEstados()
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                return _contexto.estados_empresa_usuario.OrderBy(m => m.ID_ESTADOS_EMPRESA_USUARIO).ToList();
            }
        }

        //Busca dados do ESTADO - UF
        public estados_empresa_usuario ConsultarDadosDoEstado(int idEstado)
        {
            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                estados_empresa_usuario dadosDoEstado =
                    _contexto.estados_empresa_usuario.FirstOrDefault(m => (m.ID_ESTADOS_EMPRESA_USUARIO.Equals(idEstado)));

                return dadosDoEstado;
            }
        }
    }
}
