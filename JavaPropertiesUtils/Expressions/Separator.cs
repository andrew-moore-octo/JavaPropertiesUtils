using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class Separator : IExpression
    {
        public Separator(Token<TokenType> token)
        {
            Content = token.ToStringValue();
        }
        
        public string Content { get; }

        public override string ToString()
        {
            return Content;
        }

        protected bool Equals(Separator other)
        {
            return Content == other.Content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Separator) obj);
        }

        public override int GetHashCode()
        {
            return Content != null ? Content.GetHashCode() : 0;
        }
    }
}