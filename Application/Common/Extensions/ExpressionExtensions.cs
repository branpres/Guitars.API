using System.Linq.Expressions;

namespace Application.Common.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            Expression body = Expression.AndAlso(a.Body, GetModifiedExpressionBody(a, b));
            return Expression.Lambda<Func<T, bool>>(body, a.Parameters[0]);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            Expression body = Expression.OrElse(a.Body, GetModifiedExpressionBody(a, b));
            return Expression.Lambda<Func<T, bool>>(body, a.Parameters[0]);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> e)
        {
            Expression body = Expression.Not(e.Body);
            return Expression.Lambda<Func<T, bool>>(body, e.Parameters[0]);
        }

        private static Expression GetModifiedExpressionBody<T>(Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            // returns a new expression body so that expression b has the same parameters as expression a
            var visitor = new SwapParameterExpressionVisitor(b.Parameters[0], a.Parameters[0]);
            return visitor.Visit(b.Body);
        }
    }

    internal class SwapParameterExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression From;
        private readonly ParameterExpression To;

        public SwapParameterExpressionVisitor(ParameterExpression from, ParameterExpression to)
        {
            From = from;
            To = to;
        }

        protected override Expression VisitParameter(ParameterExpression parameterExpression)
        {
            return parameterExpression == From ? To : parameterExpression;
        }
    }
}