﻿using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fruitkha.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByKeyAsync<TKey>(TKey key);
        Task<TEntity> GetByPairOfKeysAsync<TKey, TKey1>(TKey key, TKey1 key1);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
        Task<int> SqlQuery(string sqlQuery);
        Task<int> SaveChangesAsync();
        Task AddRangeAsync(List<TEntity> entities);
        Task<IDbContextTransaction> BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadCommitted);
        Task<IEnumerable<TEntity>> GetListBySpecAsync(ISpecification<TEntity> specification);
        Task<IEnumerable<TReturn>> GetListBySpecAsync<TReturn>(ISpecification<TEntity, TReturn> specification);
        Task<TEntity> GetFirstBySpecAsync(ISpecification<TEntity> specification);
        Task<bool> AnyBySpecAsync(ISpecification<TEntity> specification);
        Task<bool> AnyBySpecAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, bool>> anyExpression);
        Task<bool> AllBySpecAsync(ISpecification<TEntity> specification, Expression<Func<TEntity, bool>> allExpression);
        Task SetOriginalRowVersion<T>(T entity, byte[] rowVersion) where T : class, IRowVersion;
    }
}