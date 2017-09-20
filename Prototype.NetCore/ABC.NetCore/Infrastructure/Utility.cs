using System;
using System.Collections.Generic;
using System.Text;

namespace ABC.NetCore.Infrastructure
{
    public static class Utility
    {
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var first = input.Substring(0, 1).ToLower();
            if (input.Length == 1) return first;

            return first + input.Substring(1);
        }
    }
}
