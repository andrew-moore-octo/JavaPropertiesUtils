using Superpower;

namespace JavaPropertiesUtils.Tokenization
{
    public static class NewLines
    {
        public static readonly TextParser<TokenType> Parser = Common.NewLineParser.Value(TokenType.Whitespace);
    }
}