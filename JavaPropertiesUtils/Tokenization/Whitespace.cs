using Superpower;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Whitespace
    {
        public static readonly TextParser<TokenType> Parser = Span
            .WithAll(c => c == ' ' || c == '\t')
            .Value(TokenType.Whitespace);
    }
}