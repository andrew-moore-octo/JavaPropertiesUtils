using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class WhiteSpace : ITopLevelExpression
    {
        public WhiteSpace(Token<TokenType> token) : this(token.ToStringValue())
        {
        }

        public WhiteSpace(string content)
        {
            Content = content;
        }

        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }

        protected bool Equals(WhiteSpace other)
        {
            return Content == other.Content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((WhiteSpace) obj);
        }

        public override int GetHashCode()
        {
            return Content != null ? Content.GetHashCode() : 0;
        }
    }
}