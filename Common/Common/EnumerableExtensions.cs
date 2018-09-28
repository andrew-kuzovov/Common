using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guards;

namespace KUtil.Common
{
    public static class EnumerableExtensions
    {
        public static string Join(this IEnumerable<string> source, string separator)
        {
            Guard.CheckNotNull(source, nameof(source));
            Guard.CheckNotEmpty(separator, nameof(separator));

            return string.Join(separator, source);
        }

        public static string Join<TSource>(this IEnumerable<TSource> source, string separator)
        {
            Guard.CheckNotNull(source, nameof(source));
            Guard.CheckNotEmpty(separator, nameof(separator));

            return source.Aggregate(
                new StringBuilder(),
                (s, next) => s.Append(next.ToString()).Append(separator),
                s => s.ToString(0, s.Length > 0 ? s.Length - separator.Length : 0));
        }
    }
}
