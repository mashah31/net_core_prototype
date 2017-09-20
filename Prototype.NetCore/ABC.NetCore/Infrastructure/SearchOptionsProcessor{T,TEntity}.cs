using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace ABC.NetCore.Infrastructure
{
    public interface ISearchExpressionProvider
    {
        IEnumerable<string> GetOperators();

        Expression GetComparison(MemberExpression left, string op, ConstantExpression right);

        ConstantExpression GetValue(string input);
    }

    public class SearchExpressionProvider : ISearchExpressionProvider
    {
        protected const string EqualsOperator = "eq";

        public virtual IEnumerable<string> GetOperators()
        {
            yield return EqualsOperator;
        }

        public virtual Expression GetComparison(MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case EqualsOperator: return Expression.Equal(left, right);
                default: throw new ArgumentException($"Invalid operator '{op}'.");
            }
        }

        public virtual ConstantExpression GetValue(string input)
            => Expression.Constant(input);
    }

    public class StringSearchExpressionProvider : SearchExpressionProvider
    {
        private const string StartsWithOperator = "sw";
        private const string ContainsOperator = "co";

        private static readonly MethodInfo StartsWithMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "StartsWith" && m.GetParameters().Length == 2);

        private static readonly MethodInfo StringEqualsMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "Equals" && m.GetParameters().Length == 2);

        private static readonly MethodInfo ContainsMethod = typeof(string)
            .GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 1);

        private static readonly ConstantExpression IgnoreCase
            = Expression.Constant(StringComparison.OrdinalIgnoreCase);

        public override IEnumerable<string> GetOperators()
            => base.GetOperators()
            .Concat(new[]
            {
                StartsWithOperator,
                ContainsOperator
            });

        public override Expression GetComparison(MemberExpression left, string op, ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case StartsWithOperator:
                    return Expression.Call(left, StartsWithMethod, right, IgnoreCase);

                // TODO: This may or may not be case-insensitive, depending
                // on how your database translates Contains()
                case ContainsOperator:
                    var leftToLower = Expression.Call(left,
                                  typeof(string).GetMethod("ToLower", Type.EmptyTypes));
                    var rightToLower = Expression.Constant(right.Value.ToString().ToLower());
                    return Expression.Call(leftToLower, ContainsMethod, rightToLower);

                // Handle the "eq" operator ourselves (with a case-insensitive compare)
                case EqualsOperator:
                    return Expression.Call(left, StringEqualsMethod, right, IgnoreCase);

                default: return base.GetComparison(left, op, right);
            }
        }
    }

    public class NumberSearchExpressionProvider : ComparableSearchExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            if (!int.TryParse(input, out var intVal))
                throw new ArgumentException("Invalid search value.");

            return Expression.Constant(int.Parse(input));
        }
    }

    public class DecimalToIntSearchExpressionProvider : ComparableSearchExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            if (!decimal.TryParse(input, out var dec))
                throw new ArgumentException("Invalid search value.");

            return Expression.Constant(decimal.Parse(input));
        }
    }

    public class DateTimeSearchExpressionProvider : ComparableSearchExpressionProvider
    {
        public override ConstantExpression GetValue(string input)
        {
            if (!DateTimeOffset.TryParse(input, out var value))
                throw new ArgumentException("Invalid search value.");

            return Expression.Constant(value);
        }
    }

    public abstract class ComparableSearchExpressionProvider : SearchExpressionProvider
    {
        private const string GreaterThanOperator = "gt";
        private const string GreaterThanEqualToOperator = "gte";
        private const string LessThanOperator = "lt";
        private const string LessThanEqualToOperator = "lte";

        public override IEnumerable<string> GetOperators()
            => base.GetOperators()
            .Concat(new[]
            {
                GreaterThanOperator,
                GreaterThanEqualToOperator,
                LessThanOperator,
                LessThanEqualToOperator
            });

        public override Expression GetComparison(
            MemberExpression left,
            string op,
            ConstantExpression right)
        {
            switch (op.ToLower())
            {
                case GreaterThanOperator: return Expression.GreaterThan(left, right);
                case GreaterThanEqualToOperator: return Expression.GreaterThanOrEqual(left, right);
                case LessThanOperator: return Expression.LessThan(left, right);
                case LessThanEqualToOperator: return Expression.LessThanOrEqual(left, right);
                default: return base.GetComparison(left, op, right);
            }
        }
    }
}
