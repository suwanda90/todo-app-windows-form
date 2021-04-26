using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces.Query;

namespace ApplicationCore.Helpers.Query
{
    public class IncludeQuery<TEntity, TPreviousProperty> : IIncludeQuery<TEntity, TPreviousProperty>
    {
        public Dictionary<IIncludeQuery, string> PathMap { get; } = new Dictionary<IIncludeQuery, string>();
        public IncludeVisitor Visitor { get; } = new IncludeVisitor();

        public IncludeQuery(Dictionary<IIncludeQuery, string> pathMap)
        {
            PathMap = pathMap;
        }

        internal object ThenInclude(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> Paths => PathMap.Select(x => x.Value).ToHashSet();
    }
}
