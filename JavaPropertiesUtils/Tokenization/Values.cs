using Superpower;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Values
    {
        public static readonly TextParser<TokenType> Parser = Common.UntilNewLineParser.Value(TokenType.Value);
    }
}