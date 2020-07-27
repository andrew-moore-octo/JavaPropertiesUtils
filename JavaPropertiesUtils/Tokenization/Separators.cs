using Superpower;
using Superpower.Parsers;

namespace JavaPropertiesUtils.Tokenization
{
    public static class Separators
    {
        public static readonly TextParser<TokenType> Parser =
            from leading in Character.In(' ', '\t').Many()
            from separator in Character.In(':', '=').Optional()
            from trailing in Character.In(' ', '\t').Many()
            select TokenType.Separator;
    }
}