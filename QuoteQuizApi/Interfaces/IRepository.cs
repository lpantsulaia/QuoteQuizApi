using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace QuoteQuizApi.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Update(TEntity entity);

        TEntity Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entiry);
        void RemovRange(IEnumerable<TEntity> entities);
    }
}
