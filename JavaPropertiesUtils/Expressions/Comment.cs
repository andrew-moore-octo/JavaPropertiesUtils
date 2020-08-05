using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class Comment : ITopLevelExpression
    {
        public Comment(Token<TokenType> token) : this(token.Span.ToStringValue())
        {
        }

        public Comment(string content)
        {
            Content = content;
        }
        
        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }

        protected bool Equals(Comment other)
        {
            return Content == other.Content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Comment) obj);
        }

        public override int GetHashCode()
        {
            return Content != null ? Content.GetHashCode() : 0;
        }
    }
}