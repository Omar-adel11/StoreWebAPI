using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities;


namespace Persistance
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecifications<TKey, TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            query = spec.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));
            return query;
        }

    }
}
