using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ApplicationCore.Helpers.Query;
using ApplicationCore.Interfaces.Query;

namespace ApplicationCore.Specifications
{
    public abstract class SpecificationQuery<TEntity> : ISpecificationQuery<TEntity>
    {
        protected SpecificationQuery() { }

        protected SpecificationQuery(Expression<Func<TEntity, bool>> whereCriteria)
        {
            WhereCriteria = whereCriteria;
        }
        public Expression<Func<TEntity, bool>> WhereCriteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<TEntity, object>> OrderBy { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public Expression<Func<TEntity, object>> GroupBy { get; set; }
        
        public bool IsPagingEnabled { get; set; } = false;
        public int Take { get; set; }
        public int Skip { get; set; }

        protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected virtual void AddIncludes<TProperty>(Func<IncludeAggregator<TEntity>, IIncludeQuery<TEntity, TProperty>> includeGenerator)
        {
            var includeQuery = includeGenerator(new IncludeAggregator<TEntity>());
            IncludeStrings.AddRange(includeQuery.Paths);
        }

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }
        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        protected virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        //Not used anywhere at the moment, but someone requested an example of setting this up.
        protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }

    }
}
