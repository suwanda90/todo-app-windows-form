using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ApplicationCore.Interfaces.Query
{
    public interface ISpecificationQuery<T>
    {
        Expression<Func<T, bool>> WhereCriteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Expression<Func<T, object>> GroupBy { get; }
        
        bool IsPagingEnabled { get; }
        int Take { get; }
        int Skip { get; }
    }
}
