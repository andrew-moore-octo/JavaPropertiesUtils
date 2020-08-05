using Superpower;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Whitespace
    {
        public static readonly TextParser<TokenType> Parser = Character.In(' ', '\t', '\r', '\n')
            .AtLeastOnce()
            .Value(TokenType.Whitespace);
    }
}