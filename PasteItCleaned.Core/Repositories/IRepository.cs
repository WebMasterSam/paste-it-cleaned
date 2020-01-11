using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PasteItCleaned.Backend.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity Add(TEntity entity);
        List<TEntity> AddRange(List<TEntity> entities);
    }
}
