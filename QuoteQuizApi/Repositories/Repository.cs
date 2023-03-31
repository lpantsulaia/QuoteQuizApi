using Microsoft.EntityFrameworkCore;
using QuoteQuizApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QuoteQuizApi.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        public Repository(DbContext context)
        {
            Context = context;
        }

        public TEntity Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
         
            return entity;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
            
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await Context.Set<TEntity>().ToListAsync();
        }

        public void Remove(TEntity entiry)
        {
            Context.Set<TEntity>().Remove(entiry);
           
        }

        public void RemovRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
         
        }

        public TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            
            return entity;
        }
    }
}
