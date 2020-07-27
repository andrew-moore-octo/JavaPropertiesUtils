using System.Linq;
using Superpower;
using Superpower.Model;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Utils
{
    public static class Tokens
    {
        public static TokenListParser<TKind, Token<TKind>> In<TKind>(params TKind[] kinds)
        {
            return kinds
                .Select(Token.EqualTo)
                .Aggregate((prev, current) => prev.Or(current));
        }
    }
}