using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gma.CodeCloud.Base.FileIO
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// * - Zero or more characters.
    /// ? - Exactly one character.
    /// </remarks>
    internal class RegExBasedPatternMatcher
    {

         public static bool MatchesAny(IEnumerable<string> expressions, string input)
        {
            foreach (string expression in expressions)
            {
                if (Matches(expression, input))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool MatchesAll(IEnumerable<string> expressions, string input)
        {
            foreach (string expression in expressions)
            {
                if (!Matches(expression, input))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Matches(string expression, string input)
        {
            Regex regex = Mask2RegEx(expression);
            return regex.IsMatch(input);
        }

        private static Regex Mask2RegEx(string expression)
        {
            return new Regex(
                '^' +
                expression
                    .Replace(".", "[.]")
                    .Replace("*", ".*")
                    .Replace("?", ".")
                + '$',
                RegexOptions.IgnoreCase);
        }
    }
}
