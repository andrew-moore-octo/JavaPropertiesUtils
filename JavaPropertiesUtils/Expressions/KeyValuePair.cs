namespace JavaPropertiesUtils.Expressions
{
    public class KeyValuePair : ITopLevelExpression
    {
        public KeyValuePair(Key key, Separator separator, Value value)
        {
            Key = key;
            Separator = separator;
            Value = value;
        }
        
        public Key Key { get; }
        
        public Separator Separator { get; }
        
        public Value Value { get; }

        public override string ToString()
        {
            return Key.ToString() + Separator + Value;
        }

        protected bool Equals(KeyValuePair other)
        {
            return Equals(Key, other.Key) && Equals(Separator, other.Separator) && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((KeyValuePair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Key != null ? Key.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Separator != null ? Separator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}