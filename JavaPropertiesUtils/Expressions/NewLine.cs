using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class NewLine : ITopLevelExpression
    {
        public NewLine(Token<TokenType> token)
        {
            Content = token.ToStringValue();
        }

        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }
    }
}