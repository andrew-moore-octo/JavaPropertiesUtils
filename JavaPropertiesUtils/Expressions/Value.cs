using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class Value : IExpression
    {
        // TODO: arguments may change once we have multiline support.
        public Value(Token<TokenType> token)
        {
            Content = token.ToStringValue();
            // noship
            EscapedContent = token.ToStringValue();
        }

        public Value(string unescapedValue)
        {
            Content = unescapedValue;
            // noship
            EscapedContent = unescapedValue
                // TODO: handle unix newlines
                // TODO: nicer indentation?
                // noship
                .Replace("\r\n", "\\\r\n");
        }

        public string Content { get; }

        public string EscapedContent { get; }

        public override string ToString()
        {
            return EscapedContent;
        }
    }
}