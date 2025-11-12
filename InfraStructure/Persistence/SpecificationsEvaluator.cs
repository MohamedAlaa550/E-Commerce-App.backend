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
        // Add constraint: where TEntity : BaseEntity<TKey>
        public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
            IQueryable<TEntity> inputQuery,
            ISpecifications<TEntity, TKey> specifications)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            // modify the IQueryable using the specification's criteria expression
            if (specifications.Criteria != null)
                query = query.Where(specifications.Criteria);

            if (specifications.Includes?.Count()> 0)
            {
                specifications.Includes.Aggregate(query,
                    (currentQuerry, includeExpression) => currentQuerry.Include(includeExpression));
                
            }

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            else if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);








            return query;


        }
    }
}
