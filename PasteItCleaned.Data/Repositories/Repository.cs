using Microsoft.EntityFrameworkCore;
using PasteItCleaned.Backend.Core.Repositories;
using PasteItCleaned.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PasteItCleaned.Backend.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly PasteItCleanedDbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context as PasteItCleanedDbContext;
        }

        public TEntity Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);

            return entity;
        }

        public List<TEntity> AddRange(List<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);

            return entities;
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        protected PagedList<TEntity> PagedList(IQueryable<TEntity> query, int page, int pageSize)
        {
            var result = new PagedList<TEntity>();

            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = query.Count();

            var pageCount = (double)result.RowCount / pageSize;

            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            result.Results = query.Skip(skip).Take(pageSize).ToList();

            return result;
        }

    }
}
