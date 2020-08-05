using System.Collections.Generic;
using System.Linq;

namespace JavaPropertiesUtils.Expressions
{
    public class StringValue
    {
        public StringValue(string serializableValue, string logicalValue)
        {
            SerializableValue = serializableValue;
            LogicalValue = logicalValue;
        }
        
        public string SerializableValue { get; }
        
        public string LogicalValue { get; }

        protected bool Equals(StringValue other)
        {
            return SerializableValue == other.SerializableValue && LogicalValue == other.LogicalValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StringValue) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SerializableValue != null ? SerializableValue.GetHashCode() : 0) * 397) ^ (LogicalValue != null ? LogicalValue.GetHashCode() : 0);
            }
        }
    }

    public static class StringValues
    {
        public static StringValue Join(IEnumerable<StringValue> values)
        {
            var stringValues = values as StringValue[] ?? values.ToArray();
            
            var serializable = string.Join("", stringValues.Select(v => v.SerializableValue));
            var logical = string.Join("", stringValues.Select(v => v.LogicalValue));
            return new StringValue(serializable, logical);
        }
    }
}