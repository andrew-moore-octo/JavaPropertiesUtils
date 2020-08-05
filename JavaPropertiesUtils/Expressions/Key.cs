using System.Linq;
using Superpower.Model;

namespace JavaPropertiesUtils.Expressions
{
    public class Key : IExpression
    {
        public Key(params Token<TokenType>[] parts)
        {
            var values = parts.Select(KeyComponents.GetStringValue);
            Value = StringValues.Join(values);
        }

        public StringValue Value { get; }

        public override string ToString()
        {
            return Value.SerializableValue;
        }

        protected bool Equals(Key other)
        {
            return Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Key) obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }
}