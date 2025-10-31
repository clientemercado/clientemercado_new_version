using System.Collections.Generic;

namespace ClienteMercado.Infra.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        void Adicionar(TEntity obj);
        TEntity ObterPeloId(int id);
        IEnumerable<TEntity> ObterTodos();
        void Atualizar(TEntity obj);
        void Excluir(TEntity obj);
        void Dispose();
    }
}
