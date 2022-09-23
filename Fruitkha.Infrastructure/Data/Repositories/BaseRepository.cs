using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Fruitkha.Core.Interfaces;
using Fruitkha.Infrastructure.Data.DbContexts;

namespace Fruitkha.Infrastructure.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly VelarDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(VelarDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
        
        public async Task<TEntity> GetByKeyAsync<TKey>(TKey key) => await _dbSet.FindAsync(key);
        
        public async Task<TEntity> GetByPairOfKeysAsync<TFirstKey, TSecondKey>
            (TFirstKey firstKey, TSecondKey secondKey) => await _dbSet.FindAsync(firstKey, secondKey);
        
        public async Task<TEntity> AddAsync(TEntity entity) => (await _dbSet.AddAsync(entity)).Entity;
        
        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => _dbContext.Entry(entity).State = EntityState.Modified);
        }
        
        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }
        
        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbSet.RemoveRange(entities));
        }
        
        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            var query = includes
                .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_dbSet, (current, 
                    include) => current.Include(include));
            return query;
        }
        
        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }
        
        public async Task<IDbContextTransaction> BeginTransactionAsync
            (System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted) 
            => (await _dbContext.Database.BeginTransactionAsync(isolationLevel));
        
        public async Task<IEnumerable<TEntity>> GetListBySpecAsync(ISpecification<TEntity> specification)
            => await ApplySpecification(specification).ToListAsync();
        
        public async Task<IEnumerable<TReturn>> GetListBySpecAsync<TReturn>(ISpecification<TEntity, TReturn> specification)
            => await ApplySpecification(specification).ToListAsync();
        
        public async Task<TEntity> GetFirstBySpecAsync(ISpecification<TEntity> specification)
            => await ApplySpecification(specification).FirstOrDefaultAsync();
        
        public async Task<bool> AnyBySpecAsync(ISpecification<TEntity> specification)
            => await ApplySpecification(specification).AnyAsync();
        
        public async Task<bool> AnyBySpecAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, bool>> expression)
            => await ApplySpecification(specification).AnyAsync(expression);
        
        public async Task<bool> AllBySpecAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, bool>> expression)
            => await ApplySpecification(specification).AllAsync(expression);
        
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(_dbSet, specification);
        }
        
        private IQueryable<TReturn> ApplySpecification<TReturn>(ISpecification<TEntity, TReturn> specification)
        {
            var evaluator = new SpecificationEvaluator();
            return evaluator.GetQuery(_dbSet, specification);
        }
        
        public async Task<int> SqlQuery(string sqlQuery) => await _dbContext.Database.ExecuteSqlRawAsync(sqlQuery);
        
        public async Task SetOriginalRowVersion<TEssence>(TEssence entity, byte[] rowVersion)
            where TEssence : class, IRowVersion
        {
            await Task.Run(() => _dbContext.Entry(entity).Property(x => x.RowVersion).OriginalValue = rowVersion);
        }
    }
}