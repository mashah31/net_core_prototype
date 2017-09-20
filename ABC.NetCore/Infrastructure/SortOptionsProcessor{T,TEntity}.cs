using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ABC.NetCore.Models;
using ABC.NetCore.CustomAttributes;

namespace ABC.NetCore.Infrastructure
{
    public class SortOptionsProcessor<T, TEntity>
    {
        private readonly string[] _orderBy;

        public SortOptionsProcessor(string[] orderBy)
        {
            _orderBy = orderBy;
        }

        // Get all valid terms
        public IEnumerable<SortTerm> GetValidTerms()
        {
            // Collect all the terms from query string first
            var queryTerms = GetAllTerms().ToArray();
            if (!queryTerms.Any()) yield break;

            // Collect all valid terms 
            var declareTerms = GetTermsFromModel();

            // Iterate through each and prepare new SortTerm
            foreach (var term in queryTerms)
            {
                var declareTerm = declareTerms.SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.CurrentCultureIgnoreCase));
                if (declareTerm == null) continue;

                yield return new SortTerm
                {
                    Name = declareTerm.Name,
                    Descending = term.Descending,
                    EntityName = declareTerm.EntityName,
                    Default = declareTerm.Default
                };
            }
        }

        // Get all specified terms from query string
        public IEnumerable<SortTerm> GetAllTerms()
        {
            if (_orderBy == null) yield break;

            foreach (var term in _orderBy)
            {
                if (string.IsNullOrEmpty(term)) continue;

                var tokens = term.Split(' ');
                if (tokens.Length == 0)
                {
                    yield return new SortTerm { Name = term };
                    continue;
                }

                // If there is second token than check if it's desc spcification or not?
                var decending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase);
                yield return new SortTerm { Name = tokens[0], Descending = decending };
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var terms = GetValidTerms().ToArray();

            // If there are no terms than look for default
            if (!terms.Any())
            {
                terms = GetTermsFromModel().Where(t => t.Default).ToArray();
            }

            // If there is no default than skip sorting and continue
            if (!terms.Any()) return query;

            var modifiedQuery = query;
            var useThenBy = false;

            // Itereate through each term and build LINQ query
            foreach (var term in terms)
            {
                var propertyInfo = ExpressionHelper
                    .GetPropertyInfo<TEntity>(term.EntityName ?? term.Name);
                var obj = ExpressionHelper.Parameter<TEntity>();

                // Build LINQ expression backwards
                // query = query.OrderBy(x => x.Property)

                // x => x.Property
                var key = ExpressionHelper
                    .GetPropertyExpression(obj, propertyInfo);
                var keySelector = ExpressionHelper
                    .GetLambda(typeof(TEntity), propertyInfo.PropertyType, obj, key);

                // query.OrderBy/ThenBy[Descending](x => x.Property)
                modifiedQuery = ExpressionHelper
                    .CallOrderByOrThenBy(modifiedQuery, useThenBy, term.Descending, propertyInfo.PropertyType, keySelector);

                useThenBy = true;
            }

            return modifiedQuery;
        }

        // Now using reflection find all the properties which have been marked valid using SortableAttribute
        private static IEnumerable<SortTerm> GetTermsFromModel()
            => typeof(T).GetTypeInfo()
            .DeclaredProperties
            .Where(p => p.GetCustomAttributes<SortableAttribute>().Any())
            .Select(p => new SortTerm
            {
                Name = p.Name,
                EntityName = p.GetCustomAttribute<SortableAttribute>().EntityProperty,
                Default = p.GetCustomAttribute<SortableAttribute>().Default
            });
    }
}
