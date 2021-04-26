using System.Linq;
using ApplicationCore.Interfaces.Query;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BaseSpecificationQuery<TEntity> where TEntity : class
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecificationQuery<TEntity> specification)
        {
            var query = inputQuery;

            if (specification.WhereCriteria != null)
            {
                query = query.Where(specification.WhereCriteria);
            }

            if (specification.Includes != null)
            {
                query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (specification.IncludeStrings != null)
            {
                query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }

            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                             .Take(specification.Take);
            }

            return query;
        }
    }
}
