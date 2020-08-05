using Superpower;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Comments
    {
        public static readonly TextParser<TokenType> Parser =
            from delimiter in Character.In('!', '#')
            from text in Common.UntilNewLineParser
            select TokenType.Comment;
    }
}