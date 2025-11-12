using Domain.Contracts;
using Domain.Entities; // Ensure this is present for BaseEntity<TKey>
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal static class SpecificationsEvaluator
    {
        
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
            IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity, TKey> specifications)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
           
            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);

           if (specifications.Includes?.Count() > 0)
            {
                query = specifications.Includes
                    .Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            }

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);








            return query;


        }
    }
}
