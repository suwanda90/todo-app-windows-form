using System.Linq.Expressions;

namespace ApplicationCore.Helpers.Query
{
    public class IncludeVisitor : ExpressionVisitor
    {
        public string Path { get; set; } = string.Empty;

        protected override Expression VisitMember(MemberExpression node)
        {
            Path = string.IsNullOrEmpty(Path) ? node.Member.Name : $"{node.Member.Name}.{Path}";

            return base.VisitMember(node);
        }
    }
}
