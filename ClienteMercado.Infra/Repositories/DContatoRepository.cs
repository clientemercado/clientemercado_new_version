using ClienteMercado.Data.Contexto;
using ClienteMercado.Data.Entities;

namespace ClienteMercado.Infra.Repositories
{
    public class DContatoRepository
    {
        public contato_cliente_mercado GravarContato(contato_cliente_mercado obj)
        {
            //cliente_mercadoContext _contexto = new cliente_mercadoContext();

            //contato_cliente_mercado contato = 
            //    _contexto.contato_cliente_mercado.Add(obj);
            //    _contexto.SaveChanges();

            //return contato;

            using (cliente_mercadoContext _contexto = new cliente_mercadoContext())
            {
                contato_cliente_mercado contato =
                    _contexto.contato_cliente_mercado.Add(obj);
                _contexto.SaveChanges();

                return contato;
            }
        }
    }
}
