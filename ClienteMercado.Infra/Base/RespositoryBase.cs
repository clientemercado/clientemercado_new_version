using ClienteMercado.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ClienteMercado.Infra.Base
{
    public class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity> where TEntity : class
    {
        protected cliente_mercadoContext _contexto = new cliente_mercadoContext();

        public void Adicionar(TEntity obj)
        {
            this._contexto.Set<TEntity>().Add(obj);
            this._contexto.SaveChanges();
        }

        public void ExcluirChaveComposta(int id_FreserCodigo, int id_CencusCodigo)
        {
            TEntity entity = this._contexto.Set<TEntity>().Find(id_FreserCodigo, id_CencusCodigo);

            if (this._contexto.Entry(entity).State == EntityState.Detached)
                this._contexto.Set<TEntity>().Attach(entity);

            this._contexto.Set<TEntity>().Remove(entity);
            this._contexto.SaveChanges();
        }


        public TEntity ObterPeloId(int id)
        {
            return this._contexto.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> ObterTodos()
        {
            return this._contexto.Set<TEntity>().ToList();
        }

        public void Atualizar(TEntity obj)
        {
            this._contexto.Entry(obj).State = EntityState.Modified;
            this._contexto.SaveChanges();
        }

        public void Excluir(TEntity obj)
        {
            this._contexto.Set<TEntity>().Remove(obj);
            this._contexto.SaveChanges();
        }

        public void Dispose()
        {
            _contexto.Dispose();
        }

    }
}
