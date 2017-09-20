using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ABC.NetCore.Models;
using ABC.NetCore.CustomAttributes;

namespace ABC.NetCore.Infrastructure
{
    public class SearchOptionsProcessor<T, TEntity>
    {
        private readonly string[] _searchQuery;

        public SearchOptionsProcessor(string[] searchQuery)
        {
            _searchQuery = searchQuery;
        }

        public IEnumerable<SearchTerm> GetAllTerms()
        {
            if (_searchQuery == null) yield break;

            foreach (var expression in _searchQuery)
            {
                if (string.IsNullOrEmpty(expression)) continue;

                // Sample example:: "fieldName op Value..."
                var tokens = expression.Split(' ');
                if (tokens.Length == 0)
                {
                    // search term is invalid as it was empty, return false
                    yield return new SearchTerm { ValidSyntax = false, Name = expression };
                    continue;
                }
                if (tokens.Length < 3)
                {
                    // search term is invalid as it didn't had three parts, return false; name op value;
                    yield return new SearchTerm { ValidSyntax = false, Name = tokens[0] };
                    continue;
                }

                // return final valid search term
                yield return new SearchTerm
                {
                    ValidSyntax = true,
                    Name = tokens[0],
                    Operator = tokens[1],
                    Value = string.Join(" ", tokens.Skip(2))
                };
            }
        }

        public IEnumerable<SearchTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().Where(x => x.ValidSyntax).ToArray();
            if (!queryTerms.Any()) yield break;

            var declareTerms = GetTermsFromModel();
            foreach (var term in queryTerms)
            {
                var declareTerm = declareTerms.SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.CurrentCultureIgnoreCase));
                if (declareTerm == null) continue;

                yield return new SearchTerm
                {
                    ValidSyntax = term.ValidSyntax,
                    Name = declareTerm.Name,
                    Operator = term.Operator,
                    Value = term.Value,
                    ExpressionProvider = declareTerm.ExpressionProvider
                };
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var terms = GetValidTerms().ToArray();
            if (!terms.Any()) return query;

            var modifiedQuery = query;

            foreach (var term in terms)
            {
                var propertyInfo = ExpressionHelper
                    .GetPropertyInfo<TEntity>(term.Name);
                var obj = ExpressionHelper.Parameter<TEntity>();

                // Build up the LINQ expression backwards:
                // query = query.Where(x => x.Property == "Value")

                // Build x.Property
                var left = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                // Build "Value"
                var right = term.ExpressionProvider.GetValue(term.Value);

                // Build x.Property == "Value"
                // Extensive handling of different type of arguments and values comaprision handled 
                // under ExpressionProvider.GetComparison
                var comparisionExpression = term.ExpressionProvider.GetComparison(left, term.Operator, right);

                // Build x => x.Property == "Value"
                var lambdaExpression = ExpressionHelper
                    .GetLambda<TEntity, bool>(obj, comparisionExpression);

                // query = query.Where..
                modifiedQuery = ExpressionHelper.CallWhere(modifiedQuery, lambdaExpression);
            }

            return modifiedQuery;
        }

        private static IEnumerable<SearchTerm> GetTermsFromModel()
            => typeof(T).GetTypeInfo()
            .DeclaredProperties
            .Where(p => p.GetCustomAttributes<SearchableAttribute>().Any())
            .Select(p => new SearchTerm
            {
                Name = p.Name,
                ExpressionProvider = p.GetCustomAttribute<SearchableAttribute>().ExpressionProvider
            });
    }
}
