using System;
using System.Collections.Generic;
using System.Text;

using ABC.NetCore.Infrastructure;

namespace ABC.NetCore.Models
{
    public class SearchTerm
    {
        public string Name { get; set; }

        public string Operator { get; set; }

        public string Value { get; set; }

        public bool ValidSyntax { get; set; }

        public ISearchExpressionProvider ExpressionProvider { get; set; }
    }
}
