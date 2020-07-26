using System;
using System.Collections.Generic;
using System.Linq;
using Superpower.Model;

namespace JavaPropertiesUtils.Utils
{
    public static class TextSpans
    {
        /// <summary>
        /// Joins multiple spans together, producing a single span
        /// that covers all of the input spans.
        /// </summary>
        public static TextSpan Join(params TextSpan[] spans)
        {
            if (spans == null) throw new ArgumentNullException(nameof(spans));
            if (spans.Length == 1) return spans[0];
            
            var first = spans.OrderBy(s => s.Position.Absolute).First();
            var last = spans.OrderByDescending(s => s.Position.Absolute + s.Length).First();
            var length = last.Position.Absolute + last.Length - first.Position.Absolute;
            
            return new TextSpan(
                first.Source,
                first.Position,
                length
            );
        }
        
        /// <summary>
        /// Joins multiple spans together, producing a single span
        /// that covers all of the input spans.
        /// </summary>
        public static TextSpan Join(IEnumerable<TextSpan> spans)
        {
            return Join(spans.ToArray());
        }
    }
}